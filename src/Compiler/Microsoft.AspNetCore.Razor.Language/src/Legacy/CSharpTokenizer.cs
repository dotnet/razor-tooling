﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Razor.Language.Syntax.InternalSyntax;
using Microsoft.CodeAnalysis.CSharp;

using SyntaxFactory = Microsoft.AspNetCore.Razor.Language.Syntax.InternalSyntax.SyntaxFactory;
using CSharpSyntaxKind = Microsoft.CodeAnalysis.CSharp.SyntaxKind;

namespace Microsoft.AspNetCore.Razor.Language.Legacy;

internal class CSharpTokenizer : Tokenizer
{
    private readonly SourceTextLexer _lexer;

    private static readonly Dictionary<string, CSharpKeyword> _keywords = new Dictionary<string, CSharpKeyword>(StringComparer.Ordinal)
        {
            { "await", CSharpKeyword.Await },
            { "abstract", CSharpKeyword.Abstract },
            { "byte", CSharpKeyword.Byte },
            { "class", CSharpKeyword.Class },
            { "delegate", CSharpKeyword.Delegate },
            { "event", CSharpKeyword.Event },
            { "fixed", CSharpKeyword.Fixed },
            { "if", CSharpKeyword.If },
            { "internal", CSharpKeyword.Internal },
            { "new", CSharpKeyword.New },
            { "override", CSharpKeyword.Override },
            { "readonly", CSharpKeyword.Readonly },
            { "short", CSharpKeyword.Short },
            { "struct", CSharpKeyword.Struct },
            { "try", CSharpKeyword.Try },
            { "unsafe", CSharpKeyword.Unsafe },
            { "volatile", CSharpKeyword.Volatile },
            { "as", CSharpKeyword.As },
            { "do", CSharpKeyword.Do },
            { "is", CSharpKeyword.Is },
            { "params", CSharpKeyword.Params },
            { "ref", CSharpKeyword.Ref },
            { "switch", CSharpKeyword.Switch },
            { "ushort", CSharpKeyword.Ushort },
            { "while", CSharpKeyword.While },
            { "case", CSharpKeyword.Case },
            { "const", CSharpKeyword.Const },
            { "explicit", CSharpKeyword.Explicit },
            { "float", CSharpKeyword.Float },
            { "null", CSharpKeyword.Null },
            { "sizeof", CSharpKeyword.Sizeof },
            { "typeof", CSharpKeyword.Typeof },
            { "implicit", CSharpKeyword.Implicit },
            { "private", CSharpKeyword.Private },
            { "this", CSharpKeyword.This },
            { "using", CSharpKeyword.Using },
            { "extern", CSharpKeyword.Extern },
            { "return", CSharpKeyword.Return },
            { "stackalloc", CSharpKeyword.Stackalloc },
            { "uint", CSharpKeyword.Uint },
            { "base", CSharpKeyword.Base },
            { "catch", CSharpKeyword.Catch },
            { "continue", CSharpKeyword.Continue },
            { "double", CSharpKeyword.Double },
            { "for", CSharpKeyword.For },
            { "in", CSharpKeyword.In },
            { "lock", CSharpKeyword.Lock },
            { "object", CSharpKeyword.Object },
            { "protected", CSharpKeyword.Protected },
            { "static", CSharpKeyword.Static },
            { "false", CSharpKeyword.False },
            { "public", CSharpKeyword.Public },
            { "sbyte", CSharpKeyword.Sbyte },
            { "throw", CSharpKeyword.Throw },
            { "virtual", CSharpKeyword.Virtual },
            { "decimal", CSharpKeyword.Decimal },
            { "else", CSharpKeyword.Else },
            { "operator", CSharpKeyword.Operator },
            { "string", CSharpKeyword.String },
            { "ulong", CSharpKeyword.Ulong },
            { "bool", CSharpKeyword.Bool },
            { "char", CSharpKeyword.Char },
            { "default", CSharpKeyword.Default },
            { "foreach", CSharpKeyword.Foreach },
            { "long", CSharpKeyword.Long },
            { "void", CSharpKeyword.Void },
            { "enum", CSharpKeyword.Enum },
            { "finally", CSharpKeyword.Finally },
            { "int", CSharpKeyword.Int },
            { "out", CSharpKeyword.Out },
            { "sealed", CSharpKeyword.Sealed },
            { "true", CSharpKeyword.True },
            { "goto", CSharpKeyword.Goto },
            { "unchecked", CSharpKeyword.Unchecked },
            { "interface", CSharpKeyword.Interface },
            { "break", CSharpKeyword.Break },
            { "checked", CSharpKeyword.Checked },
            { "namespace", CSharpKeyword.Namespace },
            { "when", CSharpKeyword.When },
            { "where", CSharpKeyword.Where }
        };

