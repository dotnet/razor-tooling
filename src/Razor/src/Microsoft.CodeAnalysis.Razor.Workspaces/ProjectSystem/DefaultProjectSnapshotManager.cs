﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Buffers;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces.ProjectSystem;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

// The implementation of project snapshot manager abstracts the host's underlying project system (HostProject),
// to provide a immutable view of the underlying project systems.
//
// The HostProject support all of the configuration that the Razor SDK exposes via the project system
// (language version, extensions, named configuration).
//
// The implementation will create a ProjectSnapshot for each HostProject.
internal class DefaultProjectSnapshotManager : ProjectSnapshotManagerBase
{
    public override event EventHandler<ProjectChangeEventArgs>? Changed;

    private readonly ProjectSnapshotChangeTrigger[] _triggers;

    // Each entry holds a ProjectState and an optional ProjectSnapshot. ProjectSnapshots are
    // created lazily.
    private readonly ReadWriterLocker _locksFactory = new();
    private readonly Dictionary<string, Entry> _projects_needsLock;
    private readonly HashSet<string> _openDocuments_needsLock;
    private static readonly LoadTextOptions LoadTextOptions = new LoadTextOptions(SourceHashAlgorithm.Sha256);

    // We have a queue for changes because if one change results in another change aka, add -> open we want to make sure the "add" finishes running first before "open" is notified.
    private readonly Queue<ProjectChangeEventArgs> _notificationWork;

    public DefaultProjectSnapshotManager(
        IErrorReporter errorReporter,
        IEnumerable<ProjectSnapshotChangeTrigger> triggers,
        Workspace workspace)
    {
        _triggers = triggers?.OrderByDescending(trigger => trigger.InitializePriority).ToArray() ?? throw new ArgumentNullException(nameof(triggers));
        Workspace = workspace ?? throw new ArgumentNullException(nameof(workspace));
        ErrorReporter = errorReporter ?? throw new ArgumentNullException(nameof(errorReporter));

        _projects_needsLock = new Dictionary<string, Entry>(FilePathComparer.Instance);
        _openDocuments_needsLock = new HashSet<string>(FilePathComparer.Instance);
        _notificationWork = new Queue<ProjectChangeEventArgs>();

        using (var _ = _locksFactory.EnterReadLock())
        {
            for (var i = 0; i < _triggers.Length; i++)
            {
                _triggers[i].Initialize(this);
            }
        }
    }

    // internal for testing
    internal bool IsSolutionClosing { get; private set; }

    public override ImmutableArray<IProjectSnapshot> GetProjects()
    {
        using var _ = _locksFactory.EnterReadLock();
        using var _1 = ListPool<IProjectSnapshot>.GetPooledObject(out var builder);

        foreach (var (_, entry) in _projects_needsLock)
        {
            builder.Add(entry.GetSnapshot());
        }

        return builder.ToImmutableArray();
    }

    internal override ImmutableArray<string> GetOpenDocuments()
    {
        using var _ = _locksFactory.EnterReadLock();
        return _openDocuments_needsLock.ToImmutableArray();
    }

    internal override Workspace Workspace { get; }

    internal override IErrorReporter ErrorReporter { get; }

    public override IProjectSnapshot? GetLoadedProject(string filePath)
    {
        if (filePath is null)
        {
            throw new ArgumentNullException(nameof(filePath));
        }

        using var _ = _locksFactory.EnterReadLock();
        if (_projects_needsLock.TryGetValue(filePath, out var entry))
        {
            return entry.GetSnapshot();
        }

        return null;
    }

    public override IProjectSnapshot GetOrCreateProject(string filePath)
    {
        if (filePath is null)
        {
            throw new ArgumentNullException(nameof(filePath));
        }

        using var _ = _locksFactory.EnterReadLock();
        return GetLoadedProject(filePath) ?? new EphemeralProjectSnapshot(Workspace.Services, filePath);
    }

    public override bool IsDocumentOpen(string documentFilePath)
    {
        if (documentFilePath is null)
        {
            throw new ArgumentNullException(nameof(documentFilePath));
        }

        using var _ = _locksFactory.EnterReadLock();
        return _openDocuments_needsLock.Contains(documentFilePath);
    }

