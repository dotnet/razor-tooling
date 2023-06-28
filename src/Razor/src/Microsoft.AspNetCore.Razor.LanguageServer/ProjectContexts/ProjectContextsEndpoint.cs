﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.CommonLanguageServerProtocol.Framework;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.ProjectContexts;

[LanguageServerEndpoint(VSMethods.GetProjectContextsName)]
internal class ProjectContextsEndpoint : IRazorRequestHandler<VSGetProjectContextsParams, VSProjectContextList>, IRegistrationExtension
{
    private readonly ClientNotifierServiceBase _languageServer;

    public ProjectContextsEndpoint(ClientNotifierServiceBase languageServer)
    {
        _languageServer = languageServer ?? throw new ArgumentNullException(nameof(languageServer));
    }

    public bool MutatesSolutionState => false;

    public RegistrationExtensionResult? GetRegistration(VSInternalClientCapabilities clientCapabilities)
        => new RegistrationExtensionResult("_vs_projectContextProvider", true);

    public TextDocumentIdentifier GetTextDocumentIdentifier(VSGetProjectContextsParams request)
        => new()
        {
            Uri = request.TextDocument.Uri
        };

    public async Task<VSProjectContextList> HandleRequestAsync(VSGetProjectContextsParams request, RazorRequestContext context, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var documentContext = context.GetRequiredDocumentContext();
        var delegatedParams = new DelegatedProjectContextsParams(
            new VersionedTextDocumentIdentifier()
            {
                Uri = documentContext.Uri,
                Version = documentContext.Version
            }
        );

        var response = await _languageServer.SendRequestAsync<DelegatedProjectContextsParams, VSProjectContextList>(
            RazorLanguageServerCustomMessageTargets.RazorProjectContextsEndpoint,
            delegatedParams,
            cancellationToken).ConfigureAwait(false);

        return response ?? new();
    }
}
