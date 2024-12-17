﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Razor.ProjectSystem.Legacy;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal sealed class ProjectSnapshot(ProjectState state) : IProjectSnapshot, ILegacyProjectSnapshot
{
    private readonly ProjectState _state = state;

    private readonly object _gate = new();
    private readonly Dictionary<string, DocumentSnapshot> _filePathToDocumentMap = new(FilePathNormalizingComparer.Instance);

    public HostProject HostProject => _state.HostProject;
    public RazorCompilerOptions CompilerOptions => _state.CompilerOptions;

    public ProjectKey Key => _state.HostProject.Key;
    public RazorConfiguration Configuration => _state.HostProject.Configuration;
    public IEnumerable<string> DocumentFilePaths => _state.Documents.Keys;
    public string FilePath => _state.HostProject.FilePath;
    public string IntermediateOutputPath => _state.HostProject.IntermediateOutputPath;
    public string? RootNamespace => _state.HostProject.RootNamespace;
    public string DisplayName => _state.HostProject.DisplayName;
    public LanguageVersion CSharpLanguageVersion => _state.CSharpLanguageVersion;
    public ProjectWorkspaceState ProjectWorkspaceState => _state.ProjectWorkspaceState;

    public int DocumentCount => _state.Documents.Count;

    public RazorProjectEngine GetProjectEngine()
        => _state.ProjectEngine;

    public ValueTask<ImmutableArray<TagHelperDescriptor>> GetTagHelpersAsync(CancellationToken cancellationToken)
        => new(_state.TagHelpers);

    public bool ContainsDocument(string filePath)
    {
        lock (_gate)
        {
            // PERF: It's intentional that we call _filePathToDocumentMap.ContainsKey(...)
            // before State.Documents.ContainsKey(...), even though the latter check is
            // enough to return the correct answer. This is because _filePathToDocumentMap is
            // a Dictionary<,>, which has O(1) lookup, and State.Documents is an
            // ImmutableDictionary<,>, which has O(log n) lookup. So, checking _filePathToDocumentMap
            // first is faster if the DocumentSnapshot has already been created.

            return _filePathToDocumentMap.ContainsKey(filePath) ||
                   _state.Documents.ContainsKey(filePath);
        }
    }

    public bool TryGetDocument(string filePath, [NotNullWhen(true)] out DocumentSnapshot? document)
    {
        lock (_gate)
        {
            // Have we already seen this document? If so, return it!
            if (_filePathToDocumentMap.TryGetValue(filePath, out var snapshot))
            {
                document = snapshot;
                return true;
            }

            // Do we have DocumentSate for this document? If not, we're done!
            if (!_state.Documents.TryGetValue(filePath, out var state))
            {
                document = null;
                return false;
            }

            // If we have DocumentState, go ahead and create a new DocumentSnapshot.
            snapshot = new DocumentSnapshot(this, state);
            _filePathToDocumentMap.Add(filePath, snapshot);

            document = snapshot;
            return true;
        }
    }

    bool IProjectSnapshot.TryGetDocument(string filePath, [NotNullWhen(true)] out IDocumentSnapshot? document)
    {
        if (TryGetDocument(filePath, out var result))
        {
            document = result;
            return true;
        }

        document = null;
        return false;
    }

    /// <summary>
    /// If the provided document is an import document, gets the other documents in the project
    /// that include directives specified by the provided document. Otherwise returns an empty
    /// list.
    /// </summary>
    public ImmutableArray<IDocumentSnapshot> GetRelatedDocuments(IDocumentSnapshot document)
    {
        var targetPath = document.TargetPath;

        if (!_state.ImportsToRelatedDocuments.TryGetValue(targetPath, out var relatedDocuments))
        {
            return [];
        }

        lock (_gate)
        {
            using var builder = new PooledArrayBuilder<IDocumentSnapshot>(capacity: relatedDocuments.Count);

            foreach (var relatedDocumentFilePath in relatedDocuments)
            {
                if (TryGetDocument(relatedDocumentFilePath, out var relatedDocument))
                {
                    builder.Add(relatedDocument);
                }
            }

            return builder.DrainToImmutable();
        }
    }

    public ValueTask<ImmutableArray<ImportItem>> GetImportItemsAsync(string filePath, CancellationToken cancellationToken)
    {
        return _state.Documents.TryGetValue(filePath, out var state)
            ? new(GetImportItemsAsync(state.HostDocument, cancellationToken))
            : new([]);
    }

    public async Task<ImmutableArray<ImportItem>> GetImportItemsAsync(HostDocument hostDocument, CancellationToken cancellationToken)
    {
        var projectEngine = GetProjectEngine();

        var projectItem = projectEngine.FileSystem.GetItem(hostDocument.FilePath, hostDocument.FileKind);

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
            else if (_state.Documents.TryGetValue(importProjectItem.PhysicalPath, out var importDocumentState))
            {
                var textAndVersion = await importDocumentState.GetTextAndVersionAsync(cancellationToken).ConfigureAwait(false);
                var importItem = new ImportItem(importDocumentState.HostDocument.FilePath, importDocumentState.HostDocument.FileKind, textAndVersion.Text, textAndVersion.Version);

                importItems.Add(importItem);
            }
        }

        return importItems.DrainToImmutable();
    }

    #region ILegacyProjectSnapshot support

    RazorConfiguration ILegacyProjectSnapshot.Configuration => Configuration;
    string ILegacyProjectSnapshot.FilePath => FilePath;
    string? ILegacyProjectSnapshot.RootNamespace => RootNamespace;
    LanguageVersion ILegacyProjectSnapshot.CSharpLanguageVersion => CSharpLanguageVersion;
    ImmutableArray<TagHelperDescriptor> ILegacyProjectSnapshot.TagHelpers => ProjectWorkspaceState.TagHelpers;

    RazorProjectEngine ILegacyProjectSnapshot.GetProjectEngine()
        => _state.ProjectEngine;

    ILegacyDocumentSnapshot? ILegacyProjectSnapshot.GetDocument(string filePath)
        => TryGetDocument(filePath, out var document)
            ? document
            : null;

    #endregion
}