    public CSharpTokenizer(SeekableTextReader source)
        : base(source)
    {
        base.CurrentState = StartState;

        // _operatorHandlers = new Dictionary<char, Func<SyntaxKind>>()
        //     {
        //         { '-', MinusOperator },
        //         { '<', LessThanOperator },
        //         { '>', GreaterThanOperator },
        //         { '&', CreateTwoCharOperatorHandler(SyntaxKind.And, '=', SyntaxKind.AndAssign, '&', SyntaxKind.DoubleAnd) },
        //         { '|', CreateTwoCharOperatorHandler(SyntaxKind.Or, '=', SyntaxKind.OrAssign, '|', SyntaxKind.DoubleOr) },
        //         { '+', CreateTwoCharOperatorHandler(SyntaxKind.Plus, '=', SyntaxKind.PlusAssign, '+', SyntaxKind.Increment) },
        //         { '=', CreateTwoCharOperatorHandler(SyntaxKind.Assign, '=', SyntaxKind.Equals, '>', SyntaxKind.GreaterThanEqual) },
        //         { '!', CreateTwoCharOperatorHandler(SyntaxKind.Not, '=', SyntaxKind.NotEqual) },
        //         { '%', CreateTwoCharOperatorHandler(SyntaxKind.Modulo, '=', SyntaxKind.ModuloAssign) },
        //         { '*', CreateTwoCharOperatorHandler(SyntaxKind.Star, '=', SyntaxKind.MultiplyAssign) },
        //         { ':', CreateTwoCharOperatorHandler(SyntaxKind.Colon, ':', SyntaxKind.DoubleColon) },
        //         { '?', CreateTwoCharOperatorHandler(SyntaxKind.QuestionMark, '?', SyntaxKind.NullCoalesce) },
        //         { '^', CreateTwoCharOperatorHandler(SyntaxKind.Xor, '=', SyntaxKind.XorAssign) },
        //         { '(', () => SyntaxKind.LeftParenthesis },
        //         { ')', () => SyntaxKind.RightParenthesis },
        //         { '{', () => SyntaxKind.LeftBrace },
        //         { '}', () => SyntaxKind.RightBrace },
        //         { '[', () => SyntaxKind.LeftBracket },
        //         { ']', () => SyntaxKind.RightBracket },
        //         { ',', () => SyntaxKind.Comma },
        //         { ';', () => SyntaxKind.Semicolon },
        //         { '~', () => SyntaxKind.Tilde },
        //         { '#', () => SyntaxKind.Hash }
        //     };

        _lexer = CodeAnalysis.CSharp.SyntaxFactory.CreateLexer(source.SourceText);
    }

    protected override int StartState => (int)CSharpTokenizerState.Data;

    private new CSharpTokenizerState? CurrentState => (CSharpTokenizerState?)base.CurrentState;

    public override SyntaxKind RazorCommentKind => SyntaxKind.RazorCommentLiteral;

    public override SyntaxKind RazorCommentTransitionKind => SyntaxKind.RazorCommentTransition;

    public override SyntaxKind RazorCommentStarKind => SyntaxKind.RazorCommentStar;

