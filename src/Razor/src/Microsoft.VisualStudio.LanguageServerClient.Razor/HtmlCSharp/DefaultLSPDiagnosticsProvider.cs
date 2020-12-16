﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor.HtmlCSharp
{
    [Shared]
    [Export(typeof(LSPDiagnosticsProvider))]
    internal class DefaultLSPDiagnosticsProvider : LSPDiagnosticsProvider
    {
        private readonly LSPRequestInvoker _requestInvoker;

        [ImportingConstructor]
        public DefaultLSPDiagnosticsProvider(LSPRequestInvoker requestInvoker)
        {
            if (requestInvoker is null)
            {
                throw new ArgumentNullException(nameof(requestInvoker));
            }

            _requestInvoker = requestInvoker;
        }

        public override async Task<RazorDiagnosticsResponse> ProcessDiagnosticsAsync(
            RazorLanguageKind languageKind,
            Uri razorDocumentUri,
            Diagnostic[] diagnostics,
            LanguageServerMappingBehavior mappingBehavior,
            int hostDocumentVersion,
            CancellationToken cancellationToken)
        {
            if (razorDocumentUri is null)
            {
                throw new ArgumentNullException(nameof(razorDocumentUri));
            }

            if (diagnostics is null)
            {
                throw new ArgumentNullException(nameof(diagnostics));
            }

            var diagnosticsParams = new RazorDiagnosticsParams()
            {
                Kind = languageKind,
                RazorDocumentUri = razorDocumentUri,
                Diagnostics = diagnostics,
                MappingBehavior = mappingBehavior,
                HostDocumentVersion = hostDocumentVersion
            };

            var diagnosticResponse = await _requestInvoker.ReinvokeRequestOnServerAsync<RazorDiagnosticsParams, RazorDiagnosticsResponse>(
                LanguageServerConstants.RazorDiagnosticsEndpoint,
                RazorLSPConstants.RazorLSPContentTypeName,
                diagnosticsParams,
                cancellationToken).ConfigureAwait(false);

            return diagnosticResponse;
        }
    }
}