    internal override void DocumentAdded(HostProject hostProject, HostDocument document, TextLoader textLoader)
    {
        if (hostProject is null)
        {
            throw new ArgumentNullException(nameof(hostProject));
        }

        if (document is null)
        {
            throw new ArgumentNullException(nameof(document));
        }

        if (TryChangeEntry_UsesLock(
            hostProject.FilePath,
            document.FilePath,
            new AddDocumentAction(document, textLoader),
            out var oldSnapshot,
            out var newSnapshot))
        {
            NotifyListeners(oldSnapshot, newSnapshot, document.FilePath, ProjectChangeKind.DocumentAdded);
        }
    }

    internal override void DocumentRemoved(HostProject hostProject, HostDocument document)
    {
        if (hostProject is null)
        {
            throw new ArgumentNullException(nameof(hostProject));
        }

        if (document is null)
        {
            throw new ArgumentNullException(nameof(document));
        }

        if (TryChangeEntry_UsesLock(
            hostProject.FilePath,
            document.FilePath,
            new RemoveDocumentAction(document),
            out var oldSnapshot,
            out var newSnapshot))
        {
            NotifyListeners(oldSnapshot, newSnapshot, document.FilePath, ProjectChangeKind.DocumentRemoved);
        }
    }

    internal override void DocumentOpened(string projectFilePath, string documentFilePath, SourceText sourceText)
    {
        if (projectFilePath is null)
        {
            throw new ArgumentNullException(nameof(projectFilePath));
        }

        if (documentFilePath is null)
        {
            throw new ArgumentNullException(nameof(documentFilePath));
        }

        if (sourceText is null)
        {
            throw new ArgumentNullException(nameof(sourceText));
        }

        if (TryChangeEntry_UsesLock(
            projectFilePath,
            documentFilePath,
            new OpenDocumentAction(sourceText),
            out var oldSnapshot,
            out var newSnapshot))
        {
            NotifyListeners(oldSnapshot, newSnapshot, documentFilePath, ProjectChangeKind.DocumentChanged);
        }
    }

    internal override void DocumentClosed(string projectFilePath, string documentFilePath, TextLoader textLoader)
    {
        if (projectFilePath is null)
        {
            throw new ArgumentNullException(nameof(projectFilePath));
        }

        if (documentFilePath is null)
        {
            throw new ArgumentNullException(nameof(documentFilePath));
        }

        if (textLoader is null)
        {
            throw new ArgumentNullException(nameof(textLoader));
        }

        if (TryChangeEntry_UsesLock(
            projectFilePath,
            documentFilePath,
            new CloseDocumentAction(textLoader),
            out var oldSnapshot,
            out var newSnapshot))
        {
            NotifyListeners(oldSnapshot, newSnapshot, documentFilePath, ProjectChangeKind.DocumentChanged);
        }
    }

    internal override void DocumentChanged(string projectFilePath, string documentFilePath, SourceText sourceText)
    {
        if (projectFilePath is null)
        {
            throw new ArgumentNullException(nameof(projectFilePath));
        }

        if (documentFilePath is null)
        {
            throw new ArgumentNullException(nameof(documentFilePath));
        }

        if (sourceText is null)
        {
            throw new ArgumentNullException(nameof(sourceText));
        }

        if (TryChangeEntry_UsesLock(
            projectFilePath,
            documentFilePath,
            new DocumentTextChangedAction(sourceText),
            out var oldSnapshot,
            out var newSnapshot))
        {
            NotifyListeners(oldSnapshot, newSnapshot, documentFilePath, ProjectChangeKind.DocumentChanged);
        }
    }

    internal override void DocumentChanged(string projectFilePath, string documentFilePath, TextLoader textLoader)
    {
        if (projectFilePath is null)
        {
            throw new ArgumentNullException(nameof(projectFilePath));
        }

        if (documentFilePath is null)
        {
            throw new ArgumentNullException(nameof(documentFilePath));
        }

        if (textLoader is null)
        {
            throw new ArgumentNullException(nameof(textLoader));
        }

        if (TryChangeEntry_UsesLock(
            projectFilePath,
            documentFilePath,
            new DocumentTextLoaderChangedAction(textLoader),
            out var oldSnapshot,
            out var newSnapshot))
        {
            NotifyListeners(oldSnapshot, newSnapshot, documentFilePath, ProjectChangeKind.DocumentChanged);
        }
    }

