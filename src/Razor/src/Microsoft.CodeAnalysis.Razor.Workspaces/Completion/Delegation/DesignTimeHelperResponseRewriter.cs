﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using RazorSyntaxNode = Microsoft.AspNetCore.Razor.Language.Syntax.SyntaxNode;

namespace Microsoft.CodeAnalysis.Razor.Completion.Delegation;

/// <summary>
///  Removes Razor design-time helpers from a C# completion list.
/// </summary>
internal class DesignTimeHelperResponseRewriter : IDelegatedCSharpCompletionResponseRewriter
{
    private static readonly ImmutableHashSet<string> s_designTimeHelpers = new[]
    {
        "__builder",
        "__o",
        "__RazorDirectiveTokenHelpers__",
        "__tagHelperExecutionContext",
        "__tagHelperRunner",
        "__typeHelper",
        "_Imports",
        "BuildRenderTree"
    }.ToImmutableHashSet();

    public async Task<VSInternalCompletionList> RewriteAsync(
        VSInternalCompletionList completionList,
        int hostDocumentIndex,
        DocumentContext hostDocumentContext,
        Position projectedPosition,
        RazorCompletionOptions completionOptions,
        CancellationToken cancellationToken)
    {
        var syntaxTree = await hostDocumentContext.GetSyntaxTreeAsync(cancellationToken).ConfigureAwait(false);
        var owner = syntaxTree.Root.FindInnermostNode(hostDocumentIndex);
        if (owner is null)
        {
            Debug.Fail("Owner should never be null.");
            return completionList;
        }

        var sourceText = await hostDocumentContext.GetSourceTextAsync(cancellationToken).ConfigureAwait(false);

        // We should remove Razor design-time helpers from C#'s completion list. If the current identifier
        // being targeted does not start with a double underscore, we trim out all items starting with "__"
        // from the completion list. If the current identifier does start with a double underscore (e.g. "__ab[||]"),
        // we only trim out common design time helpers from the completion list.

        using var _ = ListPool<CompletionItem>.GetPooledObject(out var filteredItems);

        var items = completionList.Items;
        filteredItems.SetCapacityIfLarger(items.Length);

        // If the current identifier doesn't start with "__", we remove common design-time helpers *and*
        // any item starting with "__" from the completion list. Otherwise, we only remove the common
        // design-time helpers.
        var removeAllDoubleUnderscoreItems = !StartsWithDoubleUnderscore(owner, sourceText);

        foreach (var item in items)
        {
            if (s_designTimeHelpers.Contains(item.Label) || (removeAllDoubleUnderscoreItems && item.Label.StartsWith("__")))
            {
                continue;
            }

            filteredItems.Add(item);
        }

        // Avoid allocating array if nothing was filtered.
        if (items.Length != filteredItems.Count)
        {
            completionList.Items = filteredItems.ToArray();
        }

        return completionList;
    }

    private static bool StartsWithDoubleUnderscore(RazorSyntaxNode owner, SourceText sourceText)
    {
        var span = owner.Span;
        if (span.Length < 2)
        {
            return false;
        }

        var start = span.Start;
        return sourceText[start] == '_' || sourceText[start + 1] == '_';
    }
}
