﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.OmniSharpPlugin.StrongNamed
{
    public class OmniSharpWorkspaceProjectStateChangeDetector : IOmniSharpProjectSnapshotManagerChangeTrigger
    {
        public OmniSharpWorkspaceProjectStateChangeDetector(
            OmniSharpProjectSnapshotManagerDispatcher projectSnapshotManagerDispatcher!!,
            OmniSharpProjectWorkspaceStateGenerator workspaceStateGenerator!!)
        {
            InternalWorkspaceProjectStateChangeDetector = new ProjectSnapshotManagerWorkspaceProjectStateChangeDetector(
                projectSnapshotManagerDispatcher.InternalDispatcher,
                workspaceStateGenerator.InternalWorkspaceStateGenerator);
        }

        internal WorkspaceProjectStateChangeDetector InternalWorkspaceProjectStateChangeDetector { get; }

        public void Initialize(OmniSharpProjectSnapshotManagerBase projectManager)
        {
            InternalWorkspaceProjectStateChangeDetector.Initialize(projectManager.InternalProjectSnapshotManager);
        }

        private class ProjectSnapshotManagerWorkspaceProjectStateChangeDetector : WorkspaceProjectStateChangeDetector
        {
            private readonly ProjectSnapshotManagerDispatcher _projectSnapshotManagerDispatcher;

            public ProjectSnapshotManagerWorkspaceProjectStateChangeDetector(
                ProjectSnapshotManagerDispatcher projectSnapshotManagerDispatcher!!,
                ProjectWorkspaceStateGenerator workspaceStateGenerator) : base(workspaceStateGenerator, projectSnapshotManagerDispatcher)
            {
                _projectSnapshotManagerDispatcher = projectSnapshotManagerDispatcher;
            }

            // We override the InitializeSolution in order to enforce calls to this to be on the project snapshot manager's
            // thread. OmniSharp currently has an issue where they update the Solution on multiple different threads resulting
            // in change events dispatching through the Workspace on multiple different threads. This normalizes
            // that abnormality.
            protected override void InitializeSolution(Solution solution)
            {
                _ = _projectSnapshotManagerDispatcher.RunOnDispatcherThreadAsync(
                    () =>
                    {
                        try
                        {
                            base.InitializeSolution(solution);
                        }
                        catch (Exception ex)
                        {
                            Debug.Fail("Unexpected error when initializing solution: " + ex);
                        }
                    },
                    CancellationToken.None).ConfigureAwait(false);
            }

            // We override Workspace_WorkspaceChanged in order to enforce calls to this to be on the project snapshot manager's
            // thread. OmniSharp currently has an issue where they update the Solution on multiple different threads resulting
            // in change events dispatching through the Workspace on multiple different threads. This normalizes
            // that abnormality.
            internal override void Workspace_WorkspaceChanged(object sender, WorkspaceChangeEventArgs args)
            {
                _ = _projectSnapshotManagerDispatcher.RunOnDispatcherThreadAsync(
                    () =>
                    {
                        try
                        {
                            base.Workspace_WorkspaceChanged(sender, args);
                        }
                        catch (Exception ex)
                        {
                            Debug.Fail("Unexpected error when handling a workspace changed event: " + ex);
                        }
                    },
                    CancellationToken.None).ConfigureAwait(false);
            }
        }
    }
}
