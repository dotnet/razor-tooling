﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.Completion;
using Microsoft.AspNetCore.Razor.LanguageServer.Semantic;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using Xunit;
using LSPRange = OmniSharp.Extensions.LanguageServer.Protocol.Models.Range;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Test.Semantic
{
    public class DefaultRazorSemanticTokenInfoServiceTest : DefaultTagHelperServiceTestBase
    {
        #region TagHelpers
        [Fact]
        public void GetSemanticTokens_NoAttributes()
        {
            var txt = $"@addTagHelper *, TestAssembly{Environment.NewLine}<test1></test1>";
            var expectedData = new List<uint> {
                1, 1, 5, 0, 0, //line, character pos, length, tokenType, modifier
                0, 8, 5, 0, 0
            };

            var service = GetDefaultRazorSemanticTokenInfoService();
            AssertSemanticTokens(txt, expectedData, service, isRazor: false);
        }

        [Fact]
        public void GetSemanticTokens_WithAttribute()
        {
            var txt = $"@addTagHelper *, TestAssembly{Environment.NewLine}<test1 bool-val='true'></test1>";
            var expectedData = new List<uint> {
                1, 1, 5, 0, 0, //line, character pos, length, tokenType, modifier
                0, 6, 8, 1, 0,
                0, 18, 5, 0, 0
            };

            var service = GetDefaultRazorSemanticTokenInfoService();
            AssertSemanticTokens(txt, expectedData, service, isRazor: false);
        }

        [Fact]
        public void GetSemanticTokens_MinimizedAttribute()
        {
            var txt = $"@addTagHelper *, TestAssembly{Environment.NewLine}<test1 bool-val></test1>";
            var expectedData = new List<uint> {
                1, 1, 5, 0, 0, //line, character pos, length, tokenType, modifier
                0, 6, 8, 1, 0,
                0, 11, 5, 0, 0
            };

            var service = GetDefaultRazorSemanticTokenInfoService();
            AssertSemanticTokens(txt, expectedData, service, isRazor: false);
        }

        [Fact]
        public void GetSemanticTokens_IgnoresNonTagHelperAttributes()
        {
            var txt = $"@addTagHelper *, TestAssembly{Environment.NewLine}<test1 bool-val='true' class='display:none'></test1>";
            var expectedData = new List<uint> {
                1, 1, 5, 0, 0, //line, character pos, length, tokenType, modifier
                0, 6, 8, 1, 0,
                0, 39, 5, 0, 0
            };

            var service = GetDefaultRazorSemanticTokenInfoService();
            AssertSemanticTokens(txt, expectedData, service, isRazor: false);
        }

        [Fact]
        public void GetSemanticTokens_TagHelpersNotAvailableInRazor()
        {
            var txt = $"@addTagHelper *, TestAssembly{Environment.NewLine}<test1 bool-val='true' class='display:none'></test1>";
            var expectedData = new List<uint> { };

            var service = GetDefaultRazorSemanticTokenInfoService();
            AssertSemanticTokens(txt, expectedData, service, isRazor: true);
        }

        [Fact]
        public void GetSemanticTokens_DoesNotApplyOnNonTagHelpers()
        {
            var txt = $"@addTagHelper *, TestAssembly{Environment.NewLine}<p bool-val='true'></p>";
            var expectedData = new List<uint> { };

            var service = GetDefaultRazorSemanticTokenInfoService();
            AssertSemanticTokens(txt, expectedData, service, isRazor: false);
        }
        #endregion TagHelpers

        #region DirectiveAttributes
        [Fact]
        public void GetSemanticTokens_Razor_MinimizedDirectiveAttributeParameters()
        {
            // Capitalized, non-well-known-HTML elements are always marked as TagHelpers
            var txt = $"@addTagHelper *, TestAssembly{Environment.NewLine}<NotATagHelp @minimized:something />";
            var expectedData = new List<uint> {
                1, 1, 11, 0, 0,
                0, 12, 1, 2, 0,
                0, 1, 9, 4, 0,
                0, 9, 1, 3, 0,
                0, 1, 9, 4, 0
            };

            var service = GetDefaultRazorSemanticTokenInfoService();
            AssertSemanticTokens(txt, expectedData, service, isRazor: true);
        }

        [Fact]
        public void GetSemanticTokens_Razor_DirectiveAttributesParameters()
        {
            var txt = $"@addTagHelper *, TestAssembly{Environment.NewLine}<test1 @test:something='Function'></test1>";
            var expectedData = new List<uint> {
                1, 1, 5, 0, 0, //line, character pos, length, tokenType, modifier
                0, 6, 1, 2, 0,
                0, 1, 4, 4, 0,
                0, 4, 1, 3, 0,
                0, 1, 9, 4, 0,
                0, 23, 5, 0, 0
            };

            var service = GetDefaultRazorSemanticTokenInfoService();
            AssertSemanticTokens(txt, expectedData, service, isRazor: true);
        }

        [Fact]
        public void GetSemanticTokens_Razor_NonComponentsDoNotShowInRazor()
        {
            var txt = $"@addTagHelper *, TestAssembly{Environment.NewLine}<test1 bool-val='true'></test1>";
            var expectedData = new List<uint> { };

            var service = GetDefaultRazorSemanticTokenInfoService();
            AssertSemanticTokens(txt, expectedData, service, isRazor: true);
        }

        [Fact]
        public void GetSemanticTokens_Razor_Directives()
        {
            var txt = $"@addTagHelper *, TestAssembly{Environment.NewLine}<test1 @test='Function'></test1>";
            var expectedData = new List<uint> {
                1, 1, 5, 0, 0, //line, character pos, length, tokenType, modifier
                0, 6, 1, 2, 0,
                0, 1, 4, 4, 0,
                0, 18, 5, 0, 0
            };

            var service = GetDefaultRazorSemanticTokenInfoService();
            AssertSemanticTokens(txt, expectedData, service, isRazor: true);
        }

        [Fact]
        public void GetSemanticTokens_Razor_DoNotColorNonTagHelpers()
        {
            var txt = $"@addTaghelper *, TestAssembly{Environment.NewLine}<p @test='Function'></p>";
            var expectedData = new List<uint> {
                1, 3, 1, 2, 0,
                0, 1, 4, 4, 0
            };

            var service = GetDefaultRazorSemanticTokenInfoService();
            AssertSemanticTokens(txt, expectedData, service, isRazor: true);
        }

        [Fact]
        public void GetSemanticTokens_Razor_DoesNotApplyOnNonTagHelpers()
        {
            var txt = $"@addTagHelpers *, TestAssembly{Environment.NewLine}<p></p>";
            var expectedData = new List<uint> { };

            var service = GetDefaultRazorSemanticTokenInfoService();
            AssertSemanticTokens(txt, expectedData, service, isRazor: true);
        }

        [Fact]
        public void GetSemanticTokens_Razor_InRange()
        {
            var txt = $"@addTagHelper *, TestAssembly{Environment.NewLine}<test1></test1>";
            var expectedData = new List<uint> {
                1, 1, 5, 0, 0, //line, character pos, length, tokenType, modifier
            };

            var startIndex = txt.IndexOf("test1");
            var endIndex = startIndex + 5;

            var codeDocument = CreateCodeDocument(txt, DefaultTagHelpers);

            codeDocument.GetSourceText().GetLineAndOffset(startIndex, out var startLine, out var startChar);
            codeDocument.GetSourceText().GetLineAndOffset(endIndex, out var endLine, out var endChar);

            var startPosition = new Position(startLine, startChar);
            var endPosition = new Position(endLine, endChar);
            var location = new LSPRange(startPosition, endPosition);

            var service = GetDefaultRazorSemanticTokenInfoService();
            AssertSemanticTokens(txt, expectedData, service, location: location, isRazor: false);
        }
        #endregion DirectiveAttributes

        [Fact]
        public void GetSemanticTokens_Razor_NoDifferenceAsync()
        {
            var txt = $"@addTagHelper *, TestAssembly{Environment.NewLine}<test1></test1>";
            var expectedData = new List<uint> {
                1, 1, 5, 0, 0, //line, character pos, length, tokenType, modifier
                0, 8, 5, 0, 0
            };

            var service = GetDefaultRazorSemanticTokenInfoService();
            var previousResultId = AssertSemanticTokens(txt, expectedData, service, isRazor: false);

            var newResultId = AssertSemanticTokenEdits(txt, new SemanticTokensEdits { Edits = new List<SemanticTokensEdit>() }, service, isRazor: false, previousResultId: previousResultId);
            Assert.NotEqual(previousResultId, newResultId);
        }

        [Fact]
        public void GetSemanticTokens_Razor_RemoveTokens()
        {
            var txt = $"@addTagHelper *, TestAssembly{Environment.NewLine}<test1></test1><test1></test1><test1></test1>";
            var expectedData = new List<uint> {
                1, 1, 5, 0, 0, //line, character pos, length, tokenType, modifier
                0, 8, 5, 0, 0,
                0, 7, 5, 0, 0,
                0, 8, 5, 0, 0,
                0, 7, 5, 0, 0,
                0, 8, 5, 0, 0
            };

            var service = GetDefaultRazorSemanticTokenInfoService();
            var previousResultId = AssertSemanticTokens(txt, expectedData, service, isRazor: false);

            var newTxt = $"@addTagHelper *, TestAssembly{Environment.NewLine}<test1></test1>";
            var newResultId = AssertSemanticTokenEdits(newTxt, new SemanticTokensEdits { Edits = new List<SemanticTokensEdit>(){
                new SemanticTokensEdit
                {
                    Data = null,
                    DeleteCount = 20,
                    Start = 10
                }
            }}, service, isRazor: false, previousResultId: previousResultId);
            Assert.NotEqual(previousResultId, newResultId);
        }

        [Fact]
        public void GetSemanticTokens_Razor_OnlyDifferences_AppendAsync()
        {
            var txt = $"@addTagHelper *, TestAssembly{Environment.NewLine}<test1></test1>";
            var expectedData = new List<uint> {
                1, 1, 5, 0, 0, //line, character pos, length, tokenType, modifier
                0, 8, 5, 0, 0
            };

            var service = GetDefaultRazorSemanticTokenInfoService();
            var previousResultId = AssertSemanticTokens(txt, expectedData, service, isRazor: false);

            var newTxt = $"@addTagHelper *, TestAssembly{Environment.NewLine}<test1 bool-val='true'></test1>";
            var newExpectedData = new SemanticTokensEdits {
                Edits = new SemanticTokensEdit[] {
                    new SemanticTokensEdit
                    {
                        Start = 6,
                        Data = new List<uint>{ 6 , 8, 1, 0, 0, 18},
                        DeleteCount = 1,
                    }
                }
            };
            var newResultId = AssertSemanticTokenEdits(newTxt, newExpectedData, service, isRazor: false, previousResultId: previousResultId);
            Assert.NotEqual(previousResultId, newResultId);
        }

        [Fact]
        public void GetSemanticTokens_Razor_OnlyDifferences_InternalAsync()
        {
            var txt = $"@addTagHelper *, TestAssembly{Environment.NewLine}<test1></test1>";
            var expectedData = new List<uint> {
                1, 1, 5, 0, 0, //line, character pos, length, tokenType, modifier
                0, 8, 5, 0, 0
            };

            var service = GetDefaultRazorSemanticTokenInfoService();
            var previousResultId = AssertSemanticTokens(txt, expectedData, service, isRazor: false);

            var newTxt = $"@addTagHelper *, TestAssembly{Environment.NewLine}<test1></test1><test1></test1>";
            var newExpectedData = new SemanticTokensEdits
            {
                Edits = new List<SemanticTokensEdit> {
                    new SemanticTokensEdit
                    {
                        Start = 10,
                        Data = new uint[]{
                            0, 7, 5, 0, 0,
                            0, 8, 5, 0, 0,
                        },
                        DeleteCount = 0,
                    }
                }
            };
            var newResultId = AssertSemanticTokenEdits(newTxt, newExpectedData, service, isRazor: false, previousResultId: previousResultId);
            Assert.NotEqual(previousResultId, newResultId);
        }

        [Fact]
        public void GetSemanticTokens_Razor_OnlyDifferences_NewLinesAsync()
        {
            var txt = $"@addTagHelper *, TestAssembly{Environment.NewLine}<test1></test1>";
            var expectedData = new List<uint> {
                1, 1, 5, 0, 0, //line, character pos, length, tokenType, modifier
                0, 8, 5, 0, 0
            };

            var service = GetDefaultRazorSemanticTokenInfoService();
            var previousResultId = AssertSemanticTokens(txt, expectedData, service, isRazor: false);

            var newTxt = $"@addTagHelper *, TestAssembly{Environment.NewLine}<test1></test1>{Environment.NewLine}" +
                $"<test1></test1>";
            var newExpectedData = new SemanticTokensEdits
            {
                Edits = new List<SemanticTokensEdit> {
                    new SemanticTokensEdit
                    {
                        Start = 10,
                        Data = new uint[]{
                            1, 1, 5, 0, 0,
                            0, 8, 5, 0, 0,
                        },
                        DeleteCount = 0,
                    }
                }
            };
            var newResultId = AssertSemanticTokenEdits(newTxt, newExpectedData, service, isRazor: false, previousResultId: previousResultId);
            Assert.NotEqual(previousResultId, newResultId);
        }

        private string AssertSemanticTokens(string txt, IEnumerable<uint> expectedData, RazorSemanticTokenInfoService service, bool isRazor, LSPRange location = null)
        {
            // Arrange
            RazorCodeDocument codeDocument;
            if (isRazor)
            {
                codeDocument = CreateRazorDocument(txt, DefaultTagHelpers);
            }
            else
            {
                codeDocument = CreateCodeDocument(txt, DefaultTagHelpers);
            }

            // Act
            var tokens = service.GetSemanticTokens(codeDocument, location);

            // Assert
            Assert.Equal(expectedData, tokens.Data);

            return tokens.ResultId;
        }

        private string AssertSemanticTokenEdits(string txt, SemanticTokensEdits expectedEdits, RazorSemanticTokenInfoService service, bool isRazor, string previousResultId)
        {
            // Arrange
            RazorCodeDocument codeDocument;
            if (isRazor)
            {
                codeDocument = CreateRazorDocument(txt, DefaultTagHelpers);
            }
            else
            {
                codeDocument = CreateCodeDocument(txt, DefaultTagHelpers);
            }

            // Act
            var edits = service.GetSemanticTokenEdits(codeDocument, previousResultId);

            // Assert
            for(var i = 0; i < expectedEdits.Edits.Count; i++)
            {
                Assert.Equal(expectedEdits.Edits[i], edits.SemanticTokensEdits.Edits[i]);
            }

            return edits.SemanticTokensEdits.ResultId;
        }

        private RazorSemanticTokenInfoService GetDefaultRazorSemanticTokenInfoService()
        {
            return new DefaultRazorSemanticTokenInfoService();
        }
    }
}
