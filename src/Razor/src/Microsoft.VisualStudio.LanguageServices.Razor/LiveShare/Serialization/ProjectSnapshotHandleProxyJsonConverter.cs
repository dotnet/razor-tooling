﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Serialization.Json;

namespace Microsoft.VisualStudio.Razor.LiveShare.Serialization;

internal class ProjectSnapshotHandleProxyJsonConverter : ObjectJsonConverter<ProjectSnapshotHandleProxy>
{
    public static readonly ProjectSnapshotHandleProxyJsonConverter Instance = new();

    protected override ProjectSnapshotHandleProxy ReadFromProperties(JsonDataReader reader)
    {
        var filePath = reader.ReadNonNullUri(nameof(ProjectSnapshotHandleProxy.FilePath));
        var intermediateOutputPath = reader.ReadNonNullUri(nameof(ProjectSnapshotHandleProxy.IntermediateOutputPath));
        var configuration = reader.ReadNonNullObject(nameof(ProjectSnapshotHandleProxy.Configuration), ObjectReaders.ReadConfigurationFromProperties);
        var projectWorkspaceState = reader.ReadObjectOrNull(nameof(ProjectSnapshotHandleProxy.ProjectWorkspaceState), ObjectReaders.ReadProjectWorkspaceStateFromProperties) ?? ProjectWorkspaceState.Default;

        return new(filePath, intermediateOutputPath, configuration, projectWorkspaceState);
    }

    protected override void WriteProperties(JsonDataWriter writer, ProjectSnapshotHandleProxy value)
    {
        writer.Write(nameof(value.FilePath), value.FilePath);
        writer.Write(nameof(value.IntermediateOutputPath), value.IntermediateOutputPath);
        writer.WriteObject(nameof(value.Configuration), value.Configuration, ObjectWriters.WriteProperties);
        writer.WriteObjectIfNotNull(nameof(value.ProjectWorkspaceState), value.ProjectWorkspaceState, ObjectWriters.WriteProperties);
    }
}
