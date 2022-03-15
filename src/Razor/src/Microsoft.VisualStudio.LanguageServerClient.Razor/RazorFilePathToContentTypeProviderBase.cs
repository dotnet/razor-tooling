﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.VisualStudio.Utilities;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor
{
    internal abstract class RazorFilePathToContentTypeProviderBase : IFilePathToContentTypeProvider
    {
        private readonly IContentTypeRegistryService _contentTypeRegistryService;
        private readonly LSPEditorFeatureDetector _lspEditorFeatureDetector;

        public RazorFilePathToContentTypeProviderBase(
            IContentTypeRegistryService contentTypeRegistryService!!,
            LSPEditorFeatureDetector lspEditorFeatureDetector!!)
        {
            _contentTypeRegistryService = contentTypeRegistryService;
            _lspEditorFeatureDetector = lspEditorFeatureDetector;
        }

        public bool TryGetContentTypeForFilePath(string filePath, [NotNullWhen(true)] out IContentType? contentType)
        {
            if (_lspEditorFeatureDetector.IsLSPEditorAvailable(filePath, hierarchy: null))
            {
                contentType = _contentTypeRegistryService.GetContentType(RazorLSPConstants.RazorLSPContentTypeName);
                return true;
            }

            contentType = null;
            return false;
        }
    }
}
