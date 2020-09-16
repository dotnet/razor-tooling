﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.CodeActions.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace Microsoft.AspNetCore.Razor.LanguageServer.CodeActions
{
    internal class TypeAccessibilityCodeActionProvider : CSharpCodeActionProvider
    {
        private static readonly IEnumerable<string> SupportedDiagnostics = new[]
        {
            // `The type or namespace name 'type/namespace' could not be found
            //  (are you missing a using directive or an assembly reference?)`
            // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/compiler-messages/cs0246
            "CS0246",

            // `The name 'identifier' does not exist in the current context`
            // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/compiler-messages/cs0103
            "CS0103"
        };

        public override Task<IReadOnlyList<RazorCodeAction>> ProvideAsync(
            RazorCodeActionContext context,
            IEnumerable<RazorCodeAction> codeActions,
            CancellationToken cancellationToken)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (codeActions is null)
            {
                throw new ArgumentNullException(nameof(codeActions));
            }

            if (context.Request?.Context?.Diagnostics is null)
            {
                return null;
            }

            if (codeActions is null || !codeActions.Any())
            {
                return null;
            }

            var diagnostics = context.Request.Context.Diagnostics.Where(diagnostic =>
                diagnostic.Severity == DiagnosticSeverity.Error &&
                diagnostic.Code?.IsString == true &&
                SupportedDiagnostics.Any(d => diagnostic.Code.Value.String.Equals(d, StringComparison.OrdinalIgnoreCase)));

            if (diagnostics is null)
            {
                return null;
            }

            var results = new List<RazorCodeAction>();

            foreach (var diagnostic in diagnostics)
            {
                var diagnosticSpan = diagnostic.Range.AsTextSpan(context.SourceText);
                var associatedValue = context.SourceText.GetSubTextString(diagnosticSpan);

                foreach (var codeAction in codeActions)
                {
                    if (!codeAction.Title.Any(c => char.IsWhiteSpace(c)) &&
                        codeAction.Title.EndsWith(associatedValue, StringComparison.OrdinalIgnoreCase))
                    {
                        var fqnCodeAction = CreateFQNCodeAction(context, diagnostic, codeAction);
                        results.Add(fqnCodeAction);

                        var addUsingCodeAction = CreateAddUsingCodeAction(context, codeAction);
                        if (addUsingCodeAction != null)
                        {
                            results.Add(addUsingCodeAction);
                        }
                    }
                }
            }

            return Task.FromResult(results as IReadOnlyList<RazorCodeAction>);
        }

        private static RazorCodeAction CreateFQNCodeAction(
            RazorCodeActionContext context,
            Diagnostic fqnDiagnostic,
            RazorCodeAction codeAction)
        {
            var codeDocumentIdentifier = new VersionedTextDocumentIdentifier() { Uri = context.Request.TextDocument.Uri };

            var fqnTextEdit = new TextEdit()
            {
                NewText = codeAction.Title,
                Range = fqnDiagnostic.Range
            };

            var fqnWorkspaceEditDocumentChange = new WorkspaceEditDocumentChange(new TextDocumentEdit()
            {
                TextDocument = codeDocumentIdentifier,
                Edits = new[] { fqnTextEdit },
            });

            var fqnWorkspaceEdit = new WorkspaceEdit()
            {
                DocumentChanges = new[] { fqnWorkspaceEditDocumentChange }
            };

            return new RazorCodeAction()
            {
                Title = codeAction.Title,
                Edit = fqnWorkspaceEdit
            };
        }

        private static RazorCodeAction CreateAddUsingCodeAction(
            RazorCodeActionContext context,
            RazorCodeAction codeAction)
        {
            return AddUsingsCodeActionProviderFactory.CreateAddUsingCodeAction(
                fullyQualifiedName: codeAction.Title,
                context.Request.TextDocument.Uri);
        }
    }
}
