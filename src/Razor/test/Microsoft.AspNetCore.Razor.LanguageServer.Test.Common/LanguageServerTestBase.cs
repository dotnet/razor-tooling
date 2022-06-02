﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.Serialization;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;
using Microsoft.Extensions.Logging;
using Moq;
using OmniSharp.Extensions.LanguageServer.Protocol.Serialization;

namespace Microsoft.AspNetCore.Razor.Test.Common
{
    public abstract class LanguageServerTestBase
    {
        public LanguageServerTestBase()
        {
#pragma warning disable CS0618 // Type or member is obsolete
            LegacyDispatcher = new TestProjectSnapshotManagerDispatcher();
#pragma warning restore CS0618 // Type or member is obsolete
            FilePathNormalizer = new FilePathNormalizer();
            var logger = new Mock<ILogger>(MockBehavior.Strict).Object;
            Mock.Get(logger).Setup(l => l.Log(It.IsAny<LogLevel>(), It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(), It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception, string>>())).Verifiable();
            Mock.Get(logger).Setup(l => l.IsEnabled(It.IsAny<LogLevel>())).Returns(false);
            LoggerFactory = Mock.Of<ILoggerFactory>(factory => factory.CreateLogger(It.IsAny<string>()) == logger, MockBehavior.Strict);
            Serializer = new LspSerializer();
            Serializer.RegisterRazorConverters();
            Serializer.RegisterVSInternalExtensionConverters();
        }

        // This is marked as legacy because in its current form it's being assigned a "TestProjectSnapshotManagerDispatcher" which takes the
        // synchronization context from the constructing thread and binds to that. We've seen in XUnit how this can unexpectedly lead to flaky
        // tests since it doesn't actually replicate what happens in real scenario (a separate dedicated dispatcher thread). If you're reading
        // this write your tests using the normal Dispatcher property. Eventually this LegacyDispatcher property will go away when we've had
        // the opportunity to re-write our tests correctly.
        internal ProjectSnapshotManagerDispatcher LegacyDispatcher { get; }

        internal readonly ProjectSnapshotManagerDispatcher Dispatcher = new LSPProjectSnapshotManagerDispatcher(TestLoggerFactory.Instance);

        internal FilePathNormalizer FilePathNormalizer { get; }

        protected LspSerializer Serializer { get; }

        protected ILoggerFactory LoggerFactory { get; }

        protected static RazorCodeDocument CreateCodeDocument(string text, IReadOnlyList<TagHelperDescriptor> tagHelpers = null)
        {
            tagHelpers ??= Array.Empty<TagHelperDescriptor>();
            var sourceDocument = TestRazorSourceDocument.Create(text);
            var projectEngine = RazorProjectEngine.Create(RazorConfiguration.Default, RazorProjectFileSystem.Create("/"), builder => { });
            var codeDocument = projectEngine.ProcessDesignTime(sourceDocument, "mvc", Array.Empty<RazorSourceDocument>(), tagHelpers);
            return codeDocument;
        }

        internal static DocumentContextFactory CreateDocumentContextFactory(Uri documentPath, string sourceText)
        {
            var codeDocument = CreateCodeDocument(sourceText);
            return CreateDocumentContextFactory(documentPath, codeDocument);
        }

        internal static DocumentContextFactory CreateDocumentContextFactory(
            Uri documentPath,
            RazorCodeDocument codeDocument,
            bool documentFound = true,
            int version = 1337)
        {
            var sourceTextChars = new char[codeDocument.Source.Length];
            codeDocument.Source.CopyTo(0, sourceTextChars, 0, codeDocument.Source.Length);
            var sourceText = SourceText.From(new string(sourceTextChars));

            var snapshot = new Mock<DocumentSnapshot>(MockBehavior.Strict);
            var outVal = documentFound ? codeDocument : null;
            snapshot.Setup(s => s.TryGetGeneratedOutput(out outVal)).Returns(documentFound);
            snapshot.Setup(s => s.GetTextAsync()).Returns(Task.FromResult(sourceText));
            snapshot.Setup(s => s.GetGeneratedOutputAsync()).Returns(Task.FromResult(codeDocument));
            snapshot.SetupGet(s => s.FilePath).Returns(documentPath.GetAbsoluteOrUNCPath());
            snapshot.SetupGet(s => s.FileKind).Returns(codeDocument.GetFileKind());
            
            var documentContext = new DocumentContext(documentPath, snapshot.Object, version);
            var documentContextFactory = new Mock<DocumentContextFactory>(MockBehavior.Strict);
            documentContextFactory.Setup(factory => factory.TryCreateAsync(documentPath, It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(documentFound ? documentContext : null));

            return documentContextFactory.Object;
        }

        internal static DocumentContext  CreateDocumentContext(Uri uri, DocumentSnapshot snapshot)
        {
            return new DocumentContext(uri, snapshot, version: 0);
        }

        [Obsolete("Use " + nameof(LSPProjectSnapshotManagerDispatcher))]
        private class TestProjectSnapshotManagerDispatcher : ProjectSnapshotManagerDispatcher
        {
            public TestProjectSnapshotManagerDispatcher()
            {
                DispatcherScheduler = SynchronizationContext.Current is null
                    ? new ThrowingTaskScheduler()
                    : TaskScheduler.FromCurrentSynchronizationContext();
            }

            public override TaskScheduler DispatcherScheduler { get; }

            private Thread Thread { get; } = Thread.CurrentThread;

            public override bool IsDispatcherThread => Thread.CurrentThread == Thread;
        }

        private class ThrowingTaskScheduler : TaskScheduler
        {
            protected override IEnumerable<Task> GetScheduledTasks()
            {
                return Enumerable.Empty<Task>();
            }

            protected override void QueueTask(Task task)
            {
                throw new NotImplementedException();
            }

            protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
            {
                throw new NotImplementedException();
            }
        }
    }
}
