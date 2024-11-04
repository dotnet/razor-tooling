﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Cohost;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Cohost.Handlers;
using Microsoft.CodeAnalysis.Razor.CodeActions.Models;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Protocol.CodeActions;
using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.CodeAnalysis.Razor.Workspaces.Telemetry;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

#pragma warning disable RS0030 // Do not use banned APIs
[Shared]
[CohostEndpoint(Methods.TextDocumentCodeActionName)]
[Export(typeof(IDynamicRegistrationProvider))]
[ExportCohostStatelessLspService(typeof(CohostCodeActionsEndpoint))]
[method: ImportingConstructor]
#pragma warning restore RS0030 // Do not use banned APIs
internal class CohostCodeActionsEndpoint(
    IRemoteServiceInvoker remoteServiceInvoker,
    IClientCapabilitiesService clientCapabilitiesService,
    IHtmlDocumentSynchronizer htmlDocumentSynchronizer,
    LSPRequestInvoker requestInvoker,
    ITelemetryReporter telemetryReporter)
    : AbstractRazorCohostDocumentRequestHandler<VSCodeActionParams, SumType<Command, CodeAction>[]?>, IDynamicRegistrationProvider
{
    private readonly IRemoteServiceInvoker _remoteServiceInvoker = remoteServiceInvoker;
    private readonly IClientCapabilitiesService _clientCapabilitiesService = clientCapabilitiesService;
    private readonly IHtmlDocumentSynchronizer _htmlDocumentSynchronizer = htmlDocumentSynchronizer;
    private readonly LSPRequestInvoker _requestInvoker = requestInvoker;
    private readonly ITelemetryReporter _telemetryReporter = telemetryReporter;

    protected override bool MutatesSolutionState => false;

    protected override bool RequiresLSPSolution => true;

    public ImmutableArray<Registration> GetRegistrations(VSInternalClientCapabilities clientCapabilities, DocumentFilter[] filter, RazorCohostRequestContext requestContext)
    {
        if (clientCapabilities.TextDocument?.CodeAction?.DynamicRegistration == true)
        {
            return [new Registration
            {
                Method = Methods.TextDocumentCodeActionName,
                RegisterOptions = new CodeActionRegistrationOptions().EnableCodeActions()
            }];
        }

        return [];
    }

    protected override RazorTextDocumentIdentifier? GetRazorTextDocumentIdentifier(VSCodeActionParams request)
        => request.TextDocument.ToRazorTextDocumentIdentifier();

    protected override Task<SumType<Command, CodeAction>[]?> HandleRequestAsync(VSCodeActionParams request, RazorCohostRequestContext context, CancellationToken cancellationToken)
        => HandleRequestAsync(context.TextDocument.AssumeNotNull(), request, cancellationToken);

    private async Task<SumType<Command, CodeAction>[]?> HandleRequestAsync(TextDocument razorDocument, VSCodeActionParams request, CancellationToken cancellationToken)
    {
        var correlationId = Guid.NewGuid();
        using var _ = _telemetryReporter.TrackLspRequest(Methods.TextDocumentCodeActionName, LanguageServerConstants.RazorLanguageServerName, TelemetryThresholds.CodeActionRazorTelemetryThreshold, correlationId);

        var requestInfo = await _remoteServiceInvoker.TryInvokeAsync<IRemoteCodeActionsService, CodeActionRequestInfo>(
            razorDocument.Project.Solution,
            (service, solutionInfo, cancellationToken) => service.GetCodeActionRequestInfoAsync(solutionInfo, razorDocument.Id, request, cancellationToken),
            cancellationToken).ConfigureAwait(false);

        if (requestInfo is null ||
            requestInfo.LanguageKind == RazorLanguageKind.CSharp && requestInfo.CSharpRequest is null)
        {
            return null;
        }

        var delegatedCodeActions = requestInfo.LanguageKind switch
        {
            RazorLanguageKind.Html => await GetHtmlCodeActionsAsync(razorDocument, request, correlationId, cancellationToken).ConfigureAwait(false),
            RazorLanguageKind.CSharp => await GetCSharpCodeActionsAsync(razorDocument, requestInfo.CSharpRequest.AssumeNotNull(), correlationId, cancellationToken).ConfigureAwait(false),
            _ => []
        };

        return await _remoteServiceInvoker.TryInvokeAsync<IRemoteCodeActionsService, SumType<Command, CodeAction>[]?>(
            razorDocument.Project.Solution,
            (service, solutionInfo, cancellationToken) => service.GetCodeActionsAsync(solutionInfo, razorDocument.Id, request, delegatedCodeActions, cancellationToken),
            cancellationToken).ConfigureAwait(false);
    }

    private async Task<RazorVSInternalCodeAction[]> GetCSharpCodeActionsAsync(TextDocument razorDocument, VSCodeActionParams request, Guid correlationId, CancellationToken cancellationToken)
    {
        var generatedDocumentIds = razorDocument.Project.Solution.GetDocumentIdsWithUri(request.TextDocument.Uri);
        var generatedDocumentId = generatedDocumentIds.FirstOrDefault(d => d.ProjectId == razorDocument.Project.Id);
        if (generatedDocumentId is null)
        {
            return [];
        }

        if (razorDocument.Project.GetDocument(generatedDocumentId) is not { } generatedDocument)
        {
            return [];
        }

        var options = new JsonSerializerOptions();
        foreach (var converter in RazorServiceDescriptorsWrapper.GetLspConverters())
        {
            options.Converters.Add(converter);
        }

        var csharpRequest = JsonSerializer.Deserialize<Roslyn.LanguageServer.Protocol.CodeActionParams>(JsonSerializer.SerializeToDocument(request, options), options).AssumeNotNull();

        using var _ = _telemetryReporter.TrackLspRequest(Methods.TextDocumentCodeActionName, "Razor.ExternalAccess", TelemetryThresholds.CodeActionSubLSPTelemetryThreshold, correlationId);
        var csharpCodeActions = await CodeActions.GetCodeActionsAsync(generatedDocument, csharpRequest, _clientCapabilitiesService.ClientCapabilities.SupportsVisualStudioExtensions, cancellationToken).ConfigureAwait(false);

        return JsonSerializer.Deserialize<RazorVSInternalCodeAction[]>(JsonSerializer.SerializeToDocument(csharpCodeActions, options), options).AssumeNotNull();
    }

    private async Task<RazorVSInternalCodeAction[]> GetHtmlCodeActionsAsync(TextDocument razorDocument, VSCodeActionParams request, Guid correlationId, CancellationToken cancellationToken)
    {
        var htmlDocument = await _htmlDocumentSynchronizer.TryGetSynchronizedHtmlDocumentAsync(razorDocument, cancellationToken).ConfigureAwait(false);
        if (htmlDocument is null)
        {
            return [];
        }

        // We don't want to create a new request, and risk losing data, so we just tweak the Uri and
        // set it back again at the end
        var oldTdi = request.TextDocument;
        try
        {
            request.TextDocument = new VSTextDocumentIdentifier { Uri = htmlDocument.Uri };

            using var _ = _telemetryReporter.TrackLspRequest(Methods.TextDocumentCodeActionName, RazorLSPConstants.HtmlLanguageServerName, TelemetryThresholds.CodeActionSubLSPTelemetryThreshold, correlationId);
            var result = await _requestInvoker.ReinvokeRequestOnServerAsync<VSCodeActionParams, RazorVSInternalCodeAction[]?>(
                htmlDocument.Buffer,
                Methods.TextDocumentCodeActionName,
                RazorLSPConstants.HtmlLanguageServerName,
                request,
                cancellationToken).ConfigureAwait(false);

            if (result?.Response is null)
            {
                return [];
            }

            // WebTools is still using Newtonsoft, so we have to convert to STJ
            foreach (var codeAction in result.Response)
            {
                codeAction.Data = JsonHelpers.TryConvertFromJObject(codeAction.Data);
            }

            return result.Response;
        }
        finally
        {
            request.TextDocument = oldTdi;
        }
    }

    internal TestAccessor GetTestAccessor() => new(this);

    internal readonly struct TestAccessor(CohostCodeActionsEndpoint instance)
    {
        public Task<SumType<Command, CodeAction>[]?> HandleRequestAsync(TextDocument razorDocument, VSCodeActionParams request, CancellationToken cancellationToken)
            => instance.HandleRequestAsync(razorDocument, request, cancellationToken);
    }
}
