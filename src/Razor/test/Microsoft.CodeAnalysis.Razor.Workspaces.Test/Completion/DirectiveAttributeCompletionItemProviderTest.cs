﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Legacy;
using Microsoft.AspNetCore.Razor.Language.IntegrationTests;
using Microsoft.VisualStudio.Editor.Razor;
using Xunit;

namespace Microsoft.CodeAnalysis.Razor.Completion
{
    public class DirectiveAttributeCompletionItemProviderTest : RazorIntegrationTestBase
    {
        internal override string FileKind => FileKinds.Component;

        internal override bool UseTwoPhaseCompilation => true;

        public DirectiveAttributeCompletionItemProviderTest()
        {
            Provider = new DirectiveAttributeCompletionItemProvider(new DefaultTagHelperFactsService());
            EmptyAttributes = Enumerable.Empty<string>();

            // Most of these completions rely on stuff in the web namespace.
            ImportItems.Add(CreateProjectItem(
                "_Imports.razor",
                "@using Microsoft.AspNetCore.Components.Web"));

            var codeDocument = GetCodeDocument(string.Empty);
            DefaultTagHelperDocumentContext = codeDocument.GetTagHelperContext();
        }

        private DirectiveAttributeCompletionItemProvider Provider { get; }

        private TagHelperDocumentContext DefaultTagHelperDocumentContext { get; }

        private IEnumerable<string> EmptyAttributes { get; }

        private RazorCodeDocument GetCodeDocument(string content)
        {
            var result = CompileToCSharp(content, throwOnFailure: false);
            return result.CodeDocument;
        }

        [Fact]
        public void GetCompletionItems_LocationHasNoOwner_ReturnsEmptyCollection()
        {
            // Arrange
            var context = CreateRazorCompletionContext(absoluteIndex: 30, "<input @  />");

            // Act
            var completions = Provider.GetCompletionItems(context);

            // Assert
            Assert.Empty(completions);
        }

        [Fact]
        public void GetCompletionItems_OnNonAttributeArea_ReturnsEmptyCollection()
        {
            // Arrange
            var context = CreateRazorCompletionContext(absoluteIndex: 3, "<input @  />");

            // Act
            var completions = Provider.GetCompletionItems(context);

            // Assert
            Assert.Empty(completions);
        }

        [Fact]
        public void GetCompletionItems_OnDirectiveAttributeParameter_ReturnsEmptyCollection()
        {
            // Arrange
            var context = CreateRazorCompletionContext(absoluteIndex: 14, "<input @bind:fo  />");

            // Act
            var completions = Provider.GetCompletionItems(context);

            // Assert
            Assert.Empty(completions);
        }

        [Fact]
        public void GetCompletionItems_OnDirectiveAttributeName_bind_ReturnsCompletions()
        {
            // Arrange
            var context = CreateRazorCompletionContext(absoluteIndex: 8, "<input @  />");

            // Act
            var completions = Provider.GetCompletionItems(context);

            // Assert
            AssertContains(completions, "bind", "@bind", new[] { "=", ":" });
        }

        [Fact]
        public void GetCompletionItems_OnDirectiveAttributeName_attributes_ReturnsCompletions()
        {
            // Arrange
            var context = CreateRazorCompletionContext(absoluteIndex: 8, "<input @  />");

            // Act
            var completions = Provider.GetCompletionItems(context);

            // Assert
            AssertContains(completions, "attributes", "@attributes", new[] { "=" });
        }

        [Fact]
        public void GetCompletionItems_AttributeAreaEndOfSelfClosingTag_ReturnsEmptyCollection()
        {
            // Arrange
            var context = CreateRazorCompletionContext(absoluteIndex: 16, "<input @bind:fo  />");

            // Act
            var completions = Provider.GetCompletionItems(context);

            // Assert
            Assert.Empty(completions);
        }

        [Fact]
        public void GetCompletionItems_AttributeAreaEndOfOpeningTag_ReturnsEmptyCollection()
        {
            // Arrange
            var context = CreateRazorCompletionContext(absoluteIndex: 16, "<input @bind:fo   ></input>");

            // Act
            var completions = Provider.GetCompletionItems(context);

            // Assert
            Assert.Empty(completions);
        }

        [Fact]
        public void GetCompletionItems_ExistingAttribute_LeadingEdge_ReturnsEmptyCollection()
        {
            // Arrange
            var context = CreateRazorCompletionContext(absoluteIndex: 7, "<input src=\"xyz\" />");

            // Act
            var completions = Provider.GetCompletionItems(context);

            // Assert
            Assert.Empty(completions);
        }

        [Fact]
        public void GetCompletionItems_ExistingAttribute_TrailingEdge_ReturnsEmptyCollection()
        {
            // Arrange
            var context = CreateRazorCompletionContext(absoluteIndex: 16, "<input src=\"xyz\" />");

            // Act
            var completions = Provider.GetCompletionItems(context);

            // Assert
            Assert.Empty(completions);
        }

