﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Serialization;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.AspNetCore.Razor.Test.Common.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Workspaces.Protocol.ProjectSystem;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.Razor.LanguageClient;
using Microsoft.VisualStudio.Razor.LanguageClient.ProjectSystem;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.LanguageServices.Razor.Test.LanguageClient.ProjectSystem;

public class RazorProjectInfoEndpointPublisherTest(ITestOutputHelper testOutput) : LanguageServerTestBase(testOutput)
{
    [Fact]
    [WorkItem("https://github.com/dotnet/aspnetcore/issues/35945")]
    public async Task ProjectManager_Changed_Remove_Change_NoopsOnDelayedPublish()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();

        var callCount = 0;
        var tagHelpers = ImmutableArray.Create(
            new TagHelperDescriptor(FileKinds.Component, "Namespace.FileNameOther", "Assembly", "FileName", "FileName document", "FileName hint",
                caseSensitive: false, tagMatchingRules: default, attributeDescriptors: default, allowedChildTags: default, metadata: null!, diagnostics: default));

        var initialProjectSnapshot = CreateProjectSnapshot(
            @"C:\path\to\project.csproj", ProjectWorkspaceState.Create(tagHelpers, CodeAnalysis.CSharp.LanguageVersion.Preview));
        var expectedProjectSnapshot = CreateProjectSnapshot(
            @"C:\path\to\project.csproj", ProjectWorkspaceState.Create(CodeAnalysis.CSharp.LanguageVersion.Preview));
        var requestInvoker = new Mock<LSPRequestInvoker>(MockBehavior.Strict);
        requestInvoker
            .Setup(r => r.ReinvokeRequestOnServerAsync<ProjectInfoParams, object>(
                LanguageServerConstants.RazorProjectInfoEndpoint,
                RazorLSPConstants.RazorLanguageServerName,
                It.IsAny<ProjectInfoParams>(),
                It.IsAny<CancellationToken>()))
            .Callback<string, string, ProjectInfoParams, CancellationToken>((s1, s2, param, ct) => callCount++)
            .ReturnsAsync(new ReinvokeResponse<object>());

        var publisher = RazorProjectInfoEndpointPublisher.GetTestAccessor(
            requestInvoker.Object,
            projectManager);

        var documentRemovedArgs = ProjectChangeEventArgs.CreateTestInstance(
            initialProjectSnapshot, initialProjectSnapshot, documentFilePath: @"C:\path\to\file.razor", ProjectChangeKind.DocumentRemoved);
        var projectChangedArgs = ProjectChangeEventArgs.CreateTestInstance(
            initialProjectSnapshot, expectedProjectSnapshot, documentFilePath: null!, ProjectChangeKind.ProjectChanged);

        // Act
        publisher.ProjectManager_Changed(null!, documentRemovedArgs);
        publisher.ProjectManager_Changed(null!, projectChangedArgs);
        await publisher.WaitUntilCurrentBatchCompletesAsync();

