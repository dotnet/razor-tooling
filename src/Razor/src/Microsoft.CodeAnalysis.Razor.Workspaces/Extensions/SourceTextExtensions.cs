﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Buffers;
using System.Diagnostics;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.CodeAnalysis.Text;

internal static class SourceTextExtensions
{
    /// <summary>
    /// Gets the minimal range of text that changed between the two versions.
    /// </summary>
    public static TextChangeRange GetEncompassingTextChangeRange(this SourceText newText, SourceText oldText)
    {
        ArgHelper.ThrowIfNull(newText);
        ArgHelper.ThrowIfNull(oldText);

        var ranges = newText.GetChangeRanges(oldText);
        if (ranges.Count == 0)
        {
            return default;
        }

        // simple case.
        if (ranges.Count == 1)
        {
            return ranges[0];
        }

        return TextChangeRange.Collapse(ranges);
    }

    public static LinePosition GetLinePosition(this SourceText text, int position)
    {
        ArgHelper.ThrowIfNull(text);

        return text.Lines.GetLinePosition(position);
    }

    public static LinePositionSpan GetLinePositionSpan(this SourceText text, TextSpan span)
    {
        ArgHelper.ThrowIfNull(text);

        return text.Lines.GetLinePositionSpan(span);
    }

    public static LinePositionSpan GetLinePositionSpan(this SourceText text, SourceSpan span)
        => text.GetLinePositionSpan(span.ToTextSpan());

    public static string GetSubTextString(this SourceText text, TextSpan span)
    {
        ArgHelper.ThrowIfNull(text);

        using var _ = ArrayPool<char>.Shared.GetPooledArray(span.Length, out var charBuffer);

        text.CopyTo(span.Start, charBuffer, 0, span.Length);
        return new string(charBuffer, 0, span.Length);
    }

    public static bool NonWhitespaceContentEquals(this SourceText text, SourceText other)
    {
        ArgHelper.ThrowIfNull(text);
        ArgHelper.ThrowIfNull(other);

        var i = 0;
        var j = 0;
        while (i < text.Length && j < other.Length)
        {
            if (char.IsWhiteSpace(text[i]))
            {
                i++;
                continue;
            }
            else if (char.IsWhiteSpace(other[j]))
            {
                j++;
                continue;
            }
            else if (text[i] != other[j])
            {
                return false;
            }

            i++;
            j++;
        }

        while (i < text.Length && char.IsWhiteSpace(text[i]))
        {
            i++;
        }

        while (j < other.Length && char.IsWhiteSpace(other[j]))
        {
            j++;
        }

        return i == text.Length && j == other.Length;
    }

    public static bool TryGetFirstNonWhitespaceOffset(this SourceText text, out int offset)
        => text.TryGetFirstNonWhitespaceOffset(new TextSpan(0, text.Length), out offset);

    public static bool TryGetFirstNonWhitespaceOffset(this SourceText text, TextSpan span, out int offset)
    {
        ArgHelper.ThrowIfNull(text);

        for (var i = span.Start; i < span.End; i++)
        {
            if (!char.IsWhiteSpace(text[i]))
            {
                offset = i - span.Start;
                return true;
            }
        }

        offset = -1;
        return false;
    }

    public static bool TryGetFirstNonWhitespaceOffset(this SourceText text, TextSpan span, out int offset, out int newLineCount)
    {
        ArgHelper.ThrowIfNull(text);

        newLineCount = 0;

        for (var i = span.Start; i < span.End; i++)
        {
            if (!char.IsWhiteSpace(text[i]))
            {
                offset = i - span.Start;
                return true;
            }
            else if (text[i] == '\n')
            {
                newLineCount++;
            }
        }

        offset = -1;
        newLineCount = -1;
        return false;
    }

    /// <summary>
    ///  <para>
    ///   Given the source text and the current span, we start at the ending span location and iterate towards the start
    ///   until we've reached a non-whitespace character.
    ///  </para>
    ///  <para>
    ///   For instance, "  abcdef  " would have a last non-whitespace offset of 7 to correspond to the character 'f'.
    ///  </para>
    /// </summary>
    public static bool TryGetLastNonWhitespaceOffset(this SourceText text, TextSpan span, out int offset)
    {
        ArgHelper.ThrowIfNull(text);

        // If the span is at the end of the document, it's common for the "End" to represent 1 past the end of the source
        var indexableSpanEnd = Math.Min(span.End, text.Length - 1);

        for (var i = indexableSpanEnd; i >= span.Start; i--)
        {
            if (!char.IsWhiteSpace(text[i]))
            {
                offset = i - span.Start;
                return true;
            }
        }

        offset = -1;
        return false;
    }

