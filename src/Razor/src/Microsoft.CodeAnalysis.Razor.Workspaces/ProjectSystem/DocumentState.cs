﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal sealed partial class DocumentState
{
    private static readonly LoadTextOptions s_loadTextOptions = new(SourceHashAlgorithm.Sha256);

    private static readonly TextAndVersion s_emptyTextAndVersion = TextAndVersion.Create(
        SourceText.From(string.Empty),
        VersionStamp.Default);

    public static readonly TextLoader EmptyLoader = TextLoader.From(s_emptyTextAndVersion);

    public HostDocument HostDocument { get; }
    public int Version { get; }

    private TextAndVersion? _textAndVersion;
    private readonly TextLoader _textLoader;

    private ComputedStateTracker? _computedState;

    private DocumentState(
        HostDocument hostDocument,
        TextAndVersion? textAndVersion,
        TextLoader? textLoader)
    {
        HostDocument = hostDocument;
        Version = 1;

        _textAndVersion = textAndVersion;
        _textLoader = textLoader ?? EmptyLoader;
    }

    private DocumentState(
        DocumentState oldState,
        TextAndVersion? textAndVersion,
        TextLoader? textLoader)
    {
        HostDocument = oldState.HostDocument;
        Version = oldState.Version + 1;

        _textAndVersion = textAndVersion;
        _textLoader = textLoader ?? EmptyLoader;
    }

    public static DocumentState Create(HostDocument hostDocument, SourceText text)
        => new(hostDocument, TextAndVersion.Create(text, VersionStamp.Create()), textLoader: null);

    public static DocumentState Create(HostDocument hostDocument, TextLoader loader)
        => new(hostDocument, textAndVersion: null, loader);

    public bool IsGeneratedOutputResultAvailable => ComputedState.IsResultAvailable;

    private ComputedStateTracker ComputedState
        => _computedState ??= InterlockedOperations.Initialize(ref _computedState, new ComputedStateTracker());

    public bool TryGetGeneratedOutputAndVersion(out (RazorCodeDocument output, VersionStamp inputVersion) result)
    {
        return ComputedState.TryGetGeneratedOutputAndVersion(out result);
    }

    public Task<(RazorCodeDocument output, VersionStamp inputVersion)> GetGeneratedOutputAndVersionAsync(
        ProjectSnapshot project,
        DocumentSnapshot document,
        CancellationToken cancellationToken)
    {
        return ComputedState.GetGeneratedOutputAndVersionAsync(project, document, cancellationToken);
    }

    public ValueTask<TextAndVersion> GetTextAndVersionAsync(CancellationToken cancellationToken)
    {
        return TryGetTextAndVersion(out var result)
            ? new(result)
            : LoadTextAndVersionAsync(_textLoader, cancellationToken);

        async ValueTask<TextAndVersion> LoadTextAndVersionAsync(TextLoader loader, CancellationToken cancellationToken)
        {
            var textAndVersion = await loader
                .LoadTextAndVersionAsync(s_loadTextOptions, cancellationToken)
                .ConfigureAwait(false);

            return InterlockedOperations.Initialize(ref _textAndVersion, textAndVersion);
        }
    }

    public ValueTask<SourceText> GetTextAsync(CancellationToken cancellationToken)
    {
        return TryGetText(out var text)
            ? new(text)
            : GetTextCoreAsync(cancellationToken);

        async ValueTask<SourceText> GetTextCoreAsync(CancellationToken cancellationToken)
        {
            var textAsVersion = await GetTextAndVersionAsync(cancellationToken).ConfigureAwait(false);

            return textAsVersion.Text;
        }
    }

    public ValueTask<VersionStamp> GetTextVersionAsync(CancellationToken cancellationToken)
    {
        return TryGetTextVersion(out var version)
            ? new(version)
            : GetTextVersionCoreAsync(cancellationToken);

        async ValueTask<VersionStamp> GetTextVersionCoreAsync(CancellationToken cancellationToken)
        {
            var textAsVersion = await GetTextAndVersionAsync(cancellationToken).ConfigureAwait(false);

            return textAsVersion.Version;
        }
    }

    public bool TryGetTextAndVersion([NotNullWhen(true)] out TextAndVersion? result)
    {
        result = _textAndVersion;
        return result is not null;
    }

    public bool TryGetText([NotNullWhen(true)] out SourceText? result)
    {
        if (TryGetTextAndVersion(out var textAndVersion))
        {
            result = textAndVersion.Text;
            return true;
        }

        result = null;
        return false;
    }

    public bool TryGetTextVersion(out VersionStamp result)
    {
        if (TryGetTextAndVersion(out var textAndVersion))
        {
            result = textAndVersion.Version;
            return true;
        }

        result = default;
        return false;
    }

    public DocumentState WithConfigurationChange()
    {
        // Do not cache computed state
        return new(this, _textAndVersion, _textLoader);
    }

    public DocumentState WithImportsChange()
    {
        var state = new DocumentState(this, _textAndVersion, _textLoader);

        // Optimistically cache the computed state
        state._computedState = new ComputedStateTracker(_computedState);

        return state;
    }

    public DocumentState WithProjectWorkspaceStateChange()
    {
        var state = new DocumentState(this, _textAndVersion, _textLoader);

        // Optimistically cache the computed state
        state._computedState = new ComputedStateTracker(_computedState);

        return state;
    }

    public DocumentState WithText(SourceText text, VersionStamp textVersion)
    {
        // Do not cache the computed state
        return new(this, TextAndVersion.Create(text, textVersion), textLoader: null);
    }

    public DocumentState WithTextLoader(TextLoader textLoader)
    {
        // Do not cache the computed state
        return new(this, textAndVersion: null, textLoader);
    }

    internal static async Task<RazorCodeDocument> GenerateCodeDocumentAsync(
        IDocumentSnapshot document,
        RazorProjectEngine projectEngine,
        bool forceRuntimeCodeGeneration,
        CancellationToken cancellationToken)
    {
        var importItems = await GetImportItemsAsync(document, projectEngine, cancellationToken).ConfigureAwait(false);

        return await GenerateCodeDocumentAsync(
            document, projectEngine, importItems, forceRuntimeCodeGeneration, cancellationToken).ConfigureAwait(false);
    }

    private static async Task<RazorCodeDocument> GenerateCodeDocumentAsync(
        IDocumentSnapshot document,
        RazorProjectEngine projectEngine,
        ImmutableArray<ImportItem> imports,
        bool forceRuntimeCodeGeneration,
        CancellationToken cancellationToken)
    {
        var importSources = GetImportSources(imports, projectEngine);
        var tagHelpers = await document.Project.GetTagHelpersAsync(cancellationToken).ConfigureAwait(false);
        var source = await GetSourceAsync(document, projectEngine, cancellationToken).ConfigureAwait(false);

        return forceRuntimeCodeGeneration
            ? projectEngine.Process(source, document.FileKind, importSources, tagHelpers)
            : projectEngine.ProcessDesignTime(source, document.FileKind, importSources, tagHelpers);
    }

    private static async Task<ImmutableArray<ImportItem>> GetImportItemsAsync(IDocumentSnapshot document, RazorProjectEngine projectEngine, CancellationToken cancellationToken)
    {
        var projectItem = projectEngine.FileSystem.GetItem(document.FilePath, document.FileKind);

        using var importProjectItems = new PooledArrayBuilder<RazorProjectItem>();

        foreach (var feature in projectEngine.ProjectFeatures.OfType<IImportProjectFeature>())
        {
            if (feature.GetImports(projectItem) is { } featureImports)
            {
                importProjectItems.AddRange(featureImports);
            }
        }

        if (importProjectItems.Count == 0)
        {
            return [];
        }

        var project = document.Project;

        using var importItems = new PooledArrayBuilder<ImportItem>(capacity: importProjectItems.Count);

        foreach (var importProjectItem in importProjectItems)
        {
            if (importProjectItem is NotFoundProjectItem)
            {
                continue;
            }

            if (importProjectItem.PhysicalPath is null)
            {
                // This is a default import.
                using var stream = importProjectItem.Read();
                var text = SourceText.From(stream);
                var defaultImport = ImportItem.CreateDefault(text);

                importItems.Add(defaultImport);
            }
            else if (project.TryGetDocument(importProjectItem.PhysicalPath, out var importDocument))
            {
                var text = await importDocument.GetTextAsync(cancellationToken).ConfigureAwait(false);
                var versionStamp = await importDocument.GetTextVersionAsync(cancellationToken).ConfigureAwait(false);
                var importItem = new ImportItem(importDocument.FilePath, importDocument.FileKind, text, versionStamp);

                importItems.Add(importItem);
            }
        }

        return importItems.DrainToImmutable();
    }

    private static ImmutableArray<RazorSourceDocument> GetImportSources(ImmutableArray<ImportItem> importItems, RazorProjectEngine projectEngine)
    {
        using var importSources = new PooledArrayBuilder<RazorSourceDocument>(importItems.Length);

        foreach (var importItem in importItems)
        {
            var importProjectItem = importItem is { FilePath: string filePath, FileKind: var fileKind }
                ? projectEngine.FileSystem.GetItem(filePath, fileKind)
                : null;

            var properties = RazorSourceDocumentProperties.Create(importItem.FilePath, importProjectItem?.RelativePhysicalPath);
            var importSource = RazorSourceDocument.Create(importItem.Text, properties);

            importSources.Add(importSource);
        }

        return importSources.DrainToImmutable();
    }

    private static async Task<RazorSourceDocument> GetSourceAsync(IDocumentSnapshot document, RazorProjectEngine projectEngine, CancellationToken cancellationToken)
    {
        var projectItem = document is { FilePath: string filePath, FileKind: var fileKind }
            ? projectEngine.FileSystem.GetItem(filePath, fileKind)
            : null;

        var text = await document.GetTextAsync(cancellationToken).ConfigureAwait(false);
        var properties = RazorSourceDocumentProperties.Create(document.FilePath, projectItem?.RelativePhysicalPath);
        return RazorSourceDocument.Create(text, properties);
    }
}