    protected override StateResult Dispatch()
    {
        switch (CurrentState)
        {
            case CSharpTokenizerState.Data:
                return Data();
            case CSharpTokenizerState.BlockComment:
                return BlockComment();
            case CSharpTokenizerState.QuotedCharacterLiteral:
                return TokenizedExpectedStringOrCharacterLiteral(CodeAnalysis.CSharp.SyntaxKind.CharacterLiteralToken, SyntaxKind.CharacterLiteral, "\'", "\'");
            case CSharpTokenizerState.QuotedStringLiteral:
                return TokenizedExpectedStringOrCharacterLiteral(CodeAnalysis.CSharp.SyntaxKind.StringLiteralToken, SyntaxKind.StringLiteral, "\"", "\"");
            case CSharpTokenizerState.VerbatimStringLiteral:
                return TokenizedExpectedStringOrCharacterLiteral(CodeAnalysis.CSharp.SyntaxKind.StringLiteralToken, SyntaxKind.StringLiteral, "@\"", "\"");
            case CSharpTokenizerState.AfterRazorCommentTransition:
                return AfterRazorCommentTransition();
            case CSharpTokenizerState.EscapedRazorCommentTransition:
                return EscapedRazorCommentTransition();
            case CSharpTokenizerState.RazorCommentBody:
                return RazorCommentBody();
            case CSharpTokenizerState.StarAfterRazorCommentBody:
                return StarAfterRazorCommentBody();
            case CSharpTokenizerState.AtTokenAfterRazorCommentBody:
                return AtTokenAfterRazorCommentBody();
            default:
                Debug.Fail("Invalid TokenizerState");
                return default(StateResult);
        }
    }

    // Optimize memory allocation by returning constants for the most frequent cases
    protected override string GetTokenContent(SyntaxKind type)
    {
        var tokenLength = Buffer.Length;

        if (tokenLength == 1)
        {
            switch (type)
            {
                case SyntaxKind.NumericLiteral:
                    switch (Buffer[0])
                    {
                        case '0':
                            return "0";
                        case '1':
                            return "1";
                        case '2':
                            return "2";
                        case '3':
                            return "3";
                        case '4':
                            return "4";
                        case '5':
                            return "5";
                        case '6':
                            return "6";
                        case '7':
                            return "7";
                        case '8':
                            return "8";
                        case '9':
                            return "9";
                    }
                    break;
                case SyntaxKind.NewLine:
                    if (Buffer[0] == '\n')
                    {
                        return "\n";
                    }
                    break;
                case SyntaxKind.Whitespace:
                    if (Buffer[0] == ' ')
                    {
                        return " ";
                    }
                    if (Buffer[0] == '\t')
                    {
                        return "\t";
                    }
                    break;
                // case SyntaxKind.Minus:
                //     return "-";
                case SyntaxKind.Not:
                    return "!";
                // case SyntaxKind.Modulo:
                //     return "%";
                // case SyntaxKind.And:
                //     return "&";
                case SyntaxKind.LeftParenthesis:
                    return "(";
                case SyntaxKind.RightParenthesis:
                    return ")";
                // case SyntaxKind.Star:
                //     return "*";
                case SyntaxKind.Comma:
                    return ",";
                case SyntaxKind.Dot:
                    return ".";
                // case SyntaxKind.Slash:
                //     return "/";
                case SyntaxKind.Colon:
                    return ":";
                case SyntaxKind.Semicolon:
                    return ";";
                case SyntaxKind.QuestionMark:
                    return "?";
                case SyntaxKind.RightBracket:
                    return "]";
                case SyntaxKind.LeftBracket:
                    return "[";
                // case SyntaxKind.Xor:
                //     return "^";
                case SyntaxKind.LeftBrace:
                    return "{";
                // case SyntaxKind.Or:
                //     return "|";
                case SyntaxKind.RightBrace:
                    return "}";
                // case SyntaxKind.Tilde:
                //     return "~";
                // case SyntaxKind.Plus:
                //     return "+";
                case SyntaxKind.LessThan:
                    return "<";
                case SyntaxKind.Assign:
                    return "=";
                case SyntaxKind.GreaterThan:
                    return ">";
                // case SyntaxKind.Hash:
                //     return "#";
                case SyntaxKind.Transition:
                    return "@";

            }
        }
        else if (tokenLength == 2)
        {
            switch (type)
            {
                case SyntaxKind.NewLine:
                    return "\r\n";
                // case SyntaxKind.Arrow:
                //     return "->";
                // case SyntaxKind.Decrement:
                //     return "--";
                // case SyntaxKind.MinusAssign:
                //     return "-=";
                // case SyntaxKind.NotEqual:
                //     return "!=";
                // case SyntaxKind.ModuloAssign:
                //     return "%=";
                // case SyntaxKind.AndAssign:
                //     return "&=";
                // case SyntaxKind.DoubleAnd:
                //     return "&&";
                // case SyntaxKind.MultiplyAssign:
                //     return "*=";
                // case SyntaxKind.DivideAssign:
                //     return "/=";
                case SyntaxKind.DoubleColon:
                    return "::";
                // case SyntaxKind.NullCoalesce:
                //     return "??";
                // case SyntaxKind.XorAssign:
                //     return "^=";
                // case SyntaxKind.OrAssign:
                //     return "|=";
                // case SyntaxKind.DoubleOr:
                //     return "||";
                // case SyntaxKind.PlusAssign:
                //     return "+=";
                // case SyntaxKind.Increment:
                //     return "++";
                // case SyntaxKind.LessThanEqual:
                //     return "<=";
                // case SyntaxKind.LeftShift:
                //     return "<<";
                case SyntaxKind.Equals:
                    return "==";
                    // case SyntaxKind.GreaterThanEqual:
                    //     if (Buffer[0] == '=')
                    //     {
                    //         return "=>";
                    //     }
                    //     return ">=";
                    // case SyntaxKind.RightShift:
                    //     return ">>";


            }
        }
        else if (tokenLength == 3)
        {
            // switch (type)
            // {
            //     case SyntaxKind.LeftShiftAssign:
            //         return "<<=";
            //     case SyntaxKind.RightShiftAssign:
            //         return ">>=";
            // }
        }

        return base.GetTokenContent(type);
    }