    internal override void ProjectAdded(HostProject hostProject)
    {
        if (hostProject is null)
        {
            throw new ArgumentNullException(nameof(hostProject));
        }

        if (TryChangeEntry_UsesLock(
            hostProject.FilePath,
            documentFilePath: null,
            new ProjectAddedAction(hostProject),
            out var oldSnapshot,
            out var newSnapshot))
        {
            NotifyListeners(older: null, newSnapshot, documentFilePath: null, ProjectChangeKind.ProjectAdded);
        }
    }

    internal override IProjectSnapshot GetOrAddLoadedProject(string normalizedPath, RazorConfiguration configuration, string? rootNamespace)
    {
        IProjectSnapshot? newProjectSnapshot = null;
        using (var upgradeableReadLock = _locksFactory.EnterUpgradeAbleReadLock())
        {
            var project = GetLoadedProject(normalizedPath);

            if (project is not null)
            {
                return project;
            }

            var newProject = new HostProject(normalizedPath, configuration, rootNamespace);
            var state = ProjectState.Create(Workspace.Services, newProject);
            var entry = new Entry(state);

            using (var _ = upgradeableReadLock.GetWriteLock())
            {
                _projects_needsLock[newProject.FilePath] = entry;
            }

            newProjectSnapshot = _projects_needsLock[normalizedPath].GetSnapshot();
        }

        // New project was created, notify outside of the lock
        NotifyListeners(older: null, newProjectSnapshot, documentFilePath: null, ProjectChangeKind.ProjectAdded);
        return newProjectSnapshot;
    }

    internal override void ProjectConfigurationChanged(HostProject hostProject)
    {
        if (hostProject is null)
        {
            throw new ArgumentNullException(nameof(hostProject));
        }

        if (TryChangeEntry_UsesLock(
            hostProject.FilePath,
            documentFilePath: null,
            new HostProjectUpdatedAction(hostProject),
            out var oldSnapshot,
            out var newSnapshot))
        {
            NotifyListeners(oldSnapshot, newSnapshot, documentFilePath: null, ProjectChangeKind.ProjectChanged);
        }
    }

    internal override void ProjectWorkspaceStateChanged(string projectFilePath, ProjectWorkspaceState? projectWorkspaceState)
    {
        if (projectFilePath is null)
        {
            throw new ArgumentNullException(nameof(projectFilePath));
        }

        if (projectWorkspaceState is null)
        {
            throw new ArgumentNullException(nameof(projectWorkspaceState));
        }

        if (TryChangeEntry_UsesLock(
            projectFilePath,
            documentFilePath: null,
            new ProjectWorkspaceStateChanged(projectWorkspaceState),
            out var oldSnapshot,
            out var newSnapshot))
        {
            NotifyListeners(oldSnapshot, newSnapshot, documentFilePath: null, ProjectChangeKind.ProjectChanged);
        }
    }

    internal override void ProjectRemoved(HostProject hostProject)
    {
        if (hostProject is null)
        {
            throw new ArgumentNullException(nameof(hostProject));
        }

        if (TryChangeEntry_UsesLock(
            hostProject.FilePath,
            documentFilePath: null,
            new ProjectRemovedAction(hostProject.FilePath),
            out var oldSnapshot,
            out var newSnapshot))
        {
            NotifyListeners(oldSnapshot, newSnapshot, documentFilePath: null, ProjectChangeKind.ProjectRemoved);
        }
    }

    internal override bool TryRemoveLoadedProject(string normalizedPath, [NotNullWhen(true)] out IProjectSnapshot? project)
    {
        if (TryChangeEntry_UsesLock(
                normalizedPath,
                documentFilePath: null,
                new ProjectRemovedAction(normalizedPath),
                out var oldSnapshot,
                out project))
        {
            NotifyListeners(oldSnapshot, project, documentFilePath: null, ProjectChangeKind.ProjectRemoved);
            return true;
        }

        return false;
    }