    public static bool TryGetAbsoluteIndex(this SourceText text, LinePosition position, out int absoluteIndex)
        => text.TryGetAbsoluteIndex(position.Line, position.Character, out absoluteIndex);

    public static bool TryGetAbsoluteIndex(this SourceText text, LinePosition position, ILogger logger, out int absoluteIndex)
        => text.TryGetAbsoluteIndex(position.Line, position.Character, logger, out absoluteIndex);

    public static bool TryGetAbsoluteIndex(this SourceText text, Position position, out int absoluteIndex)
        => text.TryGetAbsoluteIndex(position.Line, position.Character, out absoluteIndex);

    public static bool TryGetAbsoluteIndex(this SourceText text, Position position, ILogger logger, out int absoluteIndex)
        => text.TryGetAbsoluteIndex(position.Line, position.Character, logger, out absoluteIndex);

    public static bool TryGetAbsoluteIndex(this SourceText text, int line, int character, out int absoluteIndex)
        => text.TryGetAbsoluteIndex(line, character, logger: null, out absoluteIndex);

    public static bool TryGetAbsoluteIndex(this SourceText text, int line, int character, ILogger? logger, out int absoluteIndex)
    {
        absoluteIndex = 0;
        var lineCount = text.Lines.Count;

        if (line > lineCount || (line == lineCount && character > 0))
        {
            if (logger is not null)
            {
                var errorMessage = SR.FormatPositionLine_Outside_Range(line, nameof(text), text.Lines.Count);
                logger.LogError(errorMessage);
                Debug.Fail(errorMessage);
            }

            return false;
        }

        // LSP spec allowed a Range to end one line past the end, and character 0. SourceText does not, so we adjust to the final char position
        if (line == lineCount)
        {
            absoluteIndex = text.Length;
            return true;
        }

        var sourceLine = text.Lines[line];
        var lineLengthIncludingLineBreak = sourceLine.SpanIncludingLineBreak.Length;
        if (character > lineLengthIncludingLineBreak)
        {
            if (logger is not null)
            {
                var errorMessage = SR.FormatPositionCharacter_Outside_Range(character, nameof(text), lineLengthIncludingLineBreak);
                logger.LogError(errorMessage);
                Debug.Fail(errorMessage);
            }

            return false;
        }

        absoluteIndex = sourceLine.Start + character;
        return true;
    }

    public static int GetRequiredAbsoluteIndex(this SourceText text, LinePosition position)
        => text.GetRequiredAbsoluteIndex(position.Line, position.Character);

    public static int GetRequiredAbsoluteIndex(this SourceText text, LinePosition position, ILogger logger)
        => text.GetRequiredAbsoluteIndex(position.Line, position.Character, logger);

    public static int GetRequiredAbsoluteIndex(this SourceText text, Position position)
        => text.GetRequiredAbsoluteIndex(position.Line, position.Character);

    public static int GetRequiredAbsoluteIndex(this SourceText text, Position position, ILogger logger)
        => text.GetRequiredAbsoluteIndex(position.Line, position.Character, logger);

    public static int GetRequiredAbsoluteIndex(this SourceText text, int line, int character)
        => text.TryGetAbsoluteIndex(line, character, out var absolutionPosition)
            ? absolutionPosition
            : ThrowHelper.ThrowInvalidOperationException<int>($"({line},{character}) matches or exceeds SourceText boundary {text.Lines.Count}.");

    public static int GetRequiredAbsoluteIndex(this SourceText text, int line, int character, ILogger logger)
        => text.TryGetAbsoluteIndex(line, character, logger, out var absolutionPosition)
            ? absolutionPosition
            : ThrowHelper.ThrowInvalidOperationException<int>($"({line},{character}) matches or exceeds SourceText boundary {text.Lines.Count}.");

    public static TextSpan GetTextSpan(this SourceText text, int startLine, int startCharacter, int endLine, int endCharacter)
    {
        ArgHelper.ThrowIfNull(text);

        var start = GetAbsoluteIndex(text, startLine, startCharacter, "Start");
        var end = GetAbsoluteIndex(text, endLine, endCharacter, "End");

        var length = end - start;
        if (length < 0)
        {
            return ThrowHelper.ThrowInvalidOperationException<TextSpan>($"({startLine},{startCharacter})-({endLine},{endCharacter}) resolved to zero or negative length.");
        }

        return new TextSpan(start, length);

        static int GetAbsoluteIndex(SourceText text, int line, int character, string name)
        {
            return text.TryGetAbsoluteIndex(line, character, out var absolutePosition)
                ? absolutePosition
                : ThrowHelper.ThrowInvalidOperationException<int>($"{name}: ({line},{character}) matches or exceeds SourceText boundary {text.Lines.Count}.");
        }
    }
}
