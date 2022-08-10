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
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Text;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Hover
{
    internal class RazorHoverEndpoint : AbstractRazorDelegatingEndpoint<VSHoverParamsBridge, VSInternalHover, DelegatedHoverParams>, IVSHoverEndpoint
    {
        private readonly DocumentContextFactory _documentContextFactory;
        private readonly RazorHoverInfoService _hoverInfoService;
        private readonly RazorDocumentMappingService _documentMappingService;
        private VSInternalClientCapabilities? _clientCapabilities;

        public RazorHoverEndpoint(
            DocumentContextFactory documentContextFactory,
            RazorHoverInfoService hoverInfoService,
            LanguageServerFeatureOptions languageServerFeatureOptions,
            RazorDocumentMappingService documentMappingService,
            ClientNotifierServiceBase languageServer,
            ILoggerFactory loggerFactory)
            : base(documentContextFactory, languageServerFeatureOptions, languageServer, loggerFactory.CreateLogger<RazorHoverEndpoint>())
        {
            _documentContextFactory = documentContextFactory ?? throw new ArgumentNullException(nameof(documentContextFactory));
            _hoverInfoService = hoverInfoService ?? throw new ArgumentNullException(nameof(hoverInfoService));
            _documentMappingService = documentMappingService ?? throw new ArgumentNullException(nameof(documentMappingService));
        }

        public RegistrationExtensionResult? GetRegistration(VSInternalClientCapabilities clientCapabilities)
        {
            const string AssociatedServerCapability = "hoverProvider";
            _clientCapabilities = clientCapabilities;

            var registrationOptions = new HoverOptions()
            {
                WorkDoneProgress = false,
            };

            return new RegistrationExtensionResult(AssociatedServerCapability, registrationOptions);
        }

        /// <inheritdoc/>
        protected override string CustomMessageTarget => RazorLanguageServerCustomMessageTargets.RazorHoverEndpointName;

        /// <inheritdoc/>
        protected override async Task<DelegatedHoverParams> CreateDelegatedParamsAsync(VSHoverParamsBridge request, DocumentContext documentContext, CancellationToken cancellationToken)
        {
            var sourceText = await documentContext.GetSourceTextAsync(cancellationToken).ConfigureAwait(false);
            var absoluteIndex = request.Position.GetRequiredAbsoluteIndex(sourceText, Logger);
            var projection = await _documentMappingService.GetProjectionAsync(documentContext, absoluteIndex, cancellationToken).ConfigureAwait(false);

            return new DelegatedHoverParams(
                        documentContext.Identifier,
                        projection.Position,
                        projection.LanguageKind);
        }

        /// <inheritdoc/>
        protected override async Task<VSInternalHover?> TryHandleAsync(VSHoverParamsBridge request, DocumentContext documentContext, CancellationToken cancellationToken)
        {
            var projection = await _documentMappingService.TryGetProjectionAsync(documentContext, request.Position, Logger, cancellationToken).ConfigureAwait(false);
            if (projection is null)
            {
                return null;
            }

            // HTML can still sometimes be handled by razor. For example hovering over
            // a component tag like <Counter /> will still be in an html context
            if (projection.LanguageKind == RazorLanguageKind.CSharp)
            {
                return null;
            }

            var sourceText = await documentContext.GetSourceTextAsync(cancellationToken).ConfigureAwait(false);
            var linePosition = new LinePosition(request.Position.Line, request.Position.Character);
            var hostDocumentIndex = sourceText.Lines.GetPosition(linePosition);
            var location = new SourceLocation(hostDocumentIndex, request.Position.Line, request.Position.Character);
            var codeDocument = await documentContext.GetCodeDocumentAsync(cancellationToken);

            return _hoverInfoService.GetHoverInfo(codeDocument, location, _clientCapabilities!);
        }

        /// <inheritdoc/>
        protected override async Task<VSInternalHover> HandleDelegatedResponseAsync(VSInternalHover response, DocumentContext documentContext, CancellationToken cancellationToken)
        {
            if (response.Range is null)
            {
                return response;
            }

            var codeDocument = await documentContext.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);

            if (_documentMappingService.TryMapFromProjectedDocumentRange(codeDocument, response.Range, out var projectedRange))
            {
                response.Range = projectedRange;
            }

            return response;
        }
    }
}
