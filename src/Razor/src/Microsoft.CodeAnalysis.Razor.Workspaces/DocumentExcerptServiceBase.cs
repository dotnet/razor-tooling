﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Classification;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.Razor
{
    internal abstract class DocumentExcerptServiceBase : IRazorDocumentExcerptService
    {
        public async Task<RazorExcerptResult?> TryExcerptAsync(
                        Document document,
                        TextSpan span,
                        RazorExcerptMode mode,
                        CancellationToken cancellationToken)
        {
            var result = await TryGetExcerptInternalAsync(document, span, (ExcerptModeInternal)mode, cancellationToken).ConfigureAwait(false);
            return result?.ToExcerptResult();
        }

        internal abstract Task<ExcerptResultInternal?> TryGetExcerptInternalAsync(
                Document document,
                TextSpan span,
                ExcerptModeInternal mode,
                CancellationToken cancellationToken);

        protected TextSpan ChooseExcerptSpan(SourceText text, TextSpan span, ExcerptModeInternal mode)
        {
            var startLine = text.Lines.GetLineFromPosition(span.Start);
            var endLine = text.Lines.GetLineFromPosition(span.End);

            // If we're showing a single line then this will do. Otherwise expand the range by 3 in
            // each direction (if possible).
            if (mode == ExcerptModeInternal.Tooltip)
            {
                var startIndex = Math.Max(startLine.LineNumber - 3, 0);
                startLine = text.Lines[startIndex];

                var endIndex = Math.Min(endLine.LineNumber + 3, text.Lines.Count - 1);
                endLine = text.Lines[endIndex];
            }

            return new TextSpan(startLine.Start, endLine.End - startLine.Start);
        }

        // We have IVT access to the Roslyn APIs for product code, but not for testing.
        public enum ExcerptModeInternal
        {
            SingleLine = RazorExcerptMode.SingleLine,
            Tooltip = RazorExcerptMode.Tooltip,
        }

        // We have IVT access to the Roslyn APIs for product code, but not for testing.
        public readonly struct ExcerptResultInternal
        {
            public readonly SourceText Content;

            public readonly TextSpan MappedSpan;

            public readonly ImmutableArray<ClassifiedSpan> ClassifiedSpans;

            public readonly Document Document;

            public readonly TextSpan Span;

            public ExcerptResultInternal(
                SourceText content,
                TextSpan mappedSpan,
                ImmutableArray<ClassifiedSpan> classifiedSpans,
                Document document,
                TextSpan span)
            {
                Content = content;
                MappedSpan = mappedSpan;
                ClassifiedSpans = classifiedSpans;
                Document = document;
                Span = span;
            }

            public RazorExcerptResult ToExcerptResult()
            {
                return new RazorExcerptResult(Content, MappedSpan, ClassifiedSpans, Document, Span);
            }
        }
    }
}
