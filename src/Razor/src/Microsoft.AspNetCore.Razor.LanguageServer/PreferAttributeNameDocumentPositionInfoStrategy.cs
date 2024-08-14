﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

/// <summary>
/// A projection strategy that, when given a position that occurs anywhere in an attribute name, will return the projection
/// for the position at the start of the attribute name, ignoring any prefix or suffix. eg given any location within the
/// attribute "@bind-Value:after", it will return the projection at the point of the word "Value" therein.
/// </summary>
internal class PreferAttributeNameDocumentPositionInfoStrategy : IDocumentPositionInfoStrategy
{
    public static IDocumentPositionInfoStrategy Instance { get; } = new PreferAttributeNameDocumentPositionInfoStrategy();

    public async Task<DocumentPositionInfo?> TryGetPositionInfoAsync(
        IDocumentMappingService documentMappingService,
        DocumentContext documentContext,
        Position position,
        CancellationToken cancellationToken)
    {
        var codeDocument = await documentContext.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);
        var sourceText = await documentContext.GetSourceTextAsync(cancellationToken).ConfigureAwait(false);
        if (sourceText.TryGetAbsoluteIndex(position, out var absoluteIndex))
        {
            // First, lets see if we should adjust the location to get a better result from C#. For example given <Component @bi|nd-Value="Pants" />
            // where | is the cursor, we would be unable to map that location to C#. If we pretend the caret was 3 characters to the right though,
            // in the actual component property name, then the C# server would give us a result, so we fake it.
            if (RazorSyntaxFacts.TryGetAttributeNameAbsoluteIndex(codeDocument, absoluteIndex, out var attributeNameIndex))
            {
                position = sourceText.GetPosition(attributeNameIndex);
            }
        }

        // We actually don't need a different projection strategy, we just wanted to move the caret position
        return await DefaultDocumentPositionInfoStrategy.Instance
            .TryGetPositionInfoAsync(documentMappingService, documentContext, position, cancellationToken)
            .ConfigureAwait(false);
    }
}