    internal override void SolutionOpened()
    {
        IsSolutionClosing = false;
    }

    internal override void SolutionClosed()
    {
        IsSolutionClosing = true;
    }

    internal override void ReportError(Exception exception)
    {
        if (exception is null)
        {
            throw new ArgumentNullException(nameof(exception));
        }

        ErrorReporter.ReportError(exception);
    }

    internal override void ReportError(Exception exception, IProjectSnapshot project)
    {
        if (exception is null)
        {
            throw new ArgumentNullException(nameof(exception));
        }

        ErrorReporter.ReportError(exception, project);
    }

    internal override void ReportError(Exception exception, HostProject hostProject)
    {
        if (exception is null)
        {
            throw new ArgumentNullException(nameof(exception));
        }

        var snapshot = hostProject?.FilePath is null ? null : GetLoadedProject(hostProject.FilePath);
        ErrorReporter.ReportError(exception, snapshot);
    }

    private void NotifyListeners(IProjectSnapshot? older, IProjectSnapshot? newer, string? documentFilePath, ProjectChangeKind kind)
    {
        NotifyListeners(new ProjectChangeEventArgs(older, newer, documentFilePath, kind, IsSolutionClosing));
    }

    // virtual so it can be overridden in tests
    protected virtual void NotifyListeners(ProjectChangeEventArgs e)
    {
        _notificationWork.Enqueue(e);

        if (_notificationWork.Count == 1)
        {
            // Only one notification, go ahead and start notifying. In the situation where Count > 1 it means an event was triggered as a response to another event.
            // To ensure order we wont immediately re-invoke Changed here, we'll wait for the stack to unwind to notify others. This process still happens synchronously
            // it just ensures that events happen in the correct order. For instance lets take the situation where a document is added to a project. That document will be
            // added and then opened. However, if the result of "adding" causes an "open" to triger we want to ensure that "add" finishes prior to "open" being notified.

            // Start unwinding the notification queue
            do
            {
                // Don't dequeue yet, we want the notification to sit in the queue until we've finished notifying to ensure other calls to NotifyListeners know there's a currently running event loop.
                var args = _notificationWork.Peek();
                Changed?.Invoke(this, args);

                _notificationWork.Dequeue();
            }
            while (_notificationWork.Count > 0);
        }
    }

    internal override void UpdateProject(
        string filePath,
        RazorConfiguration configuration,
        ProjectWorkspaceState projectWorkspaceState,
        string? rootNamespace,
        Func<IProjectSnapshot, ImmutableArray<IUpdateProjectAction>> calculate)
    {
        if (projectWorkspaceState is null)
        {
            throw new ArgumentNullException(nameof(projectWorkspaceState));
        }

        using var _ = ListPool<ProjectChangeEventArgs>.GetPooledObject(out var changesToNotify);

        // Get an upgradeableLock, which will keep a read lock while we compute the changes
        // and then get a write lock to actually apply them. Only one upgradeable lock
        // can be held at any given time. Write lock must be retrieved on the same
        // thread that the lock was acquired
        using (var upgradeableLock = _locksFactory.EnterUpgradeAbleReadLock())
        {
            UpdateProject_NoLock(filePath, configuration, projectWorkspaceState, rootNamespace, calculate, changesToNotify, upgradeableLock);
        }

        // Notify outside of the lock, since notifications may trigger mutations from listeners
        foreach (var notification in changesToNotify)
        {
            NotifyListeners(notification);
        }
    }

