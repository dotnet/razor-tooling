﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Test;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Utilities;
using Moq;
using Xunit;

namespace Microsoft.VisualStudio.LanguageServer.ContainedLanguage
{
    public class DefaultLSPDocumentTest
    {
        public DefaultLSPDocumentTest()
        {
            Uri = new Uri("C:/path/to/file.razor__virtual.cs");
            NotInertContentType = Mock.Of<IContentType>(contentType => contentType.IsOfType("inert") == false, MockBehavior.Strict);
        }

        private Uri Uri { get; }

        private IContentType NotInertContentType { get; }

        [Fact]
        public void InertTextBuffer_DoesNotCreateSnapshot()
        {
            // Arrange
            var textBuffer = new TestTextBuffer(new StringTextSnapshot(string.Empty));
            textBuffer.ChangeContentType(NotInertContentType, editTag: null);
            using var document = new DefaultLSPDocument(Uri, textBuffer, virtualDocuments: Array.Empty<VirtualDocument>());
            var originalSnapshot = document.CurrentSnapshot;
            textBuffer.ChangeContentType(TestInertContentType.Instance, editTag: null);

            // Act
            var edit = textBuffer.CreateEdit();
            edit.Insert(0, "New!");
            edit.Apply();

            // Assert
            var newSnapshot = document.CurrentSnapshot;
            Assert.Same(originalSnapshot, newSnapshot);
        }

        [Fact]
        public void CurrentSnapshot_ChangesWhenTextBufferChanges()
        {
            // Arrange
            var textBuffer = new TestTextBuffer(new StringTextSnapshot(string.Empty));
            textBuffer.ChangeContentType(NotInertContentType, editTag: null);
            using var document = new DefaultLSPDocument(Uri, textBuffer, virtualDocuments: Array.Empty<VirtualDocument>());
            var originalSnapshot = document.CurrentSnapshot;

            // Act
            var edit = textBuffer.CreateEdit();
            edit.Insert(0, "New!");
            edit.Apply();

            // Assert
            var newSnapshot = document.CurrentSnapshot;
            Assert.NotSame(originalSnapshot, newSnapshot);
            Assert.Equal(1, originalSnapshot.Version);
            Assert.Equal(2, newSnapshot.Version);
        }

        [Fact]
        public void UpdateVirtualDocument_UpdatesProvidedVirtualDocumentWithProvidedArgs_AndRecalcsSnapshot()
        {
            // Arrange
            var textBuffer = new TestTextBuffer(new StringTextSnapshot(string.Empty));
            var virtualDocument = new TestVirtualDocument();
            using var document = new DefaultLSPDocument(Uri, textBuffer, new[] { virtualDocument });
            var changes = Array.Empty<ITextChange>();
            var originalSnapshot = document.CurrentSnapshot;

            // Act
            document.UpdateVirtualDocument<TestVirtualDocument>(changes, hostDocumentVersion: 1337);

            // Assert
            Assert.Equal(1337, virtualDocument.HostDocumentVersion);
            Assert.Same(changes, virtualDocument.Changes);
            Assert.NotEqual(originalSnapshot, document.CurrentSnapshot);
        }

        private class TestVirtualDocument : VirtualDocument
        {
            private int _hostDocumentVersion;

            public IReadOnlyList<ITextChange> Changes { get; private set; }

            public override Uri Uri => throw new NotImplementedException();

            public override ITextBuffer TextBuffer => throw new NotImplementedException();

            public override VirtualDocumentSnapshot CurrentSnapshot => null;

            [Obsolete]
            public override long? HostDocumentSyncVersion => throw new NotImplementedException();

            public override int HostDocumentVersion => _hostDocumentVersion;

            [Obsolete]
            public override VirtualDocumentSnapshot Update(IReadOnlyList<ITextChange> changes, long hostDocumentVersion)
            {
                throw new NotImplementedException();
            }

            public override VirtualDocumentSnapshot Update(IReadOnlyList<ITextChange> changes, int hostDocumentVersion)
            {
                _hostDocumentVersion = hostDocumentVersion;
                Changes = changes;

                return null;
            }

            public override void Dispose()
            {
            }
        }
    }
}
