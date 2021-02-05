﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Host;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem
{
    internal class DocumentState
    {
        private static readonly TextAndVersion EmptyText = TextAndVersion.Create(
            SourceText.From(string.Empty),
            VersionStamp.Default);

        public static readonly Func<Task<TextAndVersion>> EmptyLoader = () => Task.FromResult(EmptyText);

        private readonly object _lock;

        private ComputedStateTracker _computedState;

        private Func<Task<TextAndVersion>> _loader;
        private Task<TextAndVersion> _loaderTask;
        private SourceText _sourceText;
        private VersionStamp? _version;

        public static DocumentState Create(
            HostWorkspaceServices services,
            HostDocument hostDocument,
            Func<Task<TextAndVersion>> loader)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (hostDocument == null)
            {
                throw new ArgumentNullException(nameof(hostDocument));
            }

            loader = loader ?? EmptyLoader;
            return new DocumentState(services, hostDocument, null, null, loader);
        }

        // Internal for testing
        internal DocumentState(
            HostWorkspaceServices services,
            HostDocument hostDocument,
            SourceText text,
            VersionStamp? version,
            Func<Task<TextAndVersion>> loader)
        {
            Services = services;
            HostDocument = hostDocument;
            _sourceText = text;
            _version = version;
            _loader = loader;
            _lock = new object();
        }

        public HostDocument HostDocument { get; }

        public HostWorkspaceServices Services { get; }

        public GeneratedDocumentContainer GeneratedDocumentContainer => HostDocument.GeneratedDocumentContainer;

        public bool IsGeneratedOutputResultAvailable => ComputedState.IsResultAvailable == true;

        private ComputedStateTracker ComputedState
        {
            get
            {
                if (_computedState == null)
                {
                    lock (_lock)
                    {
                        if (_computedState == null)
                        {
                            _computedState = new ComputedStateTracker(this);
                        }
                    }
                }

                return _computedState;
            }
        }

        public Task<(RazorCodeDocument output, VersionStamp inputVersion, VersionStamp outputCSharpVersion, VersionStamp outputHtmlVersion)> GetGeneratedOutputAndVersionAsync(DefaultProjectSnapshot project, DefaultDocumentSnapshot document)
        {
            return ComputedState.GetGeneratedOutputAndVersionAsync(project, document);
        }

        public IReadOnlyList<DocumentSnapshot> GetImports(DefaultProjectSnapshot project)
        {
            return GetImportsCore(project);
        }

        public async Task<SourceText> GetTextAsync()
        {
            if (TryGetText(out var text))
            {
                return text;
            }

            lock (_lock)
            {
                _loaderTask = _loader();
            }

            return (await _loaderTask.ConfigureAwait(false)).Text;
        }

        public async Task<VersionStamp> GetTextVersionAsync()
        {
            if (TryGetTextVersion(out var version))
            {
                return version;
            }

            lock (_lock)
            {
                _loaderTask = _loader();
            }

            return (await _loaderTask.ConfigureAwait(false)).Version;
        }

        public bool TryGetText(out SourceText result)
        {
            if (_sourceText != null)
            {
                result = _sourceText;
                return true;
            }

            if (_loaderTask != null && _loaderTask.IsCompleted)
            {
#pragma warning disable VSTHRD002 // Avoid problematic synchronous waits
                result = _loaderTask.Result.Text;
#pragma warning restore VSTHRD002 // Avoid problematic synchronous waits
                return true;
            }

            result = null;
            return false;
        }

        public bool TryGetTextVersion(out VersionStamp result)
        {
            if (_version != null)
            {
                result = _version.Value;
                return true;
            }

            if (_loaderTask != null && _loaderTask.IsCompleted)
            {
#pragma warning disable VSTHRD002 // Avoid problematic synchronous waits
                result = _loaderTask.Result.Version;
#pragma warning restore VSTHRD002 // Avoid problematic synchronous waits
                _version = result;
                return true;
            }

            result = default;
            return false;
        }

        public virtual DocumentState WithConfigurationChange()
        {
            var state = new DocumentState(Services, HostDocument, _sourceText, _version, _loader);

            // The source could not have possibly changed.
            state._sourceText = _sourceText;
            state._version = _version;
            state._loaderTask = _loaderTask;

            // Do not cache computed state

            return state;
        }

        public virtual DocumentState WithImportsChange()
        {
            var state = new DocumentState(Services, HostDocument, _sourceText, _version, _loader);

            // The source could not have possibly changed.
            state._sourceText = _sourceText;
            state._version = _version;
            state._loaderTask = _loaderTask;

            // Optimisically cache the computed state
            state._computedState = new ComputedStateTracker(state, _computedState);

            return state;
        }

        public virtual DocumentState WithProjectWorkspaceStateChange()
        {
            var state = new DocumentState(Services, HostDocument, _sourceText, _version, _loader);

            // The source could not have possibly changed.
            state._sourceText = _sourceText;
            state._version = _version;
            state._loaderTask = _loaderTask;

            // Optimisically cache the computed state
            state._computedState = new ComputedStateTracker(state, _computedState);

            return state;
        }

        public virtual DocumentState WithText(SourceText sourceText, VersionStamp version)
        {
            if (sourceText == null)
            {
                throw new ArgumentNullException(nameof(sourceText));
            }

            // Do not cache the computed state

            return new DocumentState(Services, HostDocument, sourceText, version, null);
        }

        public virtual DocumentState WithTextLoader(Func<Task<TextAndVersion>> loader)
        {
            if (loader == null)
            {
                throw new ArgumentNullException(nameof(loader));
            }

            // Do not cache the computed state

            return new DocumentState(Services, HostDocument, null, null, loader);
        }

        private IReadOnlyList<DocumentSnapshot> GetImportsCore(DefaultProjectSnapshot project)
        {
            var projectEngine = project.GetProjectEngine();
            var importFeatures = projectEngine.ProjectFeatures.OfType<IImportProjectFeature>();
            var projectItem = projectEngine.FileSystem.GetItem(HostDocument.FilePath, HostDocument.FileKind);
            var importItems = importFeatures.SelectMany(f => f.GetImports(projectItem));
            if (importItems == null)
            {
                return Array.Empty<DocumentSnapshot>();
            }

            var imports = new List<DocumentSnapshot>();
            foreach (var item in importItems)
            {
                if (item.PhysicalPath == null)
                {
                    // This is a default import.
                    var defaultImport = new DefaultImportDocumentSnapshot(project, item);
                    imports.Add(defaultImport);
                }
                else
                {
                    var import = project.GetDocument(item.PhysicalPath);
                    if (import == null)
                    {
                        // We are not tracking this document in this project. So do nothing.
                        continue;
                    }

                    imports.Add(import);
                }
            }

            return imports;
        }

        // See design notes on ProjectState.ComputedStateTracker.
        private class ComputedStateTracker
        {
            private readonly object _lock;

            private ComputedStateTracker _older;

            // We utilize a WeakReference here to avoid bloating committed memory. If pieces request document output inbetween GC collections
            // then we will provide the weak referenced task; otherwise we require any state requests to be re-computed.
            public WeakReference<Task<(RazorCodeDocument, VersionStamp, VersionStamp, VersionStamp)>> TaskUnsafeReference;

            public ComputedStateTracker(DocumentState state, ComputedStateTracker older = null)
            {
                _lock = state._lock;
                _older = older;
            }

            public bool IsResultAvailable
            {
                get
                {
                    if (TaskUnsafeReference == null)
                    {
                        return false;
                    }

                    if (TaskUnsafeReference.TryGetTarget(out var taskUnsafe))
                    {
                        return taskUnsafe.IsCompleted;
                    }

                    return false;
                }
            }

            public Task<(RazorCodeDocument, VersionStamp, VersionStamp, VersionStamp)> GetGeneratedOutputAndVersionAsync(DefaultProjectSnapshot project, DocumentSnapshot document)
            {
                if (project == null)
                {
                    throw new ArgumentNullException(nameof(project));
                }

                if (document == null)
                {
                    throw new ArgumentNullException(nameof(document));
                }

                if (TaskUnsafeReference == null ||
                    !TaskUnsafeReference.TryGetTarget(out var taskUnsafe))
                {
                    lock (_lock)
                    {
                        if (TaskUnsafeReference == null ||
                            !TaskUnsafeReference.TryGetTarget(out taskUnsafe))
                        {
                            taskUnsafe = GetGeneratedOutputAndVersionCoreAsync(project, document);
                            TaskUnsafeReference = new WeakReference<Task<(RazorCodeDocument, VersionStamp, VersionStamp, VersionStamp)>>(taskUnsafe);
                        }
                    }
                }

                return taskUnsafe;
            }

            private async Task<(RazorCodeDocument, VersionStamp, VersionStamp, VersionStamp)> GetGeneratedOutputAndVersionCoreAsync(DefaultProjectSnapshot project, DocumentSnapshot document)
            {
                // We only need to produce the generated code if any of our inputs is newer than the
                // previously cached output.
                //
                // First find the versions that are the inputs:
                // - The project + computed state
                // - The imports
                // - This document
                //
                // All of these things are cached, so no work is wasted if we do need to generate the code.
                var configurationVersion = project.State.ConfigurationVersion;
                var projectWorkspaceStateVersion = project.State.ProjectWorkspaceStateVersion;
                var documentCollectionVersion = project.State.DocumentCollectionVersion;
                var imports = await GetImportsAsync(project, document).ConfigureAwait(false);
                var documentVersion = await document.GetTextVersionAsync().ConfigureAwait(false);

                // OK now that have the previous output and all of the versions, we can see if anything
                // has changed that would require regenerating the code.
                var inputVersion = documentVersion;
                if (inputVersion.GetNewerVersion(configurationVersion) == configurationVersion)
                {
                    inputVersion = configurationVersion;
                }

                if (inputVersion.GetNewerVersion(projectWorkspaceStateVersion) == projectWorkspaceStateVersion)
                {
                    inputVersion = projectWorkspaceStateVersion;
                }

                if (inputVersion.GetNewerVersion(documentCollectionVersion) == documentCollectionVersion)
                {
                    inputVersion = documentCollectionVersion;
                }

                for (var i = 0; i < imports.Count; i++)
                {
                    var importVersion = imports[i].Version;
                    if (inputVersion.GetNewerVersion(importVersion) == importVersion)
                    {
                        inputVersion = importVersion;
                    }
                }

                RazorCodeDocument olderOutput = null;
                var olderInputVersion = default(VersionStamp);
                var olderCSharpOutputVersion = default(VersionStamp);
                var olderHtmlOutputVersion = default(VersionStamp);
                if (_older?.TaskUnsafeReference != null &&
                    _older.TaskUnsafeReference.TryGetTarget(out var taskUnsafe))
                {
                    (olderOutput, olderInputVersion, olderCSharpOutputVersion, olderHtmlOutputVersion) = await taskUnsafe.ConfigureAwait(false);
                    if (inputVersion.GetNewerVersion(olderInputVersion) == olderInputVersion)
                    {
                        // Nothing has changed, we can use the cached result.
                        lock (_lock)
                        {
                            TaskUnsafeReference = _older.TaskUnsafeReference;
                            _older = null;
                            return (olderOutput, olderInputVersion, olderCSharpOutputVersion, olderHtmlOutputVersion);
                        }
                    }
                }

                // OK we have to generate the code.
                var importSources = new List<RazorSourceDocument>();
                var projectEngine = project.GetProjectEngine();
                foreach (var item in imports)
                {
                    var importProjectItem = item.FilePath == null ? null : projectEngine.FileSystem.GetItem(item.FilePath, item.FileKind);
                    var sourceDocument = await GetRazorSourceDocumentAsync(item.Document, importProjectItem).ConfigureAwait(false);
                    importSources.Add(sourceDocument);
                }

                var projectItem = document.FilePath == null ? null : projectEngine.FileSystem.GetItem(document.FilePath, document.FileKind);
                var documentSource = await GetRazorSourceDocumentAsync(document, projectItem).ConfigureAwait(false);


                var codeDocument = projectEngine.ProcessDesignTime(documentSource, fileKind: document.FileKind, importSources, project.TagHelpers);
                var csharpDocument = codeDocument.GetCSharpDocument();
                var htmlDocument = codeDocument.GetHtmlDocument();

                // OK now we've generated the code. Let's check if the output is actually different. This is
                // a valuable optimization for our use cases because lots of changes you could make require
                // us to run code generation, but don't change the result.
                //
                // Note that we're talking about the effect on the generated C#/HTML here (not the other artifacts).
                // This is the reason why we have three versions associated with the document. 
                //
                // The INPUT version is related the .cshtml files and tag helpers
                // The CSHARPOUTPUT version is related to the generated C#
                // The HTMLOUTPUT version is related to the generated HTML
                //
                // Examples: 
                // 
                // A change to a tag helper not used by this document - updates the INPUT version, but not 
                // the OUTPUT version.
                //
                //
                // Razor IDE features should always retrieve the output and party on it regardless. Depending
                // on the use cases we may or may not need to synchronize the output.

                var outputCSharpVersion = inputVersion;
                var outputHtmlVersion = inputVersion;
                if (olderOutput != null)
                {
                    if (string.Equals(
                        olderOutput.GetCSharpDocument().GeneratedCode,
                        csharpDocument.GeneratedCode,
                        StringComparison.Ordinal))
                    {
                        outputCSharpVersion = olderCSharpOutputVersion;
                    }

                    if (string.Equals(
                        olderOutput.GetHtmlDocument().GeneratedHtml,
                        htmlDocument.GeneratedHtml,
                        StringComparison.Ordinal))
                    {
                        outputHtmlVersion = olderHtmlOutputVersion;
                    }
                }

                if (document is DefaultDocumentSnapshot defaultDocument)
                {
                    defaultDocument.State.HostDocument.GeneratedDocumentContainer.SetOutput(
                        defaultDocument,
                        csharpDocument,
                        htmlDocument,
                        inputVersion,
                        outputCSharpVersion,
                        outputHtmlVersion);
                }

                return (codeDocument, inputVersion, outputCSharpVersion, outputHtmlVersion);
            }

            private async Task<RazorSourceDocument> GetRazorSourceDocumentAsync(DocumentSnapshot document, RazorProjectItem projectItem)
            {
                var sourceText = await document.GetTextAsync();
                return sourceText.GetRazorSourceDocument(document.FilePath, projectItem?.RelativePhysicalPath);
            }

            private async Task<IReadOnlyList<ImportItem>> GetImportsAsync(ProjectSnapshot project, DocumentSnapshot document)
            {
                var imports = new List<ImportItem>();
                foreach (var snapshot in document.GetImports())
                {
                    var versionStamp = await snapshot.GetTextVersionAsync();
                    imports.Add(new ImportItem(snapshot.FilePath, versionStamp, snapshot));
                }

                return imports;
            }

            private readonly struct ImportItem
            {
                public ImportItem(string filePath, VersionStamp version, DocumentSnapshot document)
                {
                    FilePath = filePath;
                    Version = version;
                    Document = document;
                }

                public string FilePath { get; }

                public string FileKind => Document.FileKind;

                public VersionStamp Version { get; }

                public DocumentSnapshot Document { get; }
            }
        }
    }
}
