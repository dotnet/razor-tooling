﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer;
using Microsoft.AspNetCore.Razor.LanguageServer.Formatting;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.LanguageServerClient.Razor.HtmlCSharp;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Threading;
using Moq;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Sdk;
using OmniSharpFormattingOptions = OmniSharp.Extensions.LanguageServer.Protocol.Models.FormattingOptions;
using OmniSharpPosition = OmniSharp.Extensions.LanguageServer.Protocol.Models.Position;
using OmniSharpRange = OmniSharp.Extensions.LanguageServer.Protocol.Models.Range;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor
{
    public class DefaultRazorLanguageServerCustomMessageTargetTest
    {
        public DefaultRazorLanguageServerCustomMessageTargetTest()
        {
            var joinableTaskContext = new JoinableTaskContextNode(new JoinableTaskContext());
            JoinableTaskContext = joinableTaskContext.Context;
        }

        private JoinableTaskContext JoinableTaskContext { get; }

        [Fact]
        public void UpdateCSharpBuffer_CanNotDeserializeRequest_NoopsGracefully()
        {
            // Arrange
            LSPDocumentSnapshot document;
            var documentManager = new Mock<TrackingLSPDocumentManager>();
            documentManager.Setup(manager => manager.TryGetDocument(It.IsAny<Uri>(), out document))
                .Throws<XunitException>();
            var target = new DefaultRazorLanguageServerCustomMessageTarget(documentManager.Object);
            var token = JToken.FromObject(new { });

            // Act & Assert
            target.UpdateCSharpBuffer(token);
        }

        [Fact]
        public void UpdateCSharpBuffer_CannotLookupDocument_NoopsGracefully()
        {
            // Arrange
            LSPDocumentSnapshot document;
            var documentManager = new Mock<TrackingLSPDocumentManager>();
            documentManager.Setup(manager => manager.TryGetDocument(It.IsAny<Uri>(), out document))
                .Returns(false);
            var target = new DefaultRazorLanguageServerCustomMessageTarget(documentManager.Object);
            var request = new UpdateBufferRequest()
            {
                HostDocumentFilePath = "C:/path/to/file.razor",
            };
            var token = JToken.FromObject(request);

            // Act & Assert
            target.UpdateCSharpBuffer(token);
        }

        [Fact]
        public void UpdateCSharpBuffer_UpdatesDocument()
        {
            // Arrange
            var documentManager = new Mock<TrackingLSPDocumentManager>();
            documentManager.Setup(manager => manager.UpdateVirtualDocument<CSharpVirtualDocument>(It.IsAny<Uri>(), It.IsAny<IReadOnlyList<TextChange>>(), 1337))
                .Verifiable();
            var target = new DefaultRazorLanguageServerCustomMessageTarget(documentManager.Object);
            var request = new UpdateBufferRequest()
            {
                HostDocumentFilePath = "C:/path/to/file.razor",
                HostDocumentVersion = 1337,
                Changes = Array.Empty<TextChange>(),
            };
            var token = JToken.FromObject(request);

            // Act
            target.UpdateCSharpBuffer(token);

            // Assert
            documentManager.VerifyAll();
        }

        [Fact]
        public async Task RazorRangeFormattingAsync_LanguageKindRazor_ReturnsEmpty()
        {
            // Arrange
            var documentManager = Mock.Of<TrackingLSPDocumentManager>();
            var requestInvoker = new Mock<LSPRequestInvoker>();
            var target = new DefaultRazorLanguageServerCustomMessageTarget(documentManager, JoinableTaskContext, requestInvoker.Object);

            var request = new RazorDocumentRangeFormattingParams()
            {
                HostDocumentFilePath = "c:/Some/path/to/file.razor",
                Kind = RazorLanguageKind.Razor,
                ProjectedRange = new OmniSharpRange(),
                Options = new OmniSharpFormattingOptions()
                {
                    TabSize = 4,
                    InsertSpaces = true
                }
            };

            // Act
            var result = await target.RazorRangeFormattingAsync(request, CancellationToken.None).ConfigureAwait(false);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result.Edits);
        }

        [Fact]
        public async Task RazorRangeFormattingAsync_DocumentNotFound_ReturnsEmpty()
        {
            // Arrange
            var documentManager = Mock.Of<TrackingLSPDocumentManager>();
            var requestInvoker = new Mock<LSPRequestInvoker>();
            var target = new DefaultRazorLanguageServerCustomMessageTarget(documentManager, JoinableTaskContext, requestInvoker.Object);

            var request = new RazorDocumentRangeFormattingParams()
            {
                HostDocumentFilePath = "c:/Some/path/to/file.razor",
                Kind = RazorLanguageKind.CSharp,
                ProjectedRange = new OmniSharpRange(),
                Options = new OmniSharpFormattingOptions()
                {
                    TabSize = 4,
                    InsertSpaces = true
                }
            };

            // Act
            var result = await target.RazorRangeFormattingAsync(request, CancellationToken.None).ConfigureAwait(false);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result.Edits);
        }

        [Fact]
        public async Task RazorRangeFormattingAsync_ValidRequest_InvokesLanguageServer()
        {
            // Arrange
            var filePath = "c:/Some/path/to/file.razor";
            var uri = new Uri(filePath);
            var virtualDocument = new CSharpVirtualDocumentSnapshot(new Uri($"{filePath}.g.cs"), Mock.Of<ITextSnapshot>(), 1);
            LSPDocumentSnapshot document = new TestLSPDocumentSnapshot(uri, 1, new[] { virtualDocument });
            var documentManager = new Mock<TrackingLSPDocumentManager>();
            documentManager.Setup(manager => manager.TryGetDocument(It.IsAny<Uri>(), out document))
                .Returns(true);

            var expectedEdit = new TextEdit()
            {
                NewText = "SomeEdit",
                Range = new LanguageServer.Protocol.Range() { Start = new Position(), End = new Position() }
            };
            var requestInvoker = new Mock<LSPRequestInvoker>();
            requestInvoker
                .Setup(r => r.RequestServerAsync<DocumentRangeFormattingParams, TextEdit[]>(It.IsAny<string>(), It.IsAny<LanguageServerKind>(), It.IsAny<DocumentRangeFormattingParams>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new[] { expectedEdit }));

            var target = new DefaultRazorLanguageServerCustomMessageTarget(documentManager.Object, JoinableTaskContext, requestInvoker.Object);

            var request = new RazorDocumentRangeFormattingParams()
            {
                HostDocumentFilePath = filePath,
                Kind = RazorLanguageKind.CSharp,
                ProjectedRange = new OmniSharpRange()
                {
                    Start = new OmniSharpPosition(),
                    End = new OmniSharpPosition()
                },
                Options = new OmniSharpFormattingOptions()
                {
                    TabSize = 4,
                    InsertSpaces = true
                }
            };

            // Act
            var result = await target.RazorRangeFormattingAsync(request, CancellationToken.None).ConfigureAwait(false);

            // Assert
            Assert.NotNull(result);
            var edit = Assert.Single(result.Edits);
            Assert.Equal("SomeEdit", edit.NewText);
        }
    }
}
