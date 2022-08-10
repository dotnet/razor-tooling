﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Components;
using Microsoft.AspNetCore.Razor.Language.Legacy;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.Common.Extensions;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Refactoring
{
    internal class RenameEndpoint : AbstractRazorDelegatingEndpoint<RenameParamsBridge, WorkspaceEdit, DelegatedRenameParams>, IRenameEndpoint
    {
        private readonly ProjectSnapshotManagerDispatcher _projectSnapshotManagerDispatcher;
        private readonly DocumentContextFactory _documentContextFactory;
        private readonly ProjectSnapshotManager _projectSnapshotManager;
        private readonly RazorComponentSearchEngine _componentSearchEngine;
        private readonly LanguageServerFeatureOptions _languageServerFeatureOptions;
        private readonly RazorDocumentMappingService _documentMappingService;
        private readonly ClientNotifierServiceBase _languageServer;

        public RenameEndpoint(
            ProjectSnapshotManagerDispatcher projectSnapshotManagerDispatcher,
            DocumentContextFactory documentContextFactory,
            RazorComponentSearchEngine componentSearchEngine,
            ProjectSnapshotManagerAccessor projectSnapshotManagerAccessor,
            LanguageServerFeatureOptions languageServerFeatureOptions,
            RazorDocumentMappingService documentMappingService,
            ClientNotifierServiceBase languageServer,
            ILoggerFactory loggerFactory)
            : base(documentContextFactory, languageServerFeatureOptions, documentMappingService, languageServer, loggerFactory.CreateLogger<RenameEndpoint>())
        {
            _projectSnapshotManagerDispatcher = projectSnapshotManagerDispatcher ?? throw new ArgumentNullException(nameof(projectSnapshotManagerDispatcher));
            _documentContextFactory = documentContextFactory ?? throw new ArgumentNullException(nameof(documentContextFactory));
            _componentSearchEngine = componentSearchEngine ?? throw new ArgumentNullException(nameof(componentSearchEngine));
            _projectSnapshotManager = projectSnapshotManagerAccessor?.Instance ?? throw new ArgumentNullException(nameof(projectSnapshotManagerAccessor));
            _languageServerFeatureOptions = languageServerFeatureOptions ?? throw new ArgumentNullException(nameof(languageServerFeatureOptions));
            _documentMappingService = documentMappingService ?? throw new ArgumentNullException(nameof(documentMappingService));
            _languageServer = languageServer ?? throw new ArgumentNullException(nameof(languageServer));
        }

        public RegistrationExtensionResult? GetRegistration(VSInternalClientCapabilities clientCapabilities)
        {
            const string ServerCapability = "renameProvider";
            var options = new RenameOptions
            {
                PrepareProvider = false,
            };

            return new RegistrationExtensionResult(ServerCapability, options);
        }

        /// <inheritdoc/>
        protected override string CustomMessageTarget => RazorLanguageServerCustomMessageTargets.RazorRenameEndpointName;

        /// <inheritdoc/>
        protected override async Task<WorkspaceEdit?> TryHandleAsync(RenameParamsBridge request, DocumentContext documentContext, CancellationToken cancellationToken)
        {
            var sourceText = await documentContext.GetSourceTextAsync(cancellationToken).ConfigureAwait(false);
            var absoluteIndex = request.Position.GetRequiredAbsoluteIndex(sourceText, _logger);
            var codeDocument = await documentContext.GetCodeDocumentAsync(cancellationToken);

            var projection = await _documentMappingService.GetProjectionAsync(documentContext, absoluteIndex, cancellationToken);

            // If we're in C# then there is no point checking for a component tag, because there won't be one
            if (projection.LanguageKind == RazorLanguageKind.CSharp)
            {
                return null;
            }

            return await TryGetRazorComponentRenameEditsAsync(request, absoluteIndex, documentContext.Snapshot, codeDocument, cancellationToken).ConfigureAwait(false);
        }

        protected override bool IsSupported()
            => _languageServerFeatureOptions.SupportsFileManipulation;

        /// <inheritdoc/>
        protected override async Task<DelegatedRenameParams> CreateDelegatedParamsAsync(RenameParamsBridge request, DocumentContext documentContext, CancellationToken cancellationToken)
        {
            var sourceText = await documentContext.GetSourceTextAsync(cancellationToken).ConfigureAwait(false);
            var absoluteIndex = request.Position.GetRequiredAbsoluteIndex(sourceText, _logger);
            var projection = await _documentMappingService.GetProjectionAsync(documentContext, absoluteIndex, cancellationToken).ConfigureAwait(false);

            return new DelegatedRenameParams(
                        documentContext.Identifier,
                        projection.Position,
                        projection.LanguageKind,
                        request.NewName);
        }

        /// <inheritdoc/>
        protected override Task<WorkspaceEdit> HandleDelegatedResponseAsync(WorkspaceEdit response, DocumentContext documentContext, CancellationToken cancellationToken)
            => _documentMappingService.RemapWorkspaceEditAsync(response, cancellationToken);

        private async Task<WorkspaceEdit?> TryGetRazorComponentRenameEditsAsync(RenameParamsBridge request, int absoluteIndex, DocumentSnapshot requestDocumentSnapshot, RazorCodeDocument codeDocument, CancellationToken cancellationToken)
        {
            var originTagHelpers = GetOriginTagHelpers(requestDocumentSnapshot, codeDocument, absoluteIndex);
            if (originTagHelpers is null || originTagHelpers.Count == 0)
            {
                return null;
            }

            var originComponentDocumentSnapshot = await _componentSearchEngine.TryLocateComponentAsync(originTagHelpers.First()).ConfigureAwait(false);
            if (originComponentDocumentSnapshot is null)
            {
                return null;
            }

            var newPath = MakeNewPath(originComponentDocumentSnapshot.FilePath, request.NewName);
            if (File.Exists(newPath))
            {
                return null;
            }

            var documentChanges = new List<SumType<TextDocumentEdit, CreateFile, RenameFile, DeleteFile>>();
            AddFileRenameForComponent(documentChanges, originComponentDocumentSnapshot, newPath);
            AddEditsForCodeDocument(documentChanges, originTagHelpers, request.NewName, request.TextDocument.Uri, codeDocument);

            var documentSnapshots = await GetAllDocumentSnapshotsAsync(requestDocumentSnapshot, cancellationToken).ConfigureAwait(false);
            foreach (var documentSnapshot in documentSnapshots)
            {
                await AddEditsForCodeDocumentAsync(documentChanges, originTagHelpers, request.NewName, documentSnapshot);
            }

            return new WorkspaceEdit
            {
                DocumentChanges = documentChanges.ToArray(),
            };
        }

        private async Task<List<DocumentSnapshot?>> GetAllDocumentSnapshotsAsync(DocumentSnapshot skipDocumentSnapshot, CancellationToken cancellationToken)
        {
            var documentSnapshots = new List<DocumentSnapshot?>();
            var documentPaths = new HashSet<string>();
            await _projectSnapshotManagerDispatcher.RunOnDispatcherThreadAsync(GetAllDocumentSnapshotsInternalAsync, cancellationToken).ConfigureAwait(false);

            return documentSnapshots;

            async Task GetAllDocumentSnapshotsInternalAsync()
            {
                foreach (var project in _projectSnapshotManager.Projects)
                {
                    foreach (var documentPath in project.DocumentFilePaths)
                    {
                        // We've already added refactoring edits for our document snapshot
                        if (string.Equals(documentPath, skipDocumentSnapshot.FilePath, FilePathComparison.Instance))
                        {
                            continue;
                        }

                        // Don't add duplicates between projects
                        if (documentPaths.Contains(documentPath))
                        {
                            continue;
                        }

                        // Add to the list and add the path to the set
                        var documentContext = await _documentContextFactory.TryCreateAsync(new Uri(documentPath), cancellationToken);
                        if (documentContext is null)
                        {
                            throw new NotImplementedException($"{documentPath} in project {project.FilePath} but not retrievable");
                        }

                        documentSnapshots.Add(documentContext.Snapshot);
                        documentPaths.Add(documentPath);
                    }
                }
            }
        }

        public void AddFileRenameForComponent(List<SumType<TextDocumentEdit, CreateFile, RenameFile, DeleteFile>> documentChanges, DocumentSnapshot documentSnapshot, string newPath)
        {
            var oldUri = new UriBuilder
            {
                Path = documentSnapshot.FilePath,
                Host = string.Empty,
                Scheme = Uri.UriSchemeFile,
            }.Uri;
            var newUri = new UriBuilder
            {
                Path = newPath,
                Host = string.Empty,
                Scheme = Uri.UriSchemeFile,
            }.Uri;

            documentChanges.Add(new RenameFile
            {
                OldUri = oldUri,
                NewUri = newUri,
            });
        }

        private static string MakeNewPath(string originalPath, string newName)
        {
            var newFileName = $"{newName}{Path.GetExtension(originalPath)}";
            var newPath = Path.Combine(Path.GetDirectoryName(originalPath), newFileName);
            return newPath;
        }

        private async Task AddEditsForCodeDocumentAsync(
            List<SumType<TextDocumentEdit, CreateFile, RenameFile, DeleteFile>> documentChanges,
            IReadOnlyList<TagHelperDescriptor> originTagHelpers,
            string newName,
            DocumentSnapshot? documentSnapshot)
        {
            if (documentSnapshot is null)
            {
                return;
            }

            var codeDocument = await documentSnapshot.GetGeneratedOutputAsync().ConfigureAwait(false);
            if (codeDocument.IsUnsupported())
            {
                return;
            }

            if (!FileKinds.IsComponent(codeDocument.GetFileKind()))
            {
                return;
            }

            var uri = new UriBuilder
            {
                Path = documentSnapshot.FilePath,
                Host = string.Empty,
                Scheme = Uri.UriSchemeFile,
            }.Uri;

            AddEditsForCodeDocument(documentChanges, originTagHelpers, newName, uri, codeDocument);
        }

        public void AddEditsForCodeDocument(
            List<SumType<TextDocumentEdit, CreateFile, RenameFile, DeleteFile>> documentChanges,
            IReadOnlyList<TagHelperDescriptor> originTagHelpers,
            string newName,
            Uri uri,
            RazorCodeDocument codeDocument)
        {
            var documentIdentifier = new OptionalVersionedTextDocumentIdentifier { Uri = uri };
            var tagHelperElements = codeDocument.GetSyntaxTree().Root
                .DescendantNodes()
                .Where(n => n.Kind == SyntaxKind.MarkupTagHelperElement)
                .OfType<MarkupTagHelperElementSyntax>();

            for (var i = 0; i < originTagHelpers.Count; i++)
            {
                var editedName = newName;
                var originTagHelper = originTagHelpers[i];
                if (originTagHelper.IsComponentFullyQualifiedNameMatch() == true)
                {
                    // Fully qualified binding, our "new name" needs to be fully qualified.
                    var @namespace = originTagHelper.GetTypeNamespace();
                    if (@namespace == null)
                    {
                        return;
                    }

                    // The origin TagHelper was fully qualified so any fully qualified rename locations we find will need a fully qualified renamed edit.
                    editedName = @namespace + "." + newName;
                }

                foreach (var node in tagHelperElements)
                {
                    if (node is MarkupTagHelperElementSyntax tagHelperElement && BindingContainsTagHelper(originTagHelper, tagHelperElement.TagHelperInfo.BindingResult))
                    {
                        documentChanges.Add(new TextDocumentEdit
                        {
                            TextDocument = documentIdentifier,
                            Edits = CreateEditsForMarkupTagHelperElement(tagHelperElement, codeDocument, editedName).ToArray(),
                        });
                    }
                }
            }
        }

        public IReadOnlyCollection<TextEdit> CreateEditsForMarkupTagHelperElement(MarkupTagHelperElementSyntax element, RazorCodeDocument codeDocument, string newName)
        {
            var edits = new List<TextEdit>
            {
                new TextEdit()
                {
                    Range = element.StartTag.Name.GetRange(codeDocument.Source),
                    NewText = newName,
                },
            };
            if (element.EndTag != null)
            {
                edits.Add(new TextEdit()
                {
                    Range = element.EndTag.Name.GetRange(codeDocument.Source),
                    NewText = newName,
                });
            }

            return edits;
        }

        private static bool BindingContainsTagHelper(TagHelperDescriptor tagHelper, TagHelperBinding potentialBinding) =>
            potentialBinding.Descriptors.Any(descriptor => descriptor.Equals(tagHelper));

        private static IReadOnlyList<TagHelperDescriptor>? GetOriginTagHelpers(DocumentSnapshot documentSnapshot, RazorCodeDocument codeDocument, int absoluteIndex)
        {
            var change = new SourceChange(absoluteIndex, length: 0, newText: string.Empty);
            var syntaxTree = codeDocument.GetSyntaxTree();
            if (syntaxTree?.Root is null)
            {
                return null;
            }

            var owner = syntaxTree.Root.LocateOwner(change);
            if (owner is null)
            {
                Debug.Fail("Owner should never be null.");
                return null;
            }

            var node = owner.Ancestors().FirstOrDefault(n => n.Kind == SyntaxKind.MarkupTagHelperStartTag);
            if (node is not MarkupTagHelperStartTagSyntax tagHelperStartTag)
            {
                return null;
            }

            // Ensure the rename action was invoked on the component name
            // instead of a component parameter. This serves as an issue
            // mitigation till `textDocument/prepareRename` is supported
            // and we can ensure renames aren't triggered in unsupported
            // contexts. (https://github.com/dotnet/aspnetcore/issues/26407)
            if (!tagHelperStartTag.Name.FullSpan.IntersectsWith(absoluteIndex))
            {
                return null;
            }

            if (!(tagHelperStartTag?.Parent is MarkupTagHelperElementSyntax tagHelperElement))
            {
                return null;
            }

            // Can only have 1 component TagHelper belonging to an element at a time
            var primaryTagHelper = tagHelperElement.TagHelperInfo.BindingResult.Descriptors.FirstOrDefault(descriptor => descriptor.IsComponentTagHelper());
            if (primaryTagHelper is null)
            {
                return null;
            }

            var originTagHelpers = new List<TagHelperDescriptor>() { primaryTagHelper };
            var associatedTagHelper = FindAssociatedTagHelper(primaryTagHelper, documentSnapshot.Project.TagHelpers);
            if (associatedTagHelper is null)
            {
                Debug.Fail("Components should always have an associated TagHelper.");
                return null;
            }

            originTagHelpers.Add(associatedTagHelper);

            return originTagHelpers;
        }

        private static TagHelperDescriptor? FindAssociatedTagHelper(TagHelperDescriptor tagHelper, IReadOnlyList<TagHelperDescriptor> tagHelpers)
        {
            var typeName = tagHelper.GetTypeName();
            var assemblyName = tagHelper.AssemblyName;
            for (var i = 0; i < tagHelpers.Count; i++)
            {
                var currentTagHelper = tagHelpers[i];

                if (tagHelper == currentTagHelper)
                {
                    // Same as the primary, we're looking for our other pair.
                    continue;
                }

                var currentTypeName = currentTagHelper.GetTypeName();
                if (!string.Equals(typeName, currentTypeName, StringComparison.Ordinal))
                {
                    continue;
                }

                if (!string.Equals(assemblyName, currentTagHelper.AssemblyName, StringComparison.Ordinal))
                {
                    continue;
                }

                // Found our associated TagHelper, there should only ever be 1 other associated TagHelper (fully qualified and non-fully qualified).
                return currentTagHelper;
            }

            return null;
        }
    }
}