        [Fact]
        public void GetCompletionItems_ExistingAttribute_Partial_ReturnsEmptyCollection()
        {
            // Arrange
            var context = CreateRazorCompletionContext(absoluteIndex: 9, "<svg xml: ></svg>");

            // Act
            var completions = Provider.GetCompletionItems(context);

            // Assert
            Assert.Empty(completions);
        }

        [Fact]
        public void GetAttributeCompletions_NoDescriptorsForTag_ReturnsEmptyCollection()
        {
            // Arrange
            var documentContext = TagHelperDocumentContext.Create(string.Empty, Enumerable.Empty<TagHelperDescriptor>());

            // Act
            var completions = Provider.GetAttributeCompletions("@bin", "foobarbaz", EmptyAttributes, documentContext);

            // Assert
            Assert.Empty(completions);
        }

        [Fact]
        public void GetAttributeCompletions_NoDirectiveAttributesForTag_ReturnsEmptyCollection()
        {
            // Arrange
            var descriptor = TagHelperDescriptorBuilder.Create("CatchAll", "TestAssembly");
            descriptor.BoundAttributeDescriptor(boundAttribute => boundAttribute.Name = "Test");
            descriptor.TagMatchingRule(rule => rule.RequireTagName("*"));
            var documentContext = TagHelperDocumentContext.Create(string.Empty, new[] { descriptor.Build() });

            // Act
            var completions = Provider.GetAttributeCompletions("@bin", "input", EmptyAttributes, documentContext);

            // Assert
            Assert.Empty(completions);
        }

        [Fact]
        public void GetAttributeCompletions_SelectedDirectiveAttribute_IsIncludedInCompletions()
        {
            // Arrange
            var attributeNames = new string[] { "@bind" };

            // Act
            var completions = Provider.GetAttributeCompletions("@bind", "input", attributeNames, DefaultTagHelperDocumentContext);

            // Assert
            AssertContains(completions, "bind", "@bind", new[] { "=", ":" });
        }

        [Fact]
        public void GetAttributeCompletions_NonIndexer_ReturnsCompletion()
        {
            // Arrange

            // Act
            var completions = Provider.GetAttributeCompletions("@", "input", EmptyAttributes, DefaultTagHelperDocumentContext);

            // Assert
            AssertContains(completions, "bind", "@bind", new[] { "=", ":" });
        }

        [Fact]
        public void GetAttributeCompletions_Indexer_ReturnsCompletion()
        {
            // Arrange

            // Act
            var completions = Provider.GetAttributeCompletions("@", "input", EmptyAttributes, DefaultTagHelperDocumentContext);

            // Assert
            AssertContains(completions, "bind-", "@bind-...", Array.Empty<string>());
        }

        [Fact]
        public void GetAttributeCompletions_BaseDirectiveAttributeAlreadyExists_IncludesBaseAttribute()
        {
            // Arrange
            var attributeNames = new[] { "@bind", "@" };

            // Act
            var completions = Provider.GetAttributeCompletions("@", "input", attributeNames, DefaultTagHelperDocumentContext);

            // Assert
            AssertContains(completions, "bind", "@bind", new[] { "=", ":" });
        }

        [Fact]
        public void GetAttributeCompletions_BaseDirectiveAttributeAndParameterVariationsExist_ExcludesCompletion()
        {
            // Arrange
            var attributeNames = new[]
            {
                "@bind",
                "@bind:format",
                "@bind:event",
                "@bind:culture",
                "@bind:get",
                "@bind:set",
                "@bind:after",
                "@",
            };

            // Act
            var completions = Provider.GetAttributeCompletions("@", "input", attributeNames, DefaultTagHelperDocumentContext);

            // Assert
            AssertDoesNotContain(completions, "bind", "@bind");
        }

        private static void AssertContains(IReadOnlyList<RazorCompletionItem> completions, string insertText, string displayText, IReadOnlyCollection<string> commitCharacters)
        {
            displayText ??= insertText;

            Assert.Contains(completions, completion =>
                insertText == completion.InsertText &&
                displayText == completion.DisplayText &&
                commitCharacters.SequenceEqual(completion.CommitCharacters.Select(c => c.Character)) &&
                RazorCompletionItemKind.DirectiveAttribute == completion.Kind);
        }

        private static void AssertDoesNotContain(IReadOnlyList<RazorCompletionItem> completions, string insertText, string displayText)
        {
            displayText ??= insertText;

            Assert.DoesNotContain(completions, completion => insertText == completion.InsertText &&
                   displayText == completion.DisplayText &&
                   RazorCompletionItemKind.DirectiveAttribute == completion.Kind);
        }

        private RazorCompletionContext CreateRazorCompletionContext(int absoluteIndex, string documentContent)
        {
            var codeDocument = GetCodeDocument(documentContent);
            var syntaxTree = codeDocument.GetSyntaxTree();
            var tagHelperDocumentContext = codeDocument.GetTagHelperContext();

            var queryableChange = new SourceChange(absoluteIndex, length: 0, newText: string.Empty);
            var owner = syntaxTree.Root.LocateOwner(queryableChange);
            return new RazorCompletionContext(absoluteIndex, owner, syntaxTree, tagHelperDocumentContext);
        }
    }
}
