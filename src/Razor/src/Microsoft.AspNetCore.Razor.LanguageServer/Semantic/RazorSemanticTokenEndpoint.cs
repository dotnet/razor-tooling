﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.AspNetCore.Razor.LanguageServer.Semantic.Capabilities;
using Microsoft.AspNetCore.Razor.LanguageServer.Semantic.Interfaces;
using Microsoft.AspNetCore.Razor.LanguageServer.Semantic.Models;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.Extensions.Logging;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Semantic
{
    internal class RazorSemanticTokenEndpoint : ISemanticTokenHandler, ISemanticTokenRangeHandler, ISemanticTokenEditHandler, IRegistrationExtension
    {
        private const string SemanticCapability = "semanticTokensProvider";

        private SemanticTokensCapability _tokenCapability;
        private SemanticTokensRangeCapability _tokenRangeCapability;
        private SemanticTokensEditCapability _tokenEditCapability;
        private readonly ILogger _logger;

        private readonly ForegroundDispatcher _foregroundDispatcher;
        private readonly DocumentResolver _documentResolver;
        private readonly RazorSemanticTokenInfoService _semanticTokenInfoService;

        public RazorSemanticTokenEndpoint(
            ForegroundDispatcher foregroundDispatcher,
            DocumentResolver documentResolver,
            RazorSemanticTokenInfoService semanticTokenInfoService,
            ILoggerFactory loggerFactory)
        {
            if (foregroundDispatcher is null)
            {
                throw new ArgumentNullException(nameof(foregroundDispatcher));
            }

            if (documentResolver is null)
            {
                throw new ArgumentNullException(nameof(documentResolver));
            }

            if (semanticTokenInfoService is null)
            {
                throw new ArgumentNullException(nameof(semanticTokenInfoService));
            }

            if (loggerFactory is null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            _foregroundDispatcher = foregroundDispatcher;
            _documentResolver = documentResolver;
            _semanticTokenInfoService = semanticTokenInfoService;
            _logger = loggerFactory.CreateLogger<RazorSemanticTokenEndpoint>();
        }

        public async Task<SemanticTokens> Handle(SemanticTokenParams request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return await Handle(request.TextDocument.Uri.AbsolutePath, cancellationToken, range: null);
        }

        public async Task<SemanticTokens> Handle(SemanticTokensRangeParams request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return await Handle(request.TextDocument.Uri.AbsolutePath, cancellationToken, request.Range);
        }

        public async Task<SemanticTokensOrSemanticTokensEdits?> Handle(SemanticTokensEditParams request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return await Handle(request.TextDocument.Uri.AbsolutePath, request.PreviousResultId, cancellationToken);
        }

        public void SetCapability(SemanticTokensCapability capability)
        {
            _tokenCapability = capability;
        }

        public void SetCapability(SemanticTokensEditCapability capability)
        {
            _tokenEditCapability = capability;
        }

        public void SetCapability(SemanticTokensRangeCapability capability)
        {
            _tokenRangeCapability = capability;
        }

        public RegistrationExtensionResult GetRegistration()
        {
            var semanticTokensOptions = new SemanticTokensOptions
            {
                DocumentProvider = new SemanticTokensDocumentProviderOptions
                {
                    Edits = true,
                },
                Legend = new SemanticTokensLegend
                {
                    TokenModifiers = new Container<string>(SemanticTokenLegend.Instance.TokenModifiers),
                    TokenTypes = new Container<string>(SemanticTokenLegend.Instance.TokenTypes),
                },
                RangeProvider = true,
            };

            return new RegistrationExtensionResult(SemanticCapability, semanticTokensOptions);
        }

        private async Task<RazorCodeDocument> GetCodeDocument(string absolutePath, CancellationToken cancellationToken)
        {
            var document = await Task.Factory.StartNew(() =>
            {
                _documentResolver.TryResolveDocument(absolutePath, out var documentSnapshot);

                return documentSnapshot;
            }, cancellationToken, TaskCreationOptions.None, _foregroundDispatcher.ForegroundScheduler);

            if (document is null)
            {
                return null;
            }

            var codeDocument = await document.GetGeneratedOutputAsync();

            return codeDocument;
        }

        private async Task<SemanticTokens> Handle(string absolutePath, CancellationToken cancellationToken, Range range = null)
        {
            var codeDocument = await GetCodeDocument(absolutePath, cancellationToken);

            if (codeDocument.IsUnsupported())
            {
                return null;
            }

            var tokens = _semanticTokenInfoService.GetSemanticTokens(codeDocument, range);

            return tokens;
        }

        private async Task<SemanticTokensOrSemanticTokensEdits?> Handle(string absolutePath, string previousId, CancellationToken cancellationToken)
        {
            var codeDocument = await GetCodeDocument(absolutePath, cancellationToken);

            if (codeDocument.IsUnsupported())
            {
                return null;
            }

            var edits = _semanticTokenInfoService.GetSemanticTokenEdits(codeDocument, previousId);

            return edits;
        }
    }
}