    protected override SyntaxToken CreateToken(string content, SyntaxKind kind, RazorDiagnostic[] errors)
    {
        return SyntaxFactory.Token(kind, content, errors);
    }

    private StateResult Data()
    {
        if (SyntaxFacts.IsNewLine(CurrentCharacter))
        {
            // CSharp Spec §2.3.1
            var checkTwoCharNewline = CurrentCharacter == '\r';
            TakeCurrent();
            if (checkTwoCharNewline && CurrentCharacter == '\n')
            {
                TakeCurrent();
            }
            return Stay(EndToken(SyntaxKind.NewLine));
        }
        else if (SyntaxFacts.IsWhitespace(CurrentCharacter))
        {
            // CSharp Spec §2.3.3
            TakeUntil(c => !SyntaxFacts.IsWhitespace(c));
            return Stay(EndToken(SyntaxKind.Whitespace));
        }
        else if (SyntaxFacts.IsIdentifierStartCharacter(CurrentCharacter))
        {
            return Identifier();
        }
        else if (char.IsDigit(CurrentCharacter))
        {
            return NumericLiteral();
        }
        switch (CurrentCharacter)
        {
            case '@':
                return AtToken();
            case '\'':
                return Transition(CSharpTokenizerState.QuotedCharacterLiteral);
            case '"':
                return Transition(CSharpTokenizerState.QuotedStringLiteral);
            case '.':
                if (char.IsDigit(Peek()))
                {
                    return NumericLiteral();
                }
                return Stay(Single(SyntaxKind.Dot));
            case '/':
                TakeCurrent();
                if (CurrentCharacter == '/')
                {
                    TakeCurrent();
                    return SingleLineComment();
                }
                else if (CurrentCharacter == '*')
                {
                    TakeCurrent();
                    return Transition(CSharpTokenizerState.BlockComment);
                }
                else if (CurrentCharacter == '=')
                {
                    TakeCurrent();
                    return Stay(EndToken(SyntaxKind.CSharpOperator));
                }
                else
                {
                    return Stay(EndToken(SyntaxKind.CSharpOperator));
                }
            default:
                return Stay(Operator());
        }
    }

    private StateResult AtToken()
    {
        if (Peek() == '"')
        {
            return Transition(CSharpTokenizerState.VerbatimStringLiteral);
        }

        TakeCurrent();
        if (CurrentCharacter == '*')
        {
            return Transition(
                CSharpTokenizerState.AfterRazorCommentTransition,
                EndToken(SyntaxKind.RazorCommentTransition));
        }
        else if (CurrentCharacter == '@')
        {
            // Could be escaped comment transition
            return Transition(
                CSharpTokenizerState.EscapedRazorCommentTransition,
                EndToken(SyntaxKind.Transition));
        }

        return Stay(EndToken(SyntaxKind.Transition));
    }

    private StateResult EscapedRazorCommentTransition()
    {
        TakeCurrent();
        return Transition(CSharpTokenizerState.Data, EndToken(SyntaxKind.Transition));
    }

