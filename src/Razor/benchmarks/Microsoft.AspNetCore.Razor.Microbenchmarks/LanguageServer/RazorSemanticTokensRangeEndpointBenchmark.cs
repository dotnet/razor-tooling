﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Semantic;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.Microbenchmarks.LanguageServer;

public class RazorSemanticTokensRangeEndpointBenchmark : RazorLanguageServerBenchmarkBase
{
    private IRazorSemanticTokensInfoService RazorSemanticTokenService { get; set; }

    private SemanticTokensRangeEndpoint SemanticTokensRangeEndpoint { get; set; }

    private DocumentVersionCache VersionCache { get; set; }

    private Uri DocumentUri => DocumentContext.Uri;

    private IDocumentSnapshot DocumentSnapshot => DocumentContext.Snapshot;

    private VersionedDocumentContext DocumentContext { get; set; }

    private Range Range { get; set; }

    private new IRazorLogger Logger { get; set; }

    private ProjectSnapshotManagerDispatcher ProjectSnapshotManagerDispatcher { get; set; }

    private string PagesDirectory { get; set; }

    private string ProjectFilePath { get; set; }

    private string TargetPath { get; set; }

    private CancellationToken CancellationToken { get; set; }

    private RazorRequestContext RequestContext { get; set; }

    [Params(0, 100, 1000)]
    public int CsSemanticRangesToReturn { get; set; }

    private static List<SemanticRange> PregeneratedRandomSemanticRanges { get; set; }

    [GlobalSetup]
    public async Task InitializeRazorSemanticAsync()
    {
        EnsureServicesInitialized();

        var projectRoot = Path.Combine(RepoRoot, "src", "Razor", "test", "testapps", "ComponentApp");
        ProjectFilePath = Path.Combine(projectRoot, "ComponentApp.csproj");
        PagesDirectory = Path.Combine(projectRoot, "Components", "Pages");

        var largeFileText = Resources.GetResourceText("MSN.cshtml");

        var largeFilePath = Path.Combine(PagesDirectory, $"MSN.cshtml");
        var documentUri = new Uri(largeFilePath);

        TargetPath = "/Components/Pages/MSN.cshtml";

        var documentSnapshot = GetDocumentSnapshot(ProjectFilePath, largeFilePath, largeFileText, TargetPath);
        var version = 1;
        DocumentContext = new VersionedDocumentContext(documentUri, documentSnapshot, projectContext: null, version);
        Logger = new NoopLogger();
        SemanticTokensRangeEndpoint = new SemanticTokensRangeEndpoint(telemetryReporter: null);
        SemanticTokensRangeEndpoint.ApplyCapabilities(new(), new VSInternalClientCapabilities() { SupportsVisualStudioExtensions = true });

        var text = await DocumentContext.GetSourceTextAsync(CancellationToken.None).ConfigureAwait(false);
        Range = new Range
        {
            Start = new Position { Line = 0, Character = 0 },
            End = new Position { Line = text.Lines.Count - 1, Character = text.Lines.Last().Span.Length - 1 }
        };

        var documentVersion = 1;
        CancellationToken = CancellationToken.None;
        await UpdateDocumentAsync(documentVersion, DocumentSnapshot, CancellationToken).ConfigureAwait(false);

        var languageServer = RazorLanguageServer.GetInnerLanguageServerForTesting();
        RequestContext = new RazorRequestContext(DocumentContext, Logger, languageServer.GetLspServices());

        var random = new Random();
        var codeDocument = await DocumentContext.GetCodeDocumentAsync(CancellationToken);
        var pooledList = ListPool<SemanticRange>.GetPooledObject(out var pregeneratedRandomSemanticRanges);
        pregeneratedRandomSemanticRanges.Capacity = CsSemanticRangesToReturn;
        for (var i = 0; i < CsSemanticRangesToReturn; i++)
        {
            var startLine = random.Next(Range.Start.Line, Range.End.Line);
            var startChar = random.Next(0, codeDocument.Source.Lines.GetLineLength(startLine));
            var endLine = random.Next(startLine, Range.End.Line);
            var endChar = startLine == endLine
                ? random.Next(startChar + 1, codeDocument.Source.Lines.GetLineLength(startLine))
                : random.Next(0, codeDocument.Source.Lines.GetLineLength(endLine));

            pregeneratedRandomSemanticRanges.Add(
                new SemanticRange(random.Next(),
                    new Range { Start = new Position(startLine, startChar), End = new Position(endLine, endChar) },
                    modifier: 0, fromRazor: false));
        }

        pregeneratedRandomSemanticRanges.Sort();
        PregeneratedRandomSemanticRanges = pregeneratedRandomSemanticRanges;
    }

