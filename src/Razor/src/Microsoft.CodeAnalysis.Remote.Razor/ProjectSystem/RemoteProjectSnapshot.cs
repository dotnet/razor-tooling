﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectEngineHost;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Threading;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.NET.Sdk.Razor.SourceGenerators;

namespace Microsoft.CodeAnalysis.Remote.Razor.ProjectSystem;

internal sealed class RemoteProjectSnapshot : IProjectSnapshot
{
    public RemoteSolutionSnapshot SolutionSnapshot { get; }

    public ProjectKey Key { get; }

    private readonly Project _project;
#if !FORMAT_FUSE
    private readonly AsyncLazy<RazorConfiguration> _lazyConfiguration;
    private readonly AsyncLazy<RazorProjectEngine> _lazyProjectEngine;
#endif
    private readonly Dictionary<TextDocument, RemoteDocumentSnapshot> _documentMap = [];

    public RemoteProjectSnapshot(Project project, RemoteSolutionSnapshot solutionSnapshot)
    {
        if (!project.ContainsRazorDocuments())
        {
            throw new ArgumentException(SR.Project_does_not_contain_any_Razor_documents, nameof(project));
        }

        _project = project;
        SolutionSnapshot = solutionSnapshot;
        Key = _project.ToProjectKey();

#if !FORMAT_FUSE
        _lazyConfiguration = AsyncLazy.Create(ComputeConfigurationAsync);
        _lazyProjectEngine = AsyncLazy.Create(ComputeProjectEngineAsync);
#endif
    }

    public IEnumerable<string> DocumentFilePaths
        => _project.AdditionalDocuments
            .Where(static d => d.IsRazorDocument())
            .Select(static d => d.FilePath.AssumeNotNull());

    public string FilePath => _project.FilePath.AssumeNotNull();

    public string IntermediateOutputPath => FilePathNormalizer.GetNormalizedDirectoryName(_project.CompilationOutputInfo.AssemblyPath);

    public string? RootNamespace => _project.DefaultNamespace ?? "ASP";

    public string DisplayName => _project.Name;

    public Project Project => _project;

    public LanguageVersion CSharpLanguageVersion => ((CSharpParseOptions)_project.ParseOptions.AssumeNotNull()).LanguageVersion;

    public async ValueTask<ImmutableArray<TagHelperDescriptor>> GetTagHelpersAsync(CancellationToken cancellationToken)
    {
        var generatorResult = await GetRazorGeneratorResultAsync(cancellationToken).ConfigureAwait(false);
        if (generatorResult is null)
            return [];

        return [.. generatorResult.TagHelpers];
    }

    public RemoteDocumentSnapshot GetDocument(DocumentId documentId)
    {
        var document = _project.GetRequiredDocument(documentId);
        return GetDocument(document);
    }

    public RemoteDocumentSnapshot GetDocument(TextDocument document)
    {
        if (document.Project != _project)
        {
            throw new ArgumentException(SR.Document_does_not_belong_to_this_project, nameof(document));
        }

        if (!document.IsRazorDocument())
        {
            throw new ArgumentException(SR.Document_is_not_a_Razor_document);
        }

        return GetDocumentCore(document);
    }

    private RemoteDocumentSnapshot GetDocumentCore(TextDocument document)
    {
        lock (_documentMap)
        {
            if (!_documentMap.TryGetValue(document, out var snapshot))
            {
                snapshot = new RemoteDocumentSnapshot(document, this);
                _documentMap.Add(document, snapshot);
            }

            return snapshot;
        }
    }

    public bool ContainsDocument(string filePath)
    {
        if (!filePath.IsRazorFilePath())
        {
            throw new ArgumentException(SR.Format0_is_not_a_Razor_file_path(filePath), nameof(filePath));
        }

        var documentIds = _project.Solution.GetDocumentIdsWithFilePath(filePath);

        foreach (var documentId in documentIds)
        {
            if (_project.Id == documentId.ProjectId &&
                _project.ContainsAdditionalDocument(documentId))
            {
                return true;
            }
        }

        return false;
    }

    public bool TryGetDocument(string filePath, [NotNullWhen(true)] out IDocumentSnapshot? document)
    {
        if (!filePath.IsRazorFilePath())
        {
            throw new ArgumentException(SR.Format0_is_not_a_Razor_file_path(filePath), nameof(filePath));
        }

        var documentIds = _project.Solution.GetDocumentIdsWithFilePath(filePath);

        foreach (var documentId in documentIds)
        {
            if (_project.Id == documentId.ProjectId &&
                _project.GetAdditionalDocument(documentId) is { } doc)
            {
                document = GetDocumentCore(doc);
                return true;
            }
        }

        document = null;
        return false;
    }

