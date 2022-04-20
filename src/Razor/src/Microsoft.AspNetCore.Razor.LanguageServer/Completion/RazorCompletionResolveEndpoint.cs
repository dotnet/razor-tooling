﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Tooltip;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.Completion;
using Microsoft.Extensions.Logging;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Completion
{
    internal class RazorCompletionResolveEndpoint : ICompletionResolveHandler
    {
        private readonly ILogger _logger;
        private readonly LSPTagHelperTooltipFactory _lspTagHelperTooltipFactory;
        private readonly VSLSPTagHelperTooltipFactory _vsLspTagHelperTooltipFactory;
        private readonly CompletionListCache _completionListCache;
        private PlatformAgnosticCompletionCapability? _completionCapability;
        private PlatformAgnosticClientCapabilities? _clientCapabilities;

        // Guid is magically generated and doesn't mean anything. O# magic.
        public Guid Id => new("011c77cc-f90e-4f2e-b32c-dafc6587ccd6");

        public RazorCompletionResolveEndpoint(
            LSPTagHelperTooltipFactory lspTagHelperTooltipFactory!!,
            VSLSPTagHelperTooltipFactory vsLspTagHelperTooltipFactory!!,
            CompletionListCache completionListCache,
            ILoggerFactory loggerFactory!!)
        {
            _lspTagHelperTooltipFactory = lspTagHelperTooltipFactory;
            _vsLspTagHelperTooltipFactory = vsLspTagHelperTooltipFactory;
            _logger = loggerFactory.CreateLogger<RazorCompletionEndpoint>();
            _completionListCache = completionListCache;
        }

        public void SetCapability(CompletionCapability capability, ClientCapabilities clientCapabilities)
        {
            _completionCapability = (PlatformAgnosticCompletionCapability)capability;
            _clientCapabilities = (PlatformAgnosticClientCapabilities)clientCapabilities;
        }

        public Task<CompletionItem> Handle(CompletionItem completionItem, CancellationToken cancellationToken)
        {
            if (!completionItem.TryGetCompletionListResultId(out var resultId))
            {
                // Couldn't resolve.
                return Task.FromResult(completionItem);
            }

            if (!_completionListCache.TryGet(resultId.Value, out var cachedCompletionItems))
            {
                return Task.FromResult(completionItem);
            }

            var labelQuery = completionItem.Label;
            var associatedRazorCompletion = cachedCompletionItems.FirstOrDefault(completion => string.Equals(labelQuery, completion.DisplayText, StringComparison.Ordinal));
            if (associatedRazorCompletion is null)
            {
                _logger.LogError("Could not find an associated razor completion item. This should never happen since we were able to look up the cached completion list.");
                Debug.Fail("Could not find an associated razor completion item. This should never happen since we were able to look up the cached completion list.");
                return Task.FromResult(completionItem);
            }

            // If the client is VS, also fill in the Description property.
            var useDescriptionProperty = _clientCapabilities?.SupportsVisualStudioExtensions ?? false;

            MarkupContent? tagHelperMarkupTooltip = null;
            VSClassifiedTextElement? tagHelperClassifiedTextTooltip = null;

            switch (associatedRazorCompletion.Kind)
            {
                case RazorCompletionItemKind.Directive:
                    {
                        var descriptionInfo = associatedRazorCompletion.GetDirectiveCompletionDescription();
                        if (descriptionInfo is not null)
                        {
                            completionItem = completionItem with { Documentation = descriptionInfo.Description };
                        }

                        break;
                    }
                case RazorCompletionItemKind.MarkupTransition:
                    {
                        var descriptionInfo = associatedRazorCompletion.GetMarkupTransitionCompletionDescription();
                        if (descriptionInfo is not null)
                        {
                            completionItem = completionItem with { Documentation = descriptionInfo.Description };
                        }

                        break;
                    }
                case RazorCompletionItemKind.DirectiveAttribute:
                case RazorCompletionItemKind.DirectiveAttributeParameter:
                case RazorCompletionItemKind.TagHelperAttribute:
                    {
                        var descriptionInfo = associatedRazorCompletion.GetAttributeCompletionDescription();
                        if (useDescriptionProperty)
                        {
                            _vsLspTagHelperTooltipFactory.TryCreateTooltip(descriptionInfo, out tagHelperClassifiedTextTooltip);
                        }
                        else
                        {
                            _lspTagHelperTooltipFactory.TryCreateTooltip(descriptionInfo, out tagHelperMarkupTooltip);
                        }

                        break;
                    }
                case RazorCompletionItemKind.TagHelperElement:
                    {
                        var descriptionInfo = associatedRazorCompletion.GetTagHelperElementDescriptionInfo();
                        if (useDescriptionProperty)
                        {
                            _vsLspTagHelperTooltipFactory.TryCreateTooltip(descriptionInfo, out tagHelperClassifiedTextTooltip);
                        }
                        else
                        {
                            _lspTagHelperTooltipFactory.TryCreateTooltip(descriptionInfo, out tagHelperMarkupTooltip);
                        }

                        break;
                    }
            }

            if (tagHelperMarkupTooltip != null)
            {
                var documentation = new StringOrMarkupContent(tagHelperMarkupTooltip);
                completionItem = completionItem with { Documentation = documentation };
            }

            // We might strip out the commitcharacters for speed, bring them back
            var container = associatedRazorCompletion.CommitCharacters != null ? new Container<string>(associatedRazorCompletion.CommitCharacters) : null;
            completionItem = completionItem with { CommitCharacters = container };
            var vsCompletionItem = completionItem.ToVSCompletionItem(_completionCapability?.VSCompletionList);

            if (tagHelperClassifiedTextTooltip != null)
            {
                vsCompletionItem.Description = tagHelperClassifiedTextTooltip;
                return Task.FromResult<CompletionItem>(vsCompletionItem);
            }

            return Task.FromResult<CompletionItem>(vsCompletionItem);
        }
    }
}