    [Benchmark(Description = "Razor Semantic Tokens Range Endpoint")]
    public async Task RazorSemanticTokensRangeEndpointRangesAsync()
    {
        var textDocumentIdentifier = new TextDocumentIdentifier { Uri = DocumentUri };
        var request = new SemanticTokensRangeParams { Range = Range, TextDocument = textDocumentIdentifier };

        await SemanticTokensRangeEndpoint.HandleRequestAsync(request, RequestContext, CancellationToken);
    }

    private async Task UpdateDocumentAsync(int newVersion, IDocumentSnapshot documentSnapshot,
        CancellationToken cancellationToken)
    {
        await ProjectSnapshotManagerDispatcher.RunOnDispatcherThreadAsync(
                () => VersionCache.TrackDocumentVersion(documentSnapshot, newVersion), cancellationToken)
            .ConfigureAwait(false);
    }

    [GlobalCleanup]
    public async Task CleanupServerAsync()
    {
        var innerServer = RazorLanguageServer.GetInnerLanguageServerForTesting();
        await innerServer.ShutdownAsync();
        await innerServer.ExitAsync();
    }

    protected internal override void Builder(IServiceCollection collection)
    {
        collection.AddSingleton<IRazorSemanticTokensInfoService, TestCustomizableRazorSemanticTokensInfoService>();
    }

    private void EnsureServicesInitialized()
    {
        var languageServer = RazorLanguageServer.GetInnerLanguageServerForTesting();
        RazorSemanticTokenService = languageServer.GetRequiredService<IRazorSemanticTokensInfoService>();
        VersionCache = languageServer.GetRequiredService<DocumentVersionCache>();
        ProjectSnapshotManagerDispatcher = languageServer.GetRequiredService<ProjectSnapshotManagerDispatcher>();
    }

    internal class TestCustomizableRazorSemanticTokensInfoService : RazorSemanticTokensInfoService
    {
        public TestCustomizableRazorSemanticTokensInfoService(
            ClientNotifierServiceBase languageServer,
            IRazorDocumentMappingService documentMappingService,
            RazorLSPOptionsMonitor razorLSPOptionsMonitor,
            ILoggerFactory loggerFactory)
            : base(languageServer, documentMappingService, razorLSPOptionsMonitor, loggerFactory)
        {
        }

        // We can't get C# responses without significant amounts of extra work, so let's just shim it for now, any non-Null result is fine.
        internal override Task<PooledObject<List<SemanticRange>>?> GetCSharpSemanticRangesAsync(
            RazorCodeDocument codeDocument,
            TextDocumentIdentifier textDocumentIdentifier,
            Range razorRange,
            RazorSemanticTokensLegend razorSemanticTokensLegend,
            long documentVersion,
            Guid correlationId,
            CancellationToken cancellationToken,
            string previousResultId = null)
        {
            var pooledList = ListPool<SemanticRange>.GetPooledObject(out var randomSemanticRanges);
            randomSemanticRanges.Capacity = PregeneratedRandomSemanticRanges.Count;
            randomSemanticRanges.AddRange(PregeneratedRandomSemanticRanges);

            return Task.FromResult<PooledObject<List<SemanticRange>>?>(pooledList);
        }
    }
}
