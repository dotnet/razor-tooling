﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Utilities;

namespace Microsoft.VisualStudio.Razor.LanguageClient;

[Export(typeof(VirtualDocumentFactory))]
[ContentType(RazorConstants.RazorLSPContentTypeName)]
internal class HtmlVirtualDocumentFactory : VirtualDocumentFactoryBase
{
    private static IContentType? s_htmlLSPContentType;
    private readonly ILanguageServerFeatureOptionsProvider _optionsProvider;
    private readonly ITelemetryReporter _telemetryReporter;

    [ImportingConstructor]
    public HtmlVirtualDocumentFactory(
        IContentTypeRegistryService contentTypeRegistry,
        ITextBufferFactoryService textBufferFactory,
        ITextDocumentFactoryService textDocumentFactory,
        FileUriProvider filePathProvider,
        ILanguageServerFeatureOptionsProvider optionsProvider,
        ITelemetryReporter telemetryReporter)
        : base(contentTypeRegistry, textBufferFactory, textDocumentFactory, filePathProvider)
    {
        _optionsProvider = optionsProvider;
        _telemetryReporter = telemetryReporter;
    }

    protected override IContentType LanguageContentType
    {
        get
        {
            s_htmlLSPContentType ??= ContentTypeRegistry.GetContentType(RazorLSPConstants.HtmlLSPDelegationContentTypeName);

            return s_htmlLSPContentType;
        }
    }

    protected override string HostDocumentContentTypeName => RazorConstants.RazorLSPContentTypeName;
    protected override string LanguageFileNameSuffix => _optionsProvider.GetOptions().HtmlVirtualDocumentSuffix;
    protected override VirtualDocument CreateVirtualDocument(Uri uri, ITextBuffer textBuffer) => new HtmlVirtualDocument(uri, textBuffer, _telemetryReporter);
}
