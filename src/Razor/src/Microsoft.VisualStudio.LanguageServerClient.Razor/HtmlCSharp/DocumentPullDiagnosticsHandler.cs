﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Composition;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor.HtmlCSharp
{
    [Shared]
    [ExportLspMethod(MSLSPMethods.DocumentPullDiagnosticName)]
    internal class DocumentPullDiagnosticsHandler :
        IRequestHandler<DocumentDiagnosticsParams, DiagnosticReport[]>
    {
        private static readonly IReadOnlyCollection<string> DiagnosticsToIgnore = new HashSet<string>()
        {
            "RemoveUnnecessaryImportsFixable",
            "IDE0005_gen", // Using directive is unnecessary
        };

        private readonly LSPRequestInvoker _requestInvoker;
        private readonly LSPDocumentManager _documentManager;
        private readonly LSPDocumentMappingProvider _documentMappingProvider;

        [ImportingConstructor]
        public DocumentPullDiagnosticsHandler(
            LSPRequestInvoker requestInvoker,
            LSPDocumentManager documentManager,
            LSPDocumentMappingProvider documentMappingProvider)
        {
            if (requestInvoker is null)
            {
                throw new ArgumentNullException(nameof(requestInvoker));
            }

            if (documentManager is null)
            {
                throw new ArgumentNullException(nameof(documentManager));
            }

            if (documentMappingProvider is null)
            {
                throw new ArgumentNullException(nameof(documentMappingProvider));
            }

            _requestInvoker = requestInvoker;
            _documentManager = documentManager;
            _documentMappingProvider = documentMappingProvider;
        }

        // Internal for testing
        public async Task<DiagnosticReport[]> HandleRequestAsync(DocumentDiagnosticsParams request, ClientCapabilities clientCapabilities, CancellationToken cancellationToken)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (clientCapabilities is null)
            {
                throw new ArgumentNullException(nameof(clientCapabilities));
            }

            if (!_documentManager.TryGetDocument(request.TextDocument.Uri, out var documentSnapshot))
            {
                return null;
            }

            if (!documentSnapshot.TryGetVirtualDocument<CSharpVirtualDocumentSnapshot>(out var csharpDoc))
            {
                return null;
            }

            var referenceParams = new DocumentDiagnosticsParams()
            {
                TextDocument = new TextDocumentIdentifier()
                {
                    Uri = csharpDoc.Uri
                },
                PreviousResultId = request.PreviousResultId
            };

            var time_init = stopwatch.ElapsedMilliseconds;


            /// Request Multiple
            // End goal is to transition this from ReinvokeRequestOnMultipleServersAsync -> ReinvokeRequestOnServerAsync
            // We can't do this right now as we don't have the ability to specify the language client name we'd like to make the call out to
            // https://devdiv.visualstudio.com/DevDiv/_workitems/edit/1246135
            var resultsFromAllLanguageServers = await _requestInvoker.ReinvokeRequestOnMultipleServersAsync<DocumentDiagnosticsParams, DiagnosticReport[]>(
                MSLSPMethods.DocumentPullDiagnosticName,
                RazorLSPConstants.CSharpContentTypeName,
                referenceParams,
                cancellationToken).ConfigureAwait(false);

            var time_roslyn = stopwatch.ElapsedMilliseconds;

            var result = resultsFromAllLanguageServers.SelectMany(l => l).ToArray();


            /// Request Single
            //var result = await _requestInvoker.ReinvokeRequestOnServerAsync<DocumentDiagnosticsParams, DiagnosticReport[]>(
            //    MSLSPMethods.DocumentPullDiagnosticName,
            //    RazorLSPConstants.CSharpContentTypeName,
            //    referenceParams,
            //    cancellationToken).ConfigureAwait(false);

            //var time_roslyn = stopwatch.ElapsedMilliseconds;







            var time_flatten = stopwatch.ElapsedMilliseconds;

            var processedResults = await RemapDocumentDiagnosticsAsync(
                result,
                request.TextDocument.Uri,
                documentSnapshot,
                cancellationToken).ConfigureAwait(false);


            var time_postProcess = stopwatch.ElapsedMilliseconds;
            stopwatch.Stop();

            var perfMessage = $"Found {processedResults?.FirstOrDefault()?.Diagnostics?.Length ?? -1} diagnostics, (Init {time_init}; Roslyn: {time_roslyn - time_init}; Flatten: {time_flatten - time_roslyn}; Post Process {time_postProcess - time_flatten})";

            // Debug.Assert(false, perfMessage);
            Debug.WriteLine(perfMessage);

            // | ---------------------------------------------------------------------------------- |
            // |                       LSP Platform Expected Response Semantics                     |
            // | ---------------------------------------------------------------------------------- |
            // | DiagnosticReport.Diagnostics     | DiagnosticReport.ResultId | Meaning             |
            // | -------------------------------- | ------------------------- | ------------------- |
            // | `null`                           | `null`                    | document gone       |
            // | `null`                           | valid                     | nothing changed     |
            // | valid (non-null including empty) | valid                     | diagnostics changed |
            // | ---------------------------------------------------------------------------------- |
            return processedResults;
        }

        private async Task<DiagnosticReport[]> RemapDocumentDiagnosticsAsync(
            DiagnosticReport[] unmappedDiagnosticReports,
            Uri razorDocumentUri,
            LSPDocumentSnapshot documentSnapshot,
            CancellationToken cancellationToken)
        {
            if (unmappedDiagnosticReports?.Any() != true)
            {
                return unmappedDiagnosticReports;
            }

            var mappedDiagnosticReports = new List<DiagnosticReport>(unmappedDiagnosticReports.Length);

            foreach (var diagnosticReport in unmappedDiagnosticReports)
            {
                // Check if there are any diagnostics in this report
                if (diagnosticReport?.Diagnostics?.Any() != true)
                {
                    mappedDiagnosticReports.Add(diagnosticReport);
                    continue;
                }

                var unmappedDiagnostics = diagnosticReport.Diagnostics;
                var filteredDiagnostics = unmappedDiagnostics.Where(d => !CanDiagnosticBeFiltered(d));
                if (!filteredDiagnostics.Any())
                {
                    // No diagnostics left after filtering.
                    // Discard the diagnostics from this DiagnosticReport,
                    // and report there aren't any document diagnostics.
                    diagnosticReport.Diagnostics = Array.Empty<Diagnostic>();
                    mappedDiagnosticReports.Add(diagnosticReport);
                    continue;
                }

                var mappedDiagnostics = new List<Diagnostic>(filteredDiagnostics.Count());

                var rangesToMap = filteredDiagnostics.Select(r => r.Range).ToArray();
                var mappingResult = await _documentMappingProvider.MapToDocumentRangesAsync(
                    RazorLanguageKind.CSharp,
                    razorDocumentUri,
                    rangesToMap,
                    LanguageServerMappingBehavior.Inclusive,
                    cancellationToken).ConfigureAwait(false);

                if (mappingResult == null || mappingResult.HostDocumentVersion != documentSnapshot.Version)
                {
                    // Couldn't remap the range or the document changed in the meantime.
                    // Discard the diagnostics from this DiagnosticReport, and report that nothings changed.
                    diagnosticReport.Diagnostics = null;
                    mappedDiagnosticReports.Add(diagnosticReport);
                    continue;
                }

                for (var i = 0; i < filteredDiagnostics.Count(); i++)
                {
                    var diagnostic = filteredDiagnostics.ElementAt(i);
                    var range = mappingResult.Ranges[i];

                    if (range.IsUndefined())
                    {
                        // Couldn't remap the range correctly.
                        // If this isn't an `Error` Severity Diagnostic we can discard it.
                        if (diagnostic.Severity != DiagnosticSeverity.Error)
                        {
                            continue;
                        }

                        // For `Error` Severity diagnostics we still show the diagnostics to
                        // the user, however we set the range to an undefined range to ensure
                        // clicking on the diagnostic doesn't cause errors.
                    }

                    diagnostic.Range = range;
                    mappedDiagnostics.Add(diagnostic);
                }

                // Note; it's possible all ranges were undefined, and thus we
                // have an empty `mappedDiagnostics`. By returning an empty Diagnostics array
                // we're indicating to the platform there are no diagnostics for this document.
                diagnosticReport.Diagnostics = mappedDiagnostics.ToArray();
                mappedDiagnosticReports.Add(diagnosticReport);
            }

            return mappedDiagnosticReports.ToArray();

            static bool CanDiagnosticBeFiltered(Diagnostic d) =>
                DiagnosticsToIgnore.Contains(d.Code) && d.Severity != DiagnosticSeverity.Error;
        }
    }
}
