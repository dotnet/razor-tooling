﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Cohost.Handlers;
using Microsoft.CodeAnalysis.Razor.AutoInsert;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Protocol.AutoInsert;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Remote.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;

using Response = Microsoft.CodeAnalysis.Razor.Remote.RemoteResponse<Microsoft.CodeAnalysis.Razor.Protocol.AutoInsert.RemoteInsertTextEdit?>;
using RoslynFormattingOptions = Roslyn.LanguageServer.Protocol.FormattingOptions;

namespace Microsoft.CodeAnalysis.Remote.Razor;

internal class RemoteAutoInsertService(in ServiceArgs args)
    : RazorDocumentServiceBase(in args), IRemoteAutoInsertService
{
    internal sealed class Factory : FactoryBase<IRemoteAutoInsertService>
    {
        protected override IRemoteAutoInsertService CreateService(in ServiceArgs args)
            => new RemoteAutoInsertService(in args);
    }

    private readonly IAutoInsertService _autoInsertService
        = args.ExportProvider.GetExportedValue<IAutoInsertService>();
    private readonly IDocumentMappingService _documentMappingService
        = args.ExportProvider.GetExportedValue<IDocumentMappingService>();
    private readonly IFilePathService _filePathService =
        args.ExportProvider.GetExportedValue<IFilePathService>();

    public ValueTask<Response> TryResolveInsertionAsync(
        RazorPinnedSolutionInfoWrapper solutionInfo,
        DocumentId documentId,
        LinePosition linePosition,
        string character,
        bool autoCloseTags,
        CancellationToken cancellationToken)
        => RunServiceAsync(
            solutionInfo,
            documentId,
            context => TryResolveInsertionAsync(
                context,
                linePosition,
                character,
                autoCloseTags,
                cancellationToken),
            cancellationToken);

    private async ValueTask<Response> TryResolveInsertionAsync(
        RemoteDocumentContext remoteDocumentContext,
        LinePosition linePosition,
        string character,
        bool autoCloseTags,
        CancellationToken cancellationToken)
    {
        var sourceText = await remoteDocumentContext.GetSourceTextAsync(cancellationToken).ConfigureAwait(false);
        if (!sourceText.TryGetAbsoluteIndex(linePosition, out var index))
        {
            return Response.NoFurtherHandling;
        }

        var codeDocument = await remoteDocumentContext.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);

        var languageKind = _documentMappingService.GetLanguageKind(codeDocument, index, rightAssociative: true);
        if (languageKind is RazorLanguageKind.Html)
        {
            return Response.CallHtml;
        }
        else if (languageKind is RazorLanguageKind.Razor)
        {
            var insertTextEdit = await _autoInsertService.TryResolveInsertionAsync(
                remoteDocumentContext.Snapshot,
                linePosition.ToPosition(),
                character,
                autoCloseTags,
                cancellationToken);

            return insertTextEdit is { } edit
                ? Response.Results(RemoteInsertTextEdit.FromLspInsertTextEdit(edit))
                : Response.NoFurtherHandling;
        }

        // C# case

        var csharpDocument = codeDocument.GetCSharpDocument();
        if (_documentMappingService.TryMapToGeneratedDocumentPosition(csharpDocument, index, out var mappedPosition, out _))
        {
            var generatedDocument = await remoteDocumentContext.GetGeneratedDocumentAsync(_filePathService, cancellationToken).ConfigureAwait(false);
            // TODO: use correct options rather than default
            var formattingOptions = new RoslynFormattingOptions();
            var autoInsertResponseItem = await OnAutoInsert.GetOnAutoInsertResponseAsync(
                generatedDocument,
                mappedPosition,
                character,
                formattingOptions,
                cancellationToken
            );
            return autoInsertResponseItem is not null
                ? Response.Results(RemoteInsertTextEdit.FromRoslynAutoInsertResponse(autoInsertResponseItem))
                : Response.NoFurtherHandling;
        }

        return Response.NoFurtherHandling;
    }
}