    private SyntaxToken Operator()
    {
        var curPosition = Source.Position;
        var token = _lexer.LexSyntax(curPosition);

        // Don't include trailing trivia; we handle that differently than Roslyn
        var finalPosition = curPosition + token.Span.Length;

        for (; curPosition < finalPosition; curPosition++)
        {
            TakeCurrent();
        }

        SyntaxKind kind;
        string content;
        switch (token.RawKind)
        {
            case (int)CSharpSyntaxKind.ExclamationToken:
                TakeTokenContent(token, out content);
                kind = SyntaxKind.Not;
                break;
            case (int)CSharpSyntaxKind.OpenParenToken:
                TakeTokenContent(token, out content);
                kind = SyntaxKind.LeftParenthesis;
                break;
            case (int)CSharpSyntaxKind.CloseParenToken:
                TakeTokenContent(token, out content);
                kind = SyntaxKind.RightParenthesis;
                break;
            case (int)CSharpSyntaxKind.CommaToken:
                TakeTokenContent(token, out content);
                kind = SyntaxKind.Comma;
                break;
            case (int)CSharpSyntaxKind.DotToken:
                TakeTokenContent(token, out content);
                kind = SyntaxKind.Dot;
                break;
            case (int)CSharpSyntaxKind.ColonColonToken:
                TakeTokenContent(token, out content);
                kind = SyntaxKind.DoubleColon;
                break;
            case (int)CSharpSyntaxKind.ColonToken:
                TakeTokenContent(token, out content);
                kind = SyntaxKind.Colon;
                break;
            case (int)CSharpSyntaxKind.OpenBraceToken:
                TakeTokenContent(token, out content);
                kind = SyntaxKind.LeftBrace;
                break;
            case (int)CSharpSyntaxKind.CloseBraceToken:
                TakeTokenContent(token, out content);
                kind = SyntaxKind.RightBrace;
                break;
            case (int)CSharpSyntaxKind.LessThanToken:
                TakeTokenContent(token, out content);
                kind = SyntaxKind.LessThan;
                break;
            case (int)CSharpSyntaxKind.GreaterThanToken:
                TakeTokenContent(token, out content);
                kind = SyntaxKind.GreaterThan;
                break;
            case (int)CSharpSyntaxKind.EqualsToken:
                TakeTokenContent(token, out content);
                kind = SyntaxKind.Assign;
                break;
            case (int)CSharpSyntaxKind.OpenBracketToken:
                TakeTokenContent(token, out content);
                kind = SyntaxKind.LeftBracket;
                break;
            case (int)CSharpSyntaxKind.CloseBracketToken:
                TakeTokenContent(token, out content);
                kind = SyntaxKind.RightBracket;
                break;
            case (int)CSharpSyntaxKind.QuestionToken:
                TakeTokenContent(token, out content);
                kind = SyntaxKind.QuestionMark;
                break;
            case (int)CSharpSyntaxKind.SemicolonToken:
                TakeTokenContent(token, out content);
                kind = SyntaxKind.Semicolon;
                break;
            case <= (int)CSharpSyntaxKind.GreaterThanGreaterThanGreaterThanEqualsToken and >= (int)CSharpSyntaxKind.TildeToken:
                TakeTokenContent(token, out content);
                kind = SyntaxKind.CSharpOperator;
                break;
            default:
                kind = SyntaxKind.Marker;
                content = Buffer.ToString();
                Buffer.Clear();
                break;
        }

        return EndToken(content, kind);

        void TakeTokenContent(CodeAnalysis.SyntaxToken token, out string content)
        {
            content = token.ValueText;
            Buffer.Clear();
        }
    }

    private StateResult TokenizedExpectedStringOrCharacterLiteral(CodeAnalysis.CSharp.SyntaxKind expectedCSharpTokenKind, SyntaxKind razorTokenKind, string expectedPrefix, string expectedPostFix)
    {
        var curPosition = Source.Position;
        var csharpToken = _lexer.LexSyntax(curPosition);
        // Don't include trailing trivia; we handle that differently than Roslyn
        var finalPosition = curPosition + csharpToken.Span.Length;

        for (; curPosition < finalPosition; curPosition++)
        {
            TakeCurrent();
        }

        // If the token is the expected kind and has the expected prefix or doesn't have the expected postfix, then it's unterminated.
        // This is a case like `"test` (which doesn't end in the expected postfix), or `"` (which ends in the expected postfix, but
        // exactly matches the expected prefix).
        if (CodeAnalysis.CSharpExtensions.IsKind(csharpToken, expectedCSharpTokenKind)
            && (csharpToken.Text == expectedPrefix || !csharpToken.Text.EndsWith(expectedPostFix, StringComparison.Ordinal)))
        {
            CurrentErrors.Add(
                RazorDiagnosticFactory.CreateParsing_UnterminatedStringLiteral(
                    new SourceSpan(CurrentStart, contentLength: expectedPrefix.Length /* " */)));
        }

        return Transition(CSharpTokenizerState.Data, EndToken(razorTokenKind));
    }