    private void UpdateProject_NoLock(
        string filePath,
        RazorConfiguration configuration,
        ProjectWorkspaceState projectWorkspaceState,
        string? rootNamespace,
        Func<IProjectSnapshot, ImmutableArray<IUpdateProjectAction>> calculate,
        List<ProjectChangeEventArgs> changesToNotify,
        in ReadWriterLocker.UpgradeableReadLock upgradeableLock)
    {
        var project = GetLoadedProject(filePath);
        if (project is not ProjectSnapshot projectSnapshot)
        {
            return;
        }

        var originalHostProject = projectSnapshot.HostProject;
        var changes = calculate(project);

        var originalEntry = _projects_needsLock[filePath];
        Dictionary<string, Entry> updatedProjectsMap = new(changes.Length, FilePathComparer.Instance);

        // Resolve all the changes and add notifications as needed
        foreach (var change in changes)
        {
            switch (change)
            {
                case AddDocumentAction addAction:
                    {
                        var entry = GetCurrentEntry(project);
                        TryAddNotificationAndUpdate(entry, entry.State.WithAddedHostDocument(addAction.NewDocument, CreateTextAndVersionFunc(addAction.TextLoader)), ProjectChangeKind.DocumentAdded, addAction.NewDocument.FilePath);
                    }

                    break;

                case RemoveDocumentAction removeAction:
                    {
                        var entry = GetCurrentEntry(project);
                        TryAddNotificationAndUpdate(entry, entry.State.WithRemovedHostDocument(removeAction.OriginalDocument), ProjectChangeKind.DocumentRemoved, removeAction.OriginalDocument.FilePath);
                    }

                    break;

                case UpdateDocumentAction updateAction:
                    {
                        var entry = GetCurrentEntry(project);
                        TryAddNotificationAndUpdate(entry, entry.State.WithRemovedHostDocument(updateAction.OriginalDocument), ProjectChangeKind.DocumentRemoved, updateAction.OriginalDocument.FilePath);

                        entry = GetCurrentEntry(project);
                        TryAddNotificationAndUpdate(entry, entry.State.WithAddedHostDocument(updateAction.NewDocument, CreateTextAndVersionFunc(updateAction.TextLoader)), ProjectChangeKind.DocumentAdded, updateAction.NewDocument.FilePath);
                    }

                    break;

                case MoveDocumentAction moveAction:
                    var (from, to) = (moveAction.OriginalProject, moveAction.DestinationProject);
                    Debug.Assert(from == project || to == project);
                    Debug.Assert(from != to);

                    var fromEntry = GetCurrentEntry(from);
                    var toEntry = GetCurrentEntry(to);

                    TryAddNotificationAndUpdate(fromEntry, fromEntry.State.WithRemovedHostDocument(moveAction.Document), ProjectChangeKind.DocumentRemoved, moveAction.Document.FilePath);
                    TryAddNotificationAndUpdate(toEntry, toEntry.State.WithAddedHostDocument(moveAction.Document, CreateTextAndVersionFunc(moveAction.TextLoader)), ProjectChangeKind.DocumentAdded, moveAction.Document.FilePath);
                    break;

                default:
                    throw new InvalidOperationException($"Unexpected action type {change.GetType()}");
            }
        }

        if (!projectWorkspaceState.Equals(ProjectWorkspaceState.Default))
        {
            var entryBeforeWorkspaceState = GetCurrentEntry(project);
            var stateWithProjectWorkspaceState = entryBeforeWorkspaceState.State.WithProjectWorkspaceState(projectWorkspaceState);
            TryAddNotificationAndUpdate(entryBeforeWorkspaceState, stateWithProjectWorkspaceState, ProjectChangeKind.ProjectChanged, documentFilePath: null);
        }

        if (originalHostProject.RootNamespace != rootNamespace || configuration != originalHostProject.Configuration)
        {
            var currentEntry = GetCurrentEntry(project);
            var currentHostProject = currentEntry.State.HostProject;
            var newHostProject = new HostProject(currentHostProject.FilePath, configuration, rootNamespace);
            var newEntry = new Entry(currentEntry.State.WithHostProject(newHostProject));
            updatedProjectsMap[project.FilePath] = newEntry;
            changesToNotify.Add(new ProjectChangeEventArgs(currentEntry.GetSnapshot(), newEntry.GetSnapshot(), ProjectChangeKind.ProjectChanged));
        }

        // Update current state first so we can get rid of the write lock and downgrade
        // back to a read lock when notifying changes
        using (upgradeableLock.GetWriteLock())
        {
            foreach (var (path, entry) in updatedProjectsMap)
            {
                _projects_needsLock[path] = entry;
            }
        }

        void TryAddNotificationAndUpdate(Entry currentEntry, ProjectState newState, ProjectChangeKind changeKind, string? documentFilePath)
        {
            if (newState.Equals(currentEntry.State))
            {
                return;
            }

            var newEntry = new Entry(newState);
            updatedProjectsMap[currentEntry.State.HostProject.FilePath] = newEntry;
            changesToNotify.Add(new ProjectChangeEventArgs(currentEntry.GetSnapshot(), newEntry.GetSnapshot(), documentFilePath, changeKind, IsSolutionClosing));
        }

        Entry GetCurrentEntry(IProjectSnapshot project)
        {
            if (!updatedProjectsMap.TryGetValue(project.FilePath, out var entry))
            {
                entry = _projects_needsLock[project.FilePath];
                updatedProjectsMap[project.FilePath] = entry;
            }

            return entry;
        }
    }

