﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.DocumentColor;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Cohost;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CommonLanguageServerProtocol.Framework;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.LanguageServerClient.Razor.Extensions;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor.Cohost;

[Shared]
[LanguageServerEndpoint(Methods.TextDocumentDocumentColorName)]
[ExportRazorStatelessLspService(typeof(CohostDocumentColorEndpoint))]
[Export(typeof(ICapabilitiesProvider))]
[method: ImportingConstructor]
internal sealed class CohostDocumentColorEndpoint(
    IDocumentColorService documentColorService,
    IRazorLoggerFactory loggerFactory)
    : AbstractRazorCohostDocumentRequestHandler<DocumentColorParams, ColorInformation[]>, ICapabilitiesProvider
{
    private readonly IDocumentColorService _documentColorService = documentColorService;
    private readonly ILogger _logger = loggerFactory.CreateLogger<CohostDocumentColorEndpoint>();

    protected override bool MutatesSolutionState => false;
    protected override bool RequiresLSPSolution => true;

    protected override RazorTextDocumentIdentifier? GetRazorTextDocumentIdentifier(DocumentColorParams request)
        => request.TextDocument.ToRazorTextDocumentIdentifier();

    public void ApplyCapabilities(VSInternalServerCapabilities serverCapabilities, VSInternalClientCapabilities _)
        => serverCapabilities.EnableDocumentColorProvider();

    protected override Task<ColorInformation[]> HandleRequestAsync(DocumentColorParams request, RazorCohostRequestContext context, CancellationToken cancellationToken)
    {
        // TODO: Create document context from request.TextDocument, by looking at request.Solution instead of our project snapshots
        var documentContext = context.GetRequiredDocumentContext();

        _logger.LogDebug("[Cohost] Received document color request for {requestPath} and got document {documentPath}", request.TextDocument.Uri, documentContext?.FilePath);

        var clientConnection = context.GetClientConnection();
        return _documentColorService.GetColorInformationAsync(clientConnection, request, documentContext, cancellationToken);
    }
}