    internal async Task<RazorCodeDocument?> GetCodeDocumentAsync(IDocumentSnapshot documentSnapshot, CancellationToken cancellationToken)
    {
        var generatorResult = await GetRazorGeneratorResultAsync(cancellationToken).ConfigureAwait(false);
        if (generatorResult is null)
            return null;

        return generatorResult.GetCodeDocument(documentSnapshot.FilePath);
    }

    internal async Task<Document?> GetGeneratedDocumentAsync(IDocumentSnapshot documentSnapshot, CancellationToken cancellationToken)
    {
        var generatorResult = await GetRazorGeneratorResultAsync(cancellationToken).ConfigureAwait(false);
        if (generatorResult is null)
            return null;

        var hintName = generatorResult.GetHintName(documentSnapshot.FilePath);

        // TODO: use this when the location is case-insensitive on windows (https://github.com/dotnet/roslyn/issues/76869)
        //var generator = typeof(RazorSourceGenerator);
        //var generatorAssembly = generator.Assembly;
        //var generatorName = generatorAssembly.GetName();
        //var generatedDocuments = await _project.GetSourceGeneratedDocumentsForGeneratorAsync(generatorName.Name!, generatorAssembly.Location, generatorName.Version!, generator.Name, cancellationToken).ConfigureAwait(false);

        var generatedDocuments = await _project.GetSourceGeneratedDocumentsAsync(cancellationToken).ConfigureAwait(false);
        return generatedDocuments.Single(d => d.HintName == hintName);
    }

    private async Task<RazorGeneratorResult?> GetRazorGeneratorResultAsync(CancellationToken cancellationToken)
    {
        var result = await _project.GetSourceGeneratorRunResultAsync(cancellationToken).ConfigureAwait(false);
        if (result is null)
            return null;

        var runResult = result.Results.SingleOrDefault(r => r.Generator.GetGeneratorType().Assembly.Location == typeof(RazorSourceGenerator).Assembly.Location);
        if (runResult.Generator is null)
            return null;

#pragma warning disable RSEXPERIMENTAL004 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
        if (!runResult.HostOutputs.TryGetValue(nameof(RazorGeneratorResult), out var objectResult) || objectResult is not RazorGeneratorResult generatorResult)
            return null;
#pragma warning restore RSEXPERIMENTAL004 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

        return generatorResult;
    }

#if !FORMAT_FUSE
    /// <summary>
    /// NOTE: This will be removed when the source generator is used directly.
    /// </summary>
    public ValueTask<RazorProjectEngine> GetProjectEngineAsync(CancellationToken cancellationToken)
    {
        if (_lazyProjectEngine.TryGetValue(out var result))
        {
            return new(result);
        }

        return new(_lazyProjectEngine.GetValueAsync(cancellationToken));
    }

    private async Task<RazorConfiguration> ComputeConfigurationAsync(CancellationToken cancellationToken)
    {
        var compilation = await _project.GetCompilationAsync(cancellationToken).ConfigureAwait(false);

        return RazorProjectInfoFactory.ComputeRazorConfigurationOptions(_project, compilation, out _);
    }

    private async Task<RazorProjectEngine> ComputeProjectEngineAsync(CancellationToken cancellationToken)
    {
        var configuration = await _lazyConfiguration.GetValueAsync(cancellationToken).ConfigureAwait(false);

        var useRoslynTokenizer = configuration.UseRoslynTokenizer;
        var parseOptions = new CSharpParseOptions(languageVersion: CSharpLanguageVersion, preprocessorSymbols: configuration.PreprocessorSymbols);

        return ProjectEngineFactories.DefaultProvider.Create(
            configuration,
            rootDirectoryPath: Path.GetDirectoryName(FilePath).AssumeNotNull(),
            configure: builder =>
            {
                builder.SetRootNamespace(RootNamespace);
                builder.SetCSharpLanguageVersion(CSharpLanguageVersion);
                builder.SetSupportLocalizedComponentNames();
                builder.Features.Add(new ConfigureRazorParserOptions(useRoslynTokenizer, parseOptions));
            });
    }
#endif
}
