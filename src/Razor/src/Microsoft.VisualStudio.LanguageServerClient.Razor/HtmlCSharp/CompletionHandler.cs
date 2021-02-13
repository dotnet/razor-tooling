﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Composition;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.LanguageServerClient.Razor.Logging;
using Microsoft.VisualStudio.Text.Operations;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor.HtmlCSharp
{
    [Shared]
    [ExportLspMethod(Methods.TextDocumentCompletionName)]
    internal class CompletionHandler : IRequestHandler<CompletionParams, SumType<CompletionItem[], CompletionList>?>
    {
        private static readonly IReadOnlyList<string> RazorTriggerCharacters = new[] { "@" };
        private static readonly IReadOnlyList<string> CSharpTriggerCharacters = new[] { " ", "(", "=", "#", ".", "<", "[", "{", "\"", "/", ":", ">", "~" };
        private static readonly IReadOnlyList<string> HtmlTriggerCharacters = new[] { ":", "@", "#", ".", "!", "*", ",", "(", "[", "=", "-", "<", "&", "\\", "/", "'", "\"", "=", ":", " ", "`" };

        public static readonly IReadOnlyList<string> AllTriggerCharacters = new HashSet<string>(
            CSharpTriggerCharacters
                .Concat(HtmlTriggerCharacters)
                .Concat(RazorTriggerCharacters))
            .ToArray();

        private static readonly IReadOnlyCollection<string> Keywords = new string[] {
            "for", "foreach", "while", "switch", "lock",
            "case", "if", "try", "do", "using"
        };

        private static readonly IReadOnlyCollection<string> DesignTimeHelpers = new string[]
        {
            "__builder",
            "__o",
            "__RazorDirectiveTokenHelpers__",
            "__tagHelperExecutionContext",
            "__tagHelperRunner",
            "__typeHelper",
            "_Imports",
            "BuildRenderTree"
        };

        private static readonly IReadOnlyCollection<CompletionItem> KeywordCompletionItems = GenerateCompletionItems(Keywords);
        private static readonly IReadOnlyCollection<CompletionItem> DesignTimeHelpersCompletionItems = GenerateCompletionItems(DesignTimeHelpers);

        private readonly JoinableTaskFactory _joinableTaskFactory;
        private readonly LSPRequestInvoker _requestInvoker;
        private readonly LSPDocumentManager _documentManager;
        private readonly LSPProjectionProvider _projectionProvider;
        private readonly ITextStructureNavigatorSelectorService _textStructureNavigator;
        private readonly CompletionRequestContextCache _completionRequestContextCache;
        private readonly ILogger _logger;

        [ImportingConstructor]
        public CompletionHandler(
            JoinableTaskContext joinableTaskContext,
            LSPRequestInvoker requestInvoker,
            LSPDocumentManager documentManager,
            LSPProjectionProvider projectionProvider,
            ITextStructureNavigatorSelectorService textStructureNavigator,
            CompletionRequestContextCache completionRequestContextCache,
            HTMLCSharpLanguageServerLogHubLoggerProvider loggerProvider)
        {
            if (joinableTaskContext is null)
            {
                throw new ArgumentNullException(nameof(joinableTaskContext));
            }

            if (requestInvoker is null)
            {
                throw new ArgumentNullException(nameof(requestInvoker));
            }

            if (documentManager is null)
            {
                throw new ArgumentNullException(nameof(documentManager));
            }

            if (projectionProvider is null)
            {
                throw new ArgumentNullException(nameof(projectionProvider));
            }

            if (textStructureNavigator is null)
            {
                throw new ArgumentNullException(nameof(textStructureNavigator));
            }

            if (completionRequestContextCache is null)
            {
                throw new ArgumentNullException(nameof(completionRequestContextCache));
            }

            if (loggerProvider is null)
            {
                throw new ArgumentNullException(nameof(loggerProvider));
            }

            _joinableTaskFactory = joinableTaskContext.Factory;
            _requestInvoker = requestInvoker;
            _documentManager = documentManager;
            _projectionProvider = projectionProvider;
            _textStructureNavigator = textStructureNavigator;
            _completionRequestContextCache = completionRequestContextCache;

            _logger = loggerProvider.CreateLogger(nameof(CompletionHandler));
        }

        public async Task<SumType<CompletionItem[], CompletionList>?> HandleRequestAsync(CompletionParams request, ClientCapabilities clientCapabilities, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (clientCapabilities is null)
            {
                throw new ArgumentNullException(nameof(clientCapabilities));
            }

            _logger.LogInformation($"Starting request for {request.TextDocument.Uri}.");

            if (!_documentManager.TryGetDocument(request.TextDocument.Uri, out var documentSnapshot))
            {
                _logger.LogWarning($"Failed to find document {request.TextDocument.Uri}.");
                return null;
            }

            var projectionResult = await _projectionProvider.GetProjectionAsync(documentSnapshot, request.Position, cancellationToken).ConfigureAwait(false);
            if (projectionResult == null)
            {
                _logger.LogInformation($"Failed to find projection for {request.Position} in {request.TextDocument.Uri}.");
                return null;
            }

            var serverKind = projectionResult.LanguageKind == RazorLanguageKind.CSharp ? LanguageServerKind.CSharp : LanguageServerKind.Html;

            var (succeeded, result) = await TryGetProvisionalCompletionsAsync(request, documentSnapshot, projectionResult, cancellationToken).ConfigureAwait(false);
            if (succeeded)
            {
                // This means the user has just typed a dot after some identifier such as (cursor is pipe): "DateTime.| "
                // In this case Razor interprets after the dot as Html and before it as C#.
                // We use this criteria to provide a better completion experience for what we call provisional changes.
            }
            else if (!TriggerAppliesToProjection(request.Context, projectionResult.LanguageKind))
            {
                _logger.LogInformation("Trigger does not apply to projection.");
                return null;
            }
            else
            {
                // This is a valid non-provisional completion request.
                _logger.LogInformation("Searching for non-provisional completions, rewriting context.");

                var completionContext = RewriteContext(request.Context, projectionResult.LanguageKind);

                var completionParams = new CompletionParams()
                {
                    Context = completionContext,
                    Position = projectionResult.Position,
                    TextDocument = new TextDocumentIdentifier()
                    {
                        Uri = projectionResult.Uri
                    }
                };

                _logger.LogInformation($"Requesting non-provisional completions for {projectionResult.Uri}.");

                result = await _requestInvoker.ReinvokeRequestOnServerAsync<CompletionParams, SumType<CompletionItem[], CompletionList>?>(
                    Methods.TextDocumentCompletionName,
                    serverKind.ToContentType(),
                    completionParams,
                    cancellationToken).ConfigureAwait(false);

                _logger.LogInformation("Returning non-provisional completion");
            }

            if (TryConvertToCompletionList(result, out var completionList))
            {
                if (serverKind == LanguageServerKind.CSharp)
                {
                    MassageCSharpCompletions(request, documentSnapshot, completionList);
                }

                var requestContext = new CompletionRequestContext(documentSnapshot.Uri, projectionResult.Uri, serverKind);
                var resultId = _completionRequestContextCache.Set(requestContext);
                SetResolveData(resultId, completionList);
            }

            _logger.LogInformation("Returning final completion list.");
            return completionList;

            static bool TryConvertToCompletionList(SumType<CompletionItem[], CompletionList>? original, out CompletionList completionList)
            {
                if (!original.HasValue)
                {
                    completionList = null;
                    return false;
                }

                if (original.Value.TryGetFirst(out var completionItems))
                {
                    completionList = new CompletionList()
                    {
                        Items = completionItems,
                        IsIncomplete = false
                    };
                }
                else if (!original.Value.TryGetSecond(out completionList))
                {
                    Debug.Fail("This should be impossible, the completion result should be either a completion list or a set of completion items.");
                    completionList = null;
                    return false;
                }

                return true;
            }
        }

        private void MassageCSharpCompletions(
            CompletionParams request,
            LSPDocumentSnapshot documentSnapshot,
            CompletionList completionList)
        {
            var wordExtent = documentSnapshot.Snapshot.GetWordExtent(request.Position.Line, request.Position.Character, _textStructureNavigator);
            if (IsSimpleImplicitExpression(request, documentSnapshot, wordExtent))
            {
                DoNotPreselect(completionList);
                IncludeCSharpKeywords(completionList);
            }

            RemoveDesignTimeItems(documentSnapshot, wordExtent, completionList);
        }

        private static IReadOnlyCollection<CompletionItem> GenerateCompletionItems(IReadOnlyCollection<string> completionItems)
            => completionItems.Select(item => new CompletionItem { Label = item }).ToArray();

        private static bool IsSimpleImplicitExpression(CompletionParams request, LSPDocumentSnapshot documentSnapshot, TextExtent? wordExtent)
        {
            if (string.Equals(request.Context.TriggerCharacter, "@", StringComparison.Ordinal))
            {
                // Completion was triggered with `@` this is always a simple implicit expression
                return true;
            }

            if (wordExtent == null)
            {
                return false;
            }

            if (!wordExtent.Value.IsSignificant)
            {
                // Word is only whitespace, definitely not an implicit expresison
                return false;
            }

            // We need to look at the item before the word because `@` at the beginning of a word is not encapsulated in that word.
            var leadingWordCharacterIndex = Math.Max(0, wordExtent.Value.Span.Start.Position - 1);
            var leadingWordCharacter = documentSnapshot.Snapshot[leadingWordCharacterIndex];
            if (leadingWordCharacter == '@')
            {
                // This means that completion was requested at something like @for|e and the word was "fore" with the previous character index being "@"
                return true;
            }

            return false;
        }

        // We should remove Razor design time helpers from C#'s completion list. If the current identifier being targeted does not start with a double
        // underscore, we trim out all items starting with "__" from the completion list. If the current identifier does start with a double underscore
        // (e.g. "__ab[||]"), we only trim out common design time helpers from the completion list.
        private static void RemoveDesignTimeItems(
            LSPDocumentSnapshot documentSnapshot,
            TextExtent? wordExtent,
            CompletionList completionList)
        {
            var filteredItems = completionList.Items.Except(DesignTimeHelpersCompletionItems, CompletionItemComparer.Instance).ToArray();

            // If the current identifier starts with "__", only trim out common design time helpers from the list.
            // In all other cases, trim out both common design time helpers and all completion items starting with "__".
            if (RemoveAllDesignTimeItems(documentSnapshot, wordExtent))
            {
                filteredItems = filteredItems.Where(item => item.Label != null && !item.Label.StartsWith("__", StringComparison.Ordinal)).ToArray();
            }

            completionList.Items = filteredItems;

            static bool RemoveAllDesignTimeItems(LSPDocumentSnapshot documentSnapshot, TextExtent? wordExtent)
            {
                if (!wordExtent.HasValue)
                {
                    return true;
                }

                var wordSpan = wordExtent.Value.Span;
                if (wordSpan.Length < 2)
                {
                    return true;
                }

                var snapshot = documentSnapshot.Snapshot;
                var startIndex = wordSpan.Start.Position;

                if (snapshot[startIndex] == '_' && snapshot[startIndex + 1] == '_')
                {
                    return false;
                }

                return true;
            }
        }

        private static CompletionContext RewriteContext(CompletionContext context, RazorLanguageKind languageKind)
        {
            if (context.TriggerKind != CompletionTriggerKind.TriggerCharacter)
            {
                // Non-triggered based completion, the existing context is valid;
                return context;
            }

            if (languageKind == RazorLanguageKind.CSharp && CSharpTriggerCharacters.Contains(context.TriggerCharacter))
            {
                // C# trigger character for C# content
                return context;
            }

            if (languageKind == RazorLanguageKind.Html && HtmlTriggerCharacters.Contains(context.TriggerCharacter))
            {
                // HTML trigger character for HTML content
                return context;
            }

            // Trigger character not associated with the current langauge. Transform the context into an invoked context.
            var rewrittenContext = new VSCompletionContext()
            {
                TriggerKind = CompletionTriggerKind.Invoked,
            };

            var invokeKind = (context as VSCompletionContext)?.InvokeKind;
            if (invokeKind.HasValue)
            {
                rewrittenContext.InvokeKind = invokeKind.Value;
            }

            return rewrittenContext;
        }

        internal async Task<(bool, SumType<CompletionItem[], CompletionList>?)> TryGetProvisionalCompletionsAsync(
            CompletionParams request,
            LSPDocumentSnapshot documentSnapshot,
            ProjectionResult projection,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Searching for provisional completions.");

            SumType<CompletionItem[], CompletionList>? result = null;
            if (projection.LanguageKind != RazorLanguageKind.Html ||
                request.Context.TriggerKind != CompletionTriggerKind.TriggerCharacter ||
                request.Context.TriggerCharacter != ".")
            {
                _logger.LogInformation("Invalid provisional completion context.");
                return (false, result);
            }

            if (projection.Position.Character == 0)
            {
                // We're at the start of line. Can't have provisional completions here.
                _logger.LogInformation("Start of line, invalid completion location.");
                return (false, result);
            }

            var previousCharacterPosition = new Position(projection.Position.Line, projection.Position.Character - 1);
            var previousCharacterProjection = await _projectionProvider.GetProjectionAsync(
                documentSnapshot,
                previousCharacterPosition,
                cancellationToken).ConfigureAwait(false);
            if (previousCharacterProjection == null ||
                previousCharacterProjection.LanguageKind != RazorLanguageKind.CSharp ||
                previousCharacterProjection.HostDocumentVersion is null)
            {
                _logger.LogInformation($"Failed to find previous char projection in {previousCharacterProjection?.LanguageKind:G} at version {previousCharacterProjection?.HostDocumentVersion}.");
                return (false, result);
            }

            if (_documentManager is not TrackingLSPDocumentManager trackingDocumentManager)
            {
                _logger.LogInformation("Not a tracking document manager.");
                return (false, result);
            }

            // Edit the CSharp projected document to contain a '.'. This allows C# completion to provide valid
            // completion items for moments when a user has typed a '.' that's typically interpreted as Html.
            var addProvisionalDot = new VisualStudioTextChange(previousCharacterProjection.PositionIndex, 0, ".");

            await _joinableTaskFactory.SwitchToMainThreadAsync();

            trackingDocumentManager.UpdateVirtualDocument<CSharpVirtualDocument>(
                documentSnapshot.Uri,
                new[] { addProvisionalDot },
                previousCharacterProjection.HostDocumentVersion.Value);

            var provisionalCompletionParams = new CompletionParams()
            {
                Context = request.Context,
                Position = new Position(
                    previousCharacterProjection.Position.Line,
                    previousCharacterProjection.Position.Character + 1),
                TextDocument = new TextDocumentIdentifier()
                {
                    Uri = previousCharacterProjection.Uri
                }
            };

            _logger.LogInformation($"Requesting provisional completion for {previousCharacterProjection.Uri}.");

            result = await _requestInvoker.ReinvokeRequestOnServerAsync<CompletionParams, SumType<CompletionItem[], CompletionList>?>(
                Methods.TextDocumentCompletionName,
                RazorLSPConstants.CSharpContentTypeName,
                provisionalCompletionParams,
                cancellationToken).ConfigureAwait(true);

            // We have now obtained the necessary completion items. We no longer need the provisional change. Revert.
            var removeProvisionalDot = new VisualStudioTextChange(previousCharacterProjection.PositionIndex, 1, string.Empty);

            trackingDocumentManager.UpdateVirtualDocument<CSharpVirtualDocument>(
                documentSnapshot.Uri,
                new[] { removeProvisionalDot },
                previousCharacterProjection.HostDocumentVersion.Value);

            _logger.LogInformation("Returning provisional completion");
            return (true, result);
        }

        // In cases like "@{" preselection can lead to unexpected behavior, so let's exclude it.
        private static void DoNotPreselect(CompletionList completionList)
        {
            foreach (var item in completionList.Items)
            {
                item.Preselect = false;
            }
        }

        // C# keywords were previously provided by snippets, but as of now C# LSP doesn't provide snippets.
        // We're providing these for now to improve the user experience (not having to ESC out of completions to finish),
        // but once C# starts providing them their completion will be offered instead, at which point we should be able to remove this step.
        private static void IncludeCSharpKeywords(CompletionList completionList)
        {
            var newList = completionList.Items.Union(KeywordCompletionItems, CompletionItemComparer.Instance);
            completionList.Items = newList.ToArray();
        }

        internal static void SetResolveData(long resultId, CompletionList completionList)
        {
            for (var i = 0; i < completionList.Items.Length; i++)
            {
                var item = completionList.Items[i];
                var data = new CompletionResolveData()
                {
                    ResultId = resultId,
                    OriginalData = item.Data,
                };
                item.Data = data;
            }
        }

        // Internal for testing
        internal static bool TriggerAppliesToProjection(CompletionContext context, RazorLanguageKind languageKind)
        {
            if (languageKind == RazorLanguageKind.Razor)
            {
                // We don't handle any type of triggers in Razor pieces of the document
                return false;
            }

            if (context.TriggerKind != CompletionTriggerKind.TriggerCharacter)
            {
                // Not a trigger character completion, allow it.
                return true;
            }

            if (!AllTriggerCharacters.Contains(context.TriggerCharacter))
            {
                // This is an auto-invoked completion from the VS LSP platform. Completions are automatically invoked upon typing identifiers
                // and are represented as CompletionTriggerKind.TriggerCharacter and have a trigger character that we have not registered for.
                return true;
            }

            if (IsApplicableTriggerCharacter(context.TriggerCharacter, languageKind))
            {
                // Trigger character is associated with the langauge at the current cursor position
                return true;
            }

            // We were triggered but the trigger character doesn't make sense for the current cursor position. Bail.
            return false;
        }

        private static bool IsApplicableTriggerCharacter(string triggerCharacter, RazorLanguageKind languageKind)
        {
            if (RazorTriggerCharacters.Contains(triggerCharacter))
            {
                // Razor trigger characters always transition into either C# or HTML, always note as "applicable".
                return true;
            }
            else if (languageKind == RazorLanguageKind.CSharp)
            {
                return CSharpTriggerCharacters.Contains(triggerCharacter);
            }
            else if (languageKind == RazorLanguageKind.Html)
            {
                return HtmlTriggerCharacters.Contains(triggerCharacter);
            }

            // Unknown trigger character.
            return false;
        }

        private class CompletionItemComparer : IEqualityComparer<CompletionItem>
        {
            public static CompletionItemComparer Instance = new CompletionItemComparer();

            public bool Equals(CompletionItem x, CompletionItem y)
            {
                if (x is null && y is null)
                {
                    return true;
                }
                else if (x is null || y is null)
                {
                    return false;
                }

                return x.Label.Equals(y.Label, StringComparison.Ordinal);
            }

            public int GetHashCode(CompletionItem obj) => obj?.Label?.GetHashCode() ?? 0;
        }
    }
}
