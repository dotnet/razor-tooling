﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.LanguageServerClient.Razor.Extensions;
using Microsoft.VisualStudio.LanguageServerClient.Razor.HtmlCSharp;
using Microsoft.VisualStudio.LanguageServerClient.Razor.Test;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor.Debugging
{
    internal class TestLSPProjectionProvider : LSPProjectionProvider
    {
        private readonly DefaultRazorDocumentMappingService _mappingService = new(TestLoggerFactory.Instance);

        public override Task<ProjectionResult> GetProjectionAsync(LSPDocumentSnapshot documentSnapshot, Position position, CancellationToken cancellationToken)
        {
            var text = documentSnapshot.Snapshot.GetText();
            var sourceText = SourceText.From(text);
            if (!position.TryGetAbsoluteIndex(sourceText, TestLogger.Instance, out var absoluteIndex))
            {
                return Task.FromResult<ProjectionResult>(null);
            }

            var sourceDocument = TestRazorSourceDocument.Create(text, filePath: null, relativePath: null);
            var projectEngine = RazorProjectEngine.Create(builder => { });
            var codeDocument = projectEngine.ProcessDesignTime(sourceDocument, FileKinds.Component, Array.Empty<RazorSourceDocument>(), Array.Empty<TagHelperDescriptor>());

            if (!_mappingService.TryMapToProjectedDocumentPosition(codeDocument, absoluteIndex, out var projectedPosition, out var projectedIndex))
            {
                return Task.FromResult<ProjectionResult>(null);
            }

            var vsProjectedPosition = new Position { Line = projectedPosition.Line, Character = projectedPosition.Character };
            if (documentSnapshot.TryGetVirtualDocument<CSharpVirtualDocumentSnapshot>(out var csharpVirtualDocument))
            {
                var projectionResult = new ProjectionResult { Uri = csharpVirtualDocument.Uri, Position = vsProjectedPosition, PositionIndex = projectedIndex, LanguageKind = RazorLanguageKind.CSharp };
                return Task.FromResult(projectionResult);
            }

            return Task.FromResult<ProjectionResult>(null);
        }

        public override Task<ProjectionResult> GetProjectionForCompletionAsync(LSPDocumentSnapshot documentSnapshot, Position position, CancellationToken cancellationToken)
            => GetProjectionAsync(documentSnapshot, position, cancellationToken);
    }
}
