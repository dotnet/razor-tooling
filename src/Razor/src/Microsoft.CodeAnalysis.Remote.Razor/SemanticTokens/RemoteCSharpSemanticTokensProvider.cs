﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Cohost.Handlers;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.SemanticTokens;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Remote.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.Remote.Razor.SemanticTokens;

[Export(typeof(ICSharpSemanticTokensProvider)), Shared]
internal class RemoteCSharpSemanticTokensProvider : ICSharpSemanticTokensProvider
{
    public async Task<int[]?> GetCSharpSemanticTokensResponseAsync(VersionedDocumentContext documentContext, ImmutableArray<LinePositionSpan> csharpRanges, bool usePreciseSemanticTokenRanges, Guid correlationId, CancellationToken cancellationToken)
    {
        // TODO: Logic for usePreciseSemanticTokenRanges

        // We have a razor document, lets find the generated C# document
        var generatedDocument = GetGeneratedDocument(documentContext);

        var data = await SemanticTokensRange.GetSemanticTokensAsync(
            generatedDocument,
            csharpRanges,
            supportsVisualStudioExtensions: true,
            cancellationToken).ConfigureAwait(false);

        return data;
    }

    private static Document GetGeneratedDocument(VersionedDocumentContext documentContext)
    {
        var snapshot = (RemoteDocumentSnapshot)documentContext.Snapshot;
        var razorDocument = snapshot.TextDocument;
        var solution = razorDocument.Project.Solution;

        // TODO: A real implementation needs to get the SourceGeneratedDocument from the solution

        var projectKey = ProjectKey.From(razorDocument.Project).AssumeNotNull();
        var generatedFilePath = FilePathService.GetGeneratedFilePath(projectKey, razorDocument.FilePath.AssumeNotNull(), suffix: ".ide.g.cs", includeProjectKeyInGeneratedFilePath: true);
        var generatedDocumentId = solution.GetDocumentIdsWithFilePath(generatedFilePath).First(d => d.ProjectId == razorDocument.Project.Id);
        var generatedDocument = solution.GetDocument(generatedDocumentId).AssumeNotNull();
        return generatedDocument;
    }
}