﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Composition;
using Microsoft.CodeAnalysis.Host;
using Microsoft.CodeAnalysis.Host.Mef;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Editor;

namespace Microsoft.VisualStudio.Editor.Razor
{
    [Shared]
    [ExportLanguageServiceFactory(typeof(WorkspaceEditorSettings), RazorLanguage.Name)]
    internal class DefaultWorkspaceEditorSettingsFactory : ILanguageServiceFactory
    {
        private readonly SingleThreadedDispatcher _singleThreadedDispatcher;
        private readonly EditorSettingsManager _editorSettingsManager;

        [ImportingConstructor]
        public DefaultWorkspaceEditorSettingsFactory(SingleThreadedDispatcher singleThreadedDispatcher, EditorSettingsManager editorSettingsManager)
        {
            if (singleThreadedDispatcher == null)
            {
                throw new ArgumentNullException(nameof(singleThreadedDispatcher));
            }

            if (editorSettingsManager == null)
            {
                throw new ArgumentNullException(nameof(editorSettingsManager));
            }

            _singleThreadedDispatcher = singleThreadedDispatcher;
            _editorSettingsManager = editorSettingsManager;
        }

        public ILanguageService CreateLanguageService(HostLanguageServices languageServices)
        {
            if (languageServices == null)
            {
                throw new ArgumentNullException(nameof(languageServices));
            }

            return new DefaultWorkspaceEditorSettings(_singleThreadedDispatcher, _editorSettingsManager);
        }
    }
}