    private static Func<Task<TextAndVersion>> CreateTextAndVersionFunc(TextLoader textLoader)
        => textLoader is null
            ? DocumentState.EmptyLoader
            : (() => textLoader.LoadTextAndVersionAsync(LoadTextOptions, CancellationToken.None));

    private bool TryChangeEntry_UsesLock(
        string projectFilePath,
        string? documentFilePath,
        IUpdateProjectAction action,
        [NotNullWhen(true)] out IProjectSnapshot? oldSnapshot,
        [NotNullWhen(true)] out IProjectSnapshot? newSnapshot)
    {
        using var upgradeableLock = _locksFactory.EnterUpgradeAbleReadLock();

        if (action is ProjectAddedAction projectAddedAction)
        {
            if (_projects_needsLock.ContainsKey(projectAddedAction.HostProject.FilePath))
            {
                oldSnapshot = newSnapshot = null;
                return false;
            }

            var state = ProjectState.Create(Workspace.Services, projectAddedAction.HostProject);
            var newEntry = new Entry(state);

            oldSnapshot = newSnapshot = newEntry.GetSnapshot();
            using (var writeLock = upgradeableLock.GetWriteLock())
            {
                _projects_needsLock[projectAddedAction.HostProject.FilePath] = newEntry;
            }

            return true;
        }

        if (_projects_needsLock.TryGetValue(projectFilePath, out var entry))
        {
            // if the solution is closing we don't need to bother computing new state
            if (IsSolutionClosing)
            {
                oldSnapshot = newSnapshot = entry.GetSnapshot();
                return true;
            }
            else
            {
                DocumentState? documentState = null;
                if (documentFilePath is not null)
                {
                    _ = entry.State.Documents.TryGetValue(documentFilePath, out documentState);
                }

                var newEntry = Change(entry, action, documentState);
                if (!ReferenceEquals(newEntry?.State, entry.State))
                {
                    oldSnapshot = entry.GetSnapshot();
                    newSnapshot = newEntry?.GetSnapshot() ?? oldSnapshot;
                    using (var writeLock = upgradeableLock.GetWriteLock())
                    {
                        if (newEntry is null)
                        {
                            _projects_needsLock.Remove(projectFilePath);
                        }
                        else
                        {
                            _projects_needsLock[projectFilePath] = newEntry;
                        }

                        if (action is OpenDocumentAction)
                        {
                            if (documentFilePath is null)
                            {
                                throw new ArgumentNullException(nameof(documentFilePath));
                            }

                            _openDocuments_needsLock.Add(documentFilePath);
                        }
                        else if (action is CloseDocumentAction)
                        {
                            if (documentFilePath is null)
                            {
                                throw new ArgumentNullException(nameof(documentFilePath));
                            }

                            _openDocuments_needsLock.Remove(documentFilePath);
                        }
                    }

                    return true;
                }
            }
        }

        oldSnapshot = newSnapshot = null;
        return false;
    }

