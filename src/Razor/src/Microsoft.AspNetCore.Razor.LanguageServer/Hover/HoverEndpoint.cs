﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CommonLanguageServerProtocol.Framework;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Hover;

[LanguageServerEndpoint(Methods.TextDocumentHoverName)]
internal sealed class HoverEndpoint : AbstractRazorDelegatingEndpoint<TextDocumentPositionParams, VSInternalHover?>, ICapabilitiesProvider
{
    private readonly IHoverInfoService _hoverInfoService;
    private readonly IRazorDocumentMappingService _documentMappingService;
    private VSInternalClientCapabilities? _clientCapabilities;

    public HoverEndpoint(
        IHoverInfoService hoverInfoService,
        LanguageServerFeatureOptions languageServerFeatureOptions,
        IRazorDocumentMappingService documentMappingService,
        IClientConnection clientConnection,
        IRazorLoggerFactory loggerFactory)
        : base(languageServerFeatureOptions, documentMappingService, clientConnection, loggerFactory.CreateLogger<HoverEndpoint>())
    {
        _hoverInfoService = hoverInfoService ?? throw new ArgumentNullException(nameof(hoverInfoService));
        _documentMappingService = documentMappingService ?? throw new ArgumentNullException(nameof(documentMappingService));
    }

    public void ApplyCapabilities(VSInternalServerCapabilities serverCapabilities, VSInternalClientCapabilities clientCapabilities)
    {
        _clientCapabilities = clientCapabilities;
        serverCapabilities.EnableHoverProvider();
    }

    protected override bool PreferCSharpOverHtmlIfPossible => true;

    protected override IDocumentPositionInfoStrategy DocumentPositionInfoStrategy => PreferAttributeNameDocumentPositionInfoStrategy.Instance;

    protected override string CustomMessageTarget => CustomMessageNames.RazorHoverEndpointName;

    protected override Task<IDelegatedParams?> CreateDelegatedParamsAsync(TextDocumentPositionParams request, RazorRequestContext requestContext, DocumentPositionInfo positionInfo, CancellationToken cancellationToken)
    {
        var documentContext = requestContext.GetRequiredDocumentContext();
        return Task.FromResult<IDelegatedParams?>(new DelegatedPositionParams(
                documentContext.Identifier,
                positionInfo.Position,
                positionInfo.LanguageKind));
    }

    protected override Task<VSInternalHover?> TryHandleAsync(TextDocumentPositionParams request, RazorRequestContext requestContext, DocumentPositionInfo positionInfo, CancellationToken cancellationToken)
        => _hoverInfoService.GetHoverInfoAsync(
            requestContext.GetRequiredDocumentContext(),
            positionInfo,
            request.Position,
            _documentMappingService,
            _clientCapabilities,
            cancellationToken);

    protected override Task<VSInternalHover?> HandleDelegatedResponseAsync(VSInternalHover? response, TextDocumentPositionParams originalRequest, RazorRequestContext requestContext, DocumentPositionInfo positionInfo, CancellationToken cancellationToken)
        => _hoverInfoService.HandleDelegatedResponseAsync(
            response,
            requestContext.GetRequiredDocumentContext(),
            positionInfo,
            _documentMappingService,
            cancellationToken);
}
