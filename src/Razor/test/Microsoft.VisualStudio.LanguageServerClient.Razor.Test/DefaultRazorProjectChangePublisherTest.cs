﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Threading;
using Moq;
using Xunit;
using Xunit.Sdk;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor.Test
{
    public class DefaultRazorProjectChangePublisherTest : LanguageServerTestBase, IDisposable
    {
        private readonly JoinableTaskContext JoinableTaskContext = new JoinableTaskContext();

        private readonly RazorLogger RazorLogger = Mock.Of<RazorLogger>();

        [Theory]
        [InlineData(ProjectChangeKind.DocumentAdded)]
        [InlineData(ProjectChangeKind.DocumentRemoved)]
        [InlineData(ProjectChangeKind.ProjectChanged)]
        internal async Task ProjectManager_Changed_EnqueuesPublishAsync(ProjectChangeKind changeKind)
        {
            // Arrange
            var serializationSuccessful = false;
            var projectSnapshot = CreateProjectSnapshot("/path/to/project.csproj");
            var expectedPublishFilePath = "/path/to/obj/bin/Debug/project.razor.json";
            var publisher = new TestDefaultRazorProjectChangePublisher(
                JoinableTaskContext,
                RazorLogger,
                onSerializeToFile: (snapshot, publishFilePath) =>
                {
                    Assert.Same(projectSnapshot, snapshot);
                    Assert.Equal(expectedPublishFilePath, publishFilePath);
                    serializationSuccessful = true;
                })
            {
                EnqueueDelay = 10
            };
            publisher.SetPublishFilePath(projectSnapshot.FilePath, expectedPublishFilePath);
            var args = ProjectChangeEventArgs.CreateTestInstance(projectSnapshot, projectSnapshot, documentFilePath: null, changeKind);

            // Act
            publisher.ProjectSnapshotManager_Changed(null, args);

            // Assert
            var kvp = Assert.Single(publisher._deferredPublishTasks);
            await kvp.Value.ConfigureAwait(false);
            Assert.True(serializationSuccessful);
        }

        [Fact]
        public async Task ProjectManager_Changed_ProjectRemoved_AfterEnqueuedPublishAsync()
        {
            // Arrange
            var attemptedToSerialize = false;
            var projectSnapshot = CreateProjectSnapshot("/path/to/project.csproj");
            var expectedPublishFilePath = "/path/to/obj/bin/Debug/project.razor.json";
            var publisher = new TestDefaultRazorProjectChangePublisher(
                JoinableTaskContext,
                RazorLogger,
                onSerializeToFile: (snapshot, publishFilePath) => attemptedToSerialize = true)
            {
                EnqueueDelay = 10
            };
            publisher.SetPublishFilePath(projectSnapshot.FilePath, expectedPublishFilePath);
            publisher.EnqueuePublish(projectSnapshot);
            var args = ProjectChangeEventArgs.CreateTestInstance(projectSnapshot, newer: null, documentFilePath: null, ProjectChangeKind.ProjectRemoved);

            // Act
            publisher.ProjectSnapshotManager_Changed(null, args);

            // Assert
            await Task.Delay(publisher.EnqueueDelay * 3).ConfigureAwait(false);

            Assert.False(attemptedToSerialize);
        }

        [Fact]
        public async Task EnqueuePublish_BatchesPublishRequestsAsync()
        {
            // Arrange
            var serializationSuccessful = false;
            var firstSnapshot = CreateProjectSnapshot("/path/to/project.csproj");
            var secondSnapshot = CreateProjectSnapshot("/path/to/project.csproj", new[] { "/path/to/file.cshtml" });
            var expectedPublishFilePath = "/path/to/obj/bin/Debug/project.razor.json";
            var publisher = new TestDefaultRazorProjectChangePublisher(
                JoinableTaskContext,
                RazorLogger,
                onSerializeToFile: (snapshot, publishFilePath) =>
                {
                    Assert.Same(secondSnapshot, snapshot);
                    Assert.Equal(expectedPublishFilePath, publishFilePath);
                    serializationSuccessful = true;
                })
            {
                EnqueueDelay = 10
            };
            publisher.SetPublishFilePath(firstSnapshot.FilePath, expectedPublishFilePath);

            // Act
            publisher.EnqueuePublish(firstSnapshot);
            publisher.EnqueuePublish(secondSnapshot);

            // Assert
            var kvp = Assert.Single(publisher._deferredPublishTasks);
            await kvp.Value.ConfigureAwait(false);
            Assert.True(serializationSuccessful);
        }

        [Fact]
        public void Publish_UnsetPublishFilePath_Noops()
        {
            // Arrange
            var publisher = new TestDefaultRazorProjectChangePublisher(JoinableTaskContext, RazorLogger);
            var omniSharpProjectSnapshot = CreateProjectSnapshot("/path/to/project.csproj");

            // Act & Assert
            publisher.Publish(omniSharpProjectSnapshot);
        }

        [Fact]
        public void Publish_PublishesToSetPublishFilePath()
        {
            // Arrange
            var serializationSuccessful = false;
            var omniSharpProjectSnapshot = CreateProjectSnapshot("/path/to/project.csproj");
            var expectedPublishFilePath = "/path/to/obj/bin/Debug/project.razor.json";
            var publisher = new TestDefaultRazorProjectChangePublisher(
                JoinableTaskContext,
                RazorLogger,
                onSerializeToFile: (snapshot, publishFilePath) =>
                {
                    Assert.Same(omniSharpProjectSnapshot, snapshot);
                    Assert.Equal(expectedPublishFilePath, publishFilePath);
                    serializationSuccessful = true;
                });
            publisher.SetPublishFilePath(omniSharpProjectSnapshot.FilePath, expectedPublishFilePath);

            // Act
            publisher.Publish(omniSharpProjectSnapshot);

            // Assert
            Assert.True(serializationSuccessful);
        }

        [ForegroundFact]
        public async Task ProjectAdded_DoesNotPublishWithoutProjectWorkspaceStateAsync()
        {
            // Arrange
            var serializationSuccessful = false;
            var expectedPublishFilePath = "/path/to/obj/bin/Debug/project.razor.json";

            var hostProject = new HostProject("/path/to/project.csproj", RazorConfiguration.Default, "TestRootNamespace");
            var snapshotManager = CreateProjectSnapshotManager(hostProject.FilePath, allowNotifyListeners: true);

            var publisher = new TestDefaultRazorProjectChangePublisher(
                JoinableTaskContext,
                RazorLogger,
                onSerializeToFile: (snapshot, publishFilePath) =>
                {
                    Assert.Equal(expectedPublishFilePath, publishFilePath);
                    serializationSuccessful = true;
                });
            publisher.Initialize(snapshotManager);
            publisher.SetPublishFilePath(hostProject.FilePath, expectedPublishFilePath);

            // Act
            await RunOnForegroundAsync(() => snapshotManager.ProjectAdded(hostProject)).ConfigureAwait(false);

            // Assert
            Assert.False(serializationSuccessful);
        }

        [ForegroundFact]
        public async Task ProjectRemoved_UnSetPublishFilePath_NoopsAsync()
        {
            // Arrange
            var hostProject = new HostProject("/path/to/project.csproj", RazorConfiguration.Default, "TestRootNamespace");
            var snapshotManager = CreateProjectSnapshotManager(hostProject.FilePath, allowNotifyListeners: true);
            var publisher = new TestDefaultRazorProjectChangePublisher(JoinableTaskContext, RazorLogger);
            publisher.Initialize(snapshotManager);
            await RunOnForegroundAsync(() => snapshotManager.ProjectAdded(hostProject)).ConfigureAwait(false);

            // Act & Assert
            await RunOnForegroundAsync(() => snapshotManager.ProjectRemoved(hostProject)).ConfigureAwait(false);
        }

        internal ProjectSnapshot CreateProjectSnapshot(string projectFilePath)
        {
            var testProjectSnapshot = TestProjectSnapshot.Create(projectFilePath);

            return testProjectSnapshot;
        }

        internal ProjectSnapshot CreateProjectSnapshot(string projectFilePath, string[] documentFilePaths)
        {
            var testProjectSnapshot = TestProjectSnapshot.Create(projectFilePath, documentFilePaths);

            return testProjectSnapshot;
        }

        internal ProjectSnapshotManagerBase CreateProjectSnapshotManager(string projectFilePath, bool allowNotifyListeners = false)
        {
            var snapshotManager = TestProjectSnapshotManager.Create(Dispatcher);
            snapshotManager.AllowNotifyListeners = allowNotifyListeners;
            var projectWorkspaceState = new ProjectWorkspaceState(new TagHelperDescriptor[] { }, CodeAnalysis.CSharp.LanguageVersion.Default);
            snapshotManager.ProjectWorkspaceStateChanged(projectFilePath, projectWorkspaceState);

            return snapshotManager;
        }

        protected Task RunOnForegroundAsync(Action action)
        {
            return Task.Factory.StartNew(
                () => action(),
                CancellationToken.None,
                TaskCreationOptions.None,
                Dispatcher.ForegroundScheduler);
        }

        protected Task<TReturn> RunOnForegroundAsync<TReturn>(Func<TReturn> action)
        {
            return Task.Factory.StartNew(
                () => action(),
                CancellationToken.None,
                TaskCreationOptions.None,
                Dispatcher.ForegroundScheduler);
        }

        protected Task RunOnForegroundAsync(Func<Task> action)
        {
            return Task.Factory.StartNew(
                async () => await action().ConfigureAwait(true),
                CancellationToken.None,
                TaskCreationOptions.None,
                Dispatcher.ForegroundScheduler);
        }

        private class TestDefaultRazorProjectChangePublisher : DefaultRazorProjectChangePublisher
        {
            private static readonly Mock<LSPEditorFeatureDetector> _lspEditorFeatureDetector = new Mock<LSPEditorFeatureDetector>();

            private readonly Action<ProjectSnapshot, string> _onSerializeToFile;

            static TestDefaultRazorProjectChangePublisher()
            {
                _lspEditorFeatureDetector
                    .Setup(t => t.IsLSPEditorAvailable(It.IsAny<string>(), It.IsAny<IVsHierarchy>()))
                    .Returns(true);
            }

            public TestDefaultRazorProjectChangePublisher(
                JoinableTaskContext joinableTaskContext,
                RazorLogger logger,
                Action<ProjectSnapshot, string> onSerializeToFile = null)
                : base(joinableTaskContext, _lspEditorFeatureDetector.Object, logger)
            {
                _onSerializeToFile = onSerializeToFile ?? ((_, __) => throw new XunitException("SerializeToFile should not have been called."));
            }

            protected override void SerializeToFile(ProjectSnapshot projectSnapshot, string publishFilePath) => _onSerializeToFile?.Invoke(projectSnapshot, publishFilePath);
        }

        public void Dispose()
        {
            JoinableTaskContext.Dispose();
        }
    }
}
