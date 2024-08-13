﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Immutable;
using Microsoft.CodeAnalysis.CSharp;
using Xunit;

namespace Microsoft.AspNetCore.Razor.Language.Legacy;

public class CSharpPreprocessorTest() : ParserTestBase(layer: TestProject.Layer.Compiler, validateSpanEditHandlers: false, useLegacyTokenizer: true)
{
    [Fact]
    public void Pragmas()
    {
        ParseDocumentTest("""
            @{
            #pragma warning disable 123
            #pragma warning restore 123
            #pragma checksum "file.cs" "{00000000-0000-0000-0000-000000000000}" "1234"
            }
            """);
    }

    [Fact]
    public void NullableDirectives()
    {
        ParseDocumentTest("""
            @{
            #nullable enable
            #nullable disable
            #nullable restore
            #nullable enable annotations
            #nullable disable annotations
            #nullable restore annotations
            #nullable enable warnings
            #nullable disable warnings
            #nullable restore warnings
            }
            """);
    }

    [Fact]
    public void DefineThenIfDef()
    {
        ParseDocumentTest("""
            @{
            #define SYMBOL
            #if SYMBOL
            #undef SYMBOL
            #if SYMBOL
                var x = 1;
            #endif
            #else
                var x = 1;
            #endif
            }
            """);
    }

    [Fact]
    public void ErrorWarningLine()
    {
        ParseDocumentTest("""
            @{
            #line 1 "file.cs"
            #error This is an error
            #line default
            #warning This is a warning
            #line hidden
            #line (1, 1) - (5, 60) 10 "partial-class.cs"
            }
            """);
    }

    [Fact]
    public void Regions()
    {
        ParseDocumentTest("""
            @{
            #region MyRegion }
            #endregion
            }
            """);
    }

    [Fact]
    public void SimpleIfDef()
    {
        ParseDocumentTest("""
            @{
            #if true
                var x = 1;
            #endif
            }
            """);
    }

    [Fact]
    public void IfDefFromParseOptions_Symbol()
    {
        IfDefFromParseOptions("SYMBOL");
    }

    [Fact]
    public void IfDefFromParseOptions_Symbol2()
    {
        IfDefFromParseOptions("SYMBOL2");
    }

    [Fact]
    public void IfDefFromParseOptions_None()
    {
        IfDefFromParseOptions(null);
    }

    private void IfDefFromParseOptions(string? directive)
    {
        var parseOptions = CSharpParseOptions.Default;

        if (directive != null)
        {
            parseOptions = parseOptions.WithPreprocessorSymbols(ImmutableArray.Create(directive));
        }

        ParseDocumentTest("""
            @{
            #if SYMBOL
                var x = 1;
            #elif SYMBOL2
                var x = 2;
            #else
                var x = 3;
            #endif
            }
            """, parseOptions);
    }

    [Fact]
    public void IfDefAcrossMultipleBlocks()
    {
        ParseDocumentTest("""
            @{
            #if false
                var x = 1;
            }

            <div>
                <p>Content</p>
            </div>

            @{
                var y = 2;
            #endif
            }
            """);
    }

    [Fact]
    public void IfDefDisabledSectionUnbalanced()
    {
        ParseDocumentTest("""
            @{
            #if false
                void M() {
            #endif
            }
            """);
    }

    [Fact]
    public void IfDefNotOnNewline_01()
    {
        ParseDocumentTest("""
            @{ #if false }
            <div>
                <p>Content</p>
            </div>
            @{
            #endif
            }
            """);
    }

    [Fact]
    public void IfDefNotOnNewline_02()
    {
        ParseDocumentTest("""
            @{#if false }
            <div>
                <p>Content</p>
            </div>
            @{
            #endif
            }
            """);
    }

    [Fact]
    public void ElIfNotOnNewline()
    {
        ParseDocumentTest("""
            @{
            #if true
            }
            <div>
                <p>Content</p>
            </div>
            @{ #elif false }
            <div>
                <p>Content2</p>
            </div>
            @{
            #endif
            }
            """);
    }

    [Fact]
    public void ElseNotOnNewline()
    {
        ParseDocumentTest("""
            @{
            #if true
            }
            <div>
                <p>Content</p>
            </div>
            @{ #else }
            <div>
                <p>Content2</p>
            </div>
            @{
            #endif
            }
            """);
    }

    [Fact]
    public void EndIfNotOnNewline()
    {
        ParseDocumentTest("""
            @{
            #if false
            }
            <div>
                <p>Content</p>
            </div>
            @{ #endif }
            """);
    }
}