    // CSharp Spec §2.3.2
    private StateResult BlockComment()
    {
        TakeUntil(c => c == '*');
        if (EndOfFile)
        {
            CurrentErrors.Add(
                RazorDiagnosticFactory.CreateParsing_BlockCommentNotTerminated(
                    new SourceSpan(CurrentStart, contentLength: 1 /* end of file */)));

            return Transition(CSharpTokenizerState.Data, EndToken(SyntaxKind.CSharpComment));
        }
        if (CurrentCharacter == '*')
        {
            TakeCurrent();
            if (CurrentCharacter == '/')
            {
                TakeCurrent();
                return Transition(CSharpTokenizerState.Data, EndToken(SyntaxKind.CSharpComment));
            }
        }
        return Stay();
    }

    // CSharp Spec §2.3.2
    private StateResult SingleLineComment()
    {
        TakeUntil(c => SyntaxFacts.IsNewLine(c));
        return Stay(EndToken(SyntaxKind.CSharpComment));
    }

    // CSharp Spec §2.4.4
    private StateResult NumericLiteral()
    {
        var curPosition = Source.Position;
        var csharpToken = _lexer.LexSyntax(curPosition);
        // Don't include trailing trivia; we handle that differently than Roslyn
        var finalPosition = curPosition + csharpToken.Span.Length;

        for (; curPosition < finalPosition; curPosition++)
        {
            TakeCurrent();
        }

        return Transition(CSharpTokenizerState.Data, EndToken(SyntaxKind.NumericLiteral));
    }

    // CSharp Spec §2.4.2
    private StateResult Identifier()
    {
        Debug.Assert(SyntaxFacts.IsIdentifierStartCharacter(CurrentCharacter));
        TakeCurrent();
        TakeUntil(c => !SyntaxFacts.IsIdentifierPartCharacter(c));
        SyntaxToken token = null;
        if (HaveContent)
        {
            CSharpKeyword keyword;
            var type = SyntaxKind.Identifier;
            var tokenContent = Buffer.ToString();
            if (_keywords.TryGetValue(tokenContent, out keyword))
            {
                type = SyntaxKind.Keyword;
            }

            token = SyntaxFactory.Token(type, tokenContent);

            Buffer.Clear();
            CurrentErrors.Clear();
        }

        return Stay(token);
    }

    private StateResult Transition(CSharpTokenizerState state)
    {
        return Transition((int)state, result: null);
    }

    private StateResult Transition(CSharpTokenizerState state, SyntaxToken result)
    {
        return Transition((int)state, result);
    }

    private static bool IsRealLiteralSuffix(char character)
    {
        return character == 'F' ||
               character == 'f' ||
               character == 'D' ||
               character == 'd' ||
               character == 'M' ||
               character == 'm';
    }

    internal static CSharpKeyword? GetTokenKeyword(SyntaxToken token)
    {
        if (token != null && _keywords.TryGetValue(token.Content, out var keyword))
        {
            return keyword;
        }

        return null;
    }

    private enum CSharpTokenizerState
    {
        Data,
        BlockComment,
        QuotedCharacterLiteral,
        QuotedStringLiteral,
        VerbatimStringLiteral,

        // Razor Comments - need to be the same for HTML and CSharp
        AfterRazorCommentTransition = RazorCommentTokenizerState.AfterRazorCommentTransition,
        EscapedRazorCommentTransition = RazorCommentTokenizerState.EscapedRazorCommentTransition,
        RazorCommentBody = RazorCommentTokenizerState.RazorCommentBody,
        StarAfterRazorCommentBody = RazorCommentTokenizerState.StarAfterRazorCommentBody,
        AtTokenAfterRazorCommentBody = RazorCommentTokenizerState.AtTokenAfterRazorCommentBody,
    }
}