    private static Entry? Change(Entry originalEntry, IUpdateProjectAction action, DocumentState? documentState)
    {
        switch (action)
        {
            case AddDocumentAction addAction:
                return new Entry(originalEntry.State.WithAddedHostDocument(addAction.NewDocument, CreateTextAndVersionFunc(addAction.TextLoader)));

            case RemoveDocumentAction removeAction:
                return new Entry(originalEntry.State.WithRemovedHostDocument(removeAction.OriginalDocument));

            case CloseDocumentAction closeAction:
                {
                    if (documentState is null)
                    {
                        throw new ArgumentNullException(nameof(documentState));
                    }

                    var state = originalEntry.State.WithChangedHostDocument(
                        documentState.HostDocument,
                        async () => await closeAction.TextLoader.LoadTextAndVersionAsync(LoadTextOptions, cancellationToken: default).ConfigureAwait(false));
                    return new Entry(state);
                }

            case OpenDocumentAction openAction:
                if (documentState is null)
                {
                    throw new ArgumentNullException(nameof(documentState));
                }

                {
                    if (documentState.TryGetText(out var olderText) &&
                        documentState.TryGetTextVersion(out var olderVersion))
                    {
                        var version = openAction.SourceText.ContentEquals(olderText) ? olderVersion : olderVersion.GetNewerVersion();
                        var newState = originalEntry.State.WithChangedHostDocument(documentState.HostDocument, openAction.SourceText, version);
                        return new Entry(newState);
                    }
                    else
                    {
                        var newState = originalEntry.State.WithChangedHostDocument(documentState.HostDocument, async () =>
                        {
                            olderText = await documentState.GetTextAsync().ConfigureAwait(false);
                            olderVersion = await documentState.GetTextVersionAsync().ConfigureAwait(false);

                            var version = openAction.SourceText.ContentEquals(olderText) ? olderVersion : olderVersion.GetNewerVersion();
                            return TextAndVersion.Create(openAction.SourceText, version, documentState.HostDocument.FilePath);
                        });

                        return new Entry(newState);
                    }
                }

            case DocumentTextLoaderChangedAction textLoaderChangedAction:
                if (documentState is null)
                {
                    throw new ArgumentNullException(nameof(documentState));
                }

                return new Entry(originalEntry.State.WithChangedHostDocument(documentState.HostDocument, CreateTextAndVersionFunc(textLoaderChangedAction.TextLoader)));

            case DocumentTextChangedAction textChangedAction:
                if (documentState is null)
                {
                    throw new ArgumentNullException(nameof(documentState));
                }

                {
                    if (documentState.TryGetText(out var olderText) &&
                        documentState.TryGetTextVersion(out var olderVersion))
                    {
                        var version = textChangedAction.SourceText.ContentEquals(olderText) ? olderVersion : olderVersion.GetNewerVersion();
                        var state = originalEntry.State.WithChangedHostDocument(documentState.HostDocument, textChangedAction.SourceText, version);

                        return new Entry(state);
                    }
                    else
                    {
                        var state = originalEntry.State.WithChangedHostDocument(documentState.HostDocument, async () =>
                        {
                            olderText = await documentState.GetTextAsync().ConfigureAwait(false);
                            olderVersion = await documentState.GetTextVersionAsync().ConfigureAwait(false);

                            var version = textChangedAction.SourceText.ContentEquals(olderText) ? olderVersion : olderVersion.GetNewerVersion();
                            return TextAndVersion.Create(textChangedAction.SourceText, version, documentState.HostDocument.FilePath);
                        });

                        return new Entry(state);
                    }
                }

            case ProjectRemovedAction:
                return null;

            case ProjectWorkspaceStateChanged worskapceStateChangedAction:
                return new Entry(originalEntry.State.WithProjectWorkspaceState(worskapceStateChangedAction.WorkspaceState));

            case HostProjectUpdatedAction hostProjectUpdatedAction:
                return new Entry(originalEntry.State.WithHostProject(hostProjectUpdatedAction.HostProject));

            default:
                throw new InvalidOperationException($"Unexpected action type {action.GetType()}");
        }
    }

    private class Entry
    {
        private IProjectSnapshot? _snapshotUnsafe;
        public readonly ProjectState State;

        public Entry(ProjectState state)
        {
            State = state;
        }

        public IProjectSnapshot GetSnapshot()
        {
            return _snapshotUnsafe ??= new ProjectSnapshot(State);
        }
    }
}
