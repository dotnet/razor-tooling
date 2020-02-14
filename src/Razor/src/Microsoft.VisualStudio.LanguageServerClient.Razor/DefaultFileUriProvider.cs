﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Composition;
using System.IO;
using Microsoft.VisualStudio.Text;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor
{
    [Shared]
    [Export(typeof(FileUriProvider))]
    internal class DefaultFileUriProvider : FileUriProvider
    {
        private readonly ITextDocumentFactoryService _textDocumentFactory;

        [ImportingConstructor]
        public DefaultFileUriProvider(ITextDocumentFactoryService textDocumentFactory)
        {
            if (textDocumentFactory is null)
            {
                throw new ArgumentNullException(nameof(textDocumentFactory));
            }

            _textDocumentFactory = textDocumentFactory;
        }

        public override Uri GetOrCreate(ITextBuffer textBuffer)
        {
            if (textBuffer is null)
            {
                throw new ArgumentNullException(nameof(textBuffer));
            }

            string filePath;
            if (_textDocumentFactory.TryGetTextDocument(textBuffer, out var textDocument))
            {
                filePath = textDocument.FilePath;
            }
            else
            {
                // TextBuffer doesn't have a file path, we need to fabricate one.
                filePath = Path.DirectorySeparatorChar + Guid.NewGuid().ToString();
            }

            var uri = new Uri(filePath);
            return uri;
        }
    }
}