        // Assert
        Assert.Equal(1, callCount);
    }

    [Fact]
    public async Task ProjectManager_Changed_NotActive_Noops()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();

        var hostProject = new HostProject(@"C:\path\to\project.csproj", @"C:\path\to\obj", RazorConfiguration.Default, rootNamespace: "TestRootNamespace");
        var hostDocument = new HostDocument(@"C:\path\to\file.razor", "file.razor");

        await projectManager.UpdateAsync(updater =>
        {
            updater.ProjectAdded(hostProject);
        });

        var callCount = 0;
        var requestInvoker = new Mock<LSPRequestInvoker>(MockBehavior.Strict);
        requestInvoker
            .Setup(r => r.ReinvokeRequestOnServerAsync<ProjectInfoParams, object>(
                LanguageServerConstants.RazorProjectInfoEndpoint,
                RazorLSPConstants.RazorLanguageServerName,
                It.IsAny<ProjectInfoParams>(),
                It.IsAny<CancellationToken>()))
            .Callback<string, string, ProjectInfoParams, CancellationToken>((s1, s2, param, ct) => callCount++)
            .ReturnsAsync(new ReinvokeResponse<object>());

        var publisher = RazorProjectInfoEndpointPublisher.GetTestAccessor(
            requestInvoker.Object,
            projectManager);

        // Act
        await projectManager.UpdateAsync(updater =>
        {
            updater.DocumentAdded(hostProject.Key, hostDocument, new EmptyTextLoader(hostDocument.FilePath));
        });
        await publisher.WaitUntilCurrentBatchCompletesAsync();

        // Assert
        Assert.Equal(0, callCount);
    }

    [Fact]
    public async Task ProjectManager_Changed_ServerStarted_InitializedProject_Publishes()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();

        var hostProject = new HostProject(@"C:\path\to\project.csproj", @"C:\path\to\obj", RazorConfiguration.Default, rootNamespace: "TestRootNamespace");
        var hostDocument = new HostDocument(@"C:\path\to\file.razor", "file.razor");

        await projectManager.UpdateAsync(updater =>
        {
            updater.ProjectAdded(hostProject);
            updater.ProjectWorkspaceStateChanged(hostProject.Key, ProjectWorkspaceState.Default);
            updater.DocumentAdded(hostProject.Key, hostDocument, new EmptyTextLoader(hostDocument.FilePath));
        });

        var projectSnapshot = projectManager.GetProjects()[0];

        var callCount = 0;
        var requestInvoker = new Mock<LSPRequestInvoker>(MockBehavior.Strict);
        requestInvoker
            .Setup(r => r.ReinvokeRequestOnServerAsync<ProjectInfoParams, object>(
                LanguageServerConstants.RazorProjectInfoEndpoint,
                RazorLSPConstants.RazorLanguageServerName,
                It.IsAny<ProjectInfoParams>(),
                It.IsAny<CancellationToken>()))
            .Callback<string, string, ProjectInfoParams, CancellationToken>((s1, s2, param, ct) => callCount++)
            .ReturnsAsync(new ReinvokeResponse<object>());

        var publisher = RazorProjectInfoEndpointPublisher.GetTestAccessor(
            requestInvoker.Object,
            projectManager);

        // Act
        publisher.StartSending();

        // Assert
        Assert.Equal(1, callCount);
    }

    [Theory]
    [InlineData(ProjectChangeKind.DocumentAdded, true, false)]
    [InlineData(ProjectChangeKind.DocumentRemoved, true, false)]
    [InlineData(ProjectChangeKind.ProjectChanged, true, false)]
    [InlineData(ProjectChangeKind.ProjectRemoved, true, true)]
    [InlineData(ProjectChangeKind.ProjectAdded, false, false)]
    internal async Task ProjectManager_Changed_EnqueuesPublishAsync(ProjectChangeKind changeKind, bool waitForQueueEmpty, bool expectNullProjectInfo)
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();
        var projectConfigurationFilePathStore = new DefaultProjectConfigurationFilePathStore();

        var projectSnapshot = CreateProjectSnapshot(@"C:\path\to\project.csproj", ProjectWorkspaceState.Create(CodeAnalysis.CSharp.LanguageVersion.CSharp7_3));
        var expectedProjectInfo = projectSnapshot.ToRazorProjectInfo(projectSnapshot.IntermediateOutputPath);
        var callCount = 0;
        var requestInvoker = new Mock<LSPRequestInvoker>(MockBehavior.Strict);
        var projectInfoParams = (ProjectInfoParams?)null;
        requestInvoker
            .Setup(r => r.ReinvokeRequestOnServerAsync<ProjectInfoParams, object>(
                LanguageServerConstants.RazorProjectInfoEndpoint,
                RazorLSPConstants.RazorLanguageServerName,
                It.IsAny<ProjectInfoParams>(),
                It.IsAny<CancellationToken>()))
            .Callback<string, string, ProjectInfoParams, CancellationToken>((s1, s2, param, ct) =>
                {
                    callCount++;
                    projectInfoParams = param;
                })
            .ReturnsAsync(new ReinvokeResponse<object>());

        var publisher = RazorProjectInfoEndpointPublisher.GetTestAccessor(
            requestInvoker.Object,
            projectManager);

        var args = ProjectChangeEventArgs.CreateTestInstance(projectSnapshot, projectSnapshot, documentFilePath: null!, changeKind);

        // Act
        publisher.ProjectManager_Changed(null!, args);
        if (waitForQueueEmpty)
        {
            await publisher.WaitUntilCurrentBatchCompletesAsync();
        }

        // Assert
        Assert.Equal(1, callCount);
        var projectInfo =
            RazorProjectInfoDeserializer.Instance.DeserializeFromString(projectInfoParams!.ProjectInfo);
        if (expectNullProjectInfo)
        {
            Assert.Null(projectInfo);
        }
        else
        {
            Assert.NotNull(projectInfo);
            Assert.Equal(expectedProjectInfo, projectInfo);
        }
    }

    [Fact]
    public async Task EnqueuePublish_BatchesPublishRequestsAsync()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();

        var firstSnapshot = CreateProjectSnapshot(@"C:\path\to\project.csproj");
        var secondSnapshot = CreateProjectSnapshot(@"C:\path\to\project.csproj", [@"C:\path\to\file.cshtml"]);
        var expectedProjectInfoString = secondSnapshot.ToBase64EncodedProjectInfo(secondSnapshot.IntermediateOutputPath);

        var projectInfoParams = (ProjectInfoParams?)null;
        var callCount = 0;
        var requestInvoker = new Mock<LSPRequestInvoker>(MockBehavior.Strict);
        requestInvoker
            .Setup(r => r.ReinvokeRequestOnServerAsync<ProjectInfoParams, object>(
                LanguageServerConstants.RazorProjectInfoEndpoint,
                RazorLSPConstants.RazorLanguageServerName,
                It.IsAny<ProjectInfoParams>(),
                It.IsAny<CancellationToken>()))
            .Callback<string, string, ProjectInfoParams, CancellationToken>((s1, s2, param, ct) =>
            {
                callCount++;
                projectInfoParams = param;
            })
            .ReturnsAsync(new ReinvokeResponse<object>());

        var publisher = RazorProjectInfoEndpointPublisher.GetTestAccessor(
            requestInvoker.Object,
            projectManager);

        // Act
        publisher.EnqueuePublish(firstSnapshot);
        publisher.EnqueuePublish(secondSnapshot);
        await publisher.WaitUntilCurrentBatchCompletesAsync();

        // Assert
        Assert.Equal(1, callCount);
        Assert.NotNull(projectInfoParams);
        Assert.Equal(expectedProjectInfoString, projectInfoParams.ProjectInfo);
    }

    internal static IProjectSnapshot CreateProjectSnapshot(
        string projectFilePath,
        ProjectWorkspaceState? projectWorkspaceState = null,
        string[]? documentFilePaths = null)
    {
        return TestProjectSnapshot.Create(projectFilePath, documentFilePaths ?? [], projectWorkspaceState);
    }

    internal static IProjectSnapshot CreateProjectSnapshot(string projectFilePath, string[] documentFilePaths)
    {
        return TestProjectSnapshot.Create(projectFilePath, documentFilePaths);
    }
}
