﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Microsoft.AspNetCore.Razor.LanguageServer.Semantic
{
    public struct SemanticTokensOrSemanticTokensEdits
    {
        public SemanticTokensOrSemanticTokensEdits(SemanticTokensEdits semanticTokensEdits)
        {
            SemanticTokensEdits = semanticTokensEdits;
            SemanticTokens = null;
        }

        public SemanticTokensOrSemanticTokensEdits(SemanticTokens semanticTokens)
        {
            SemanticTokensEdits = null;
            SemanticTokens = semanticTokens;
        }

        public bool IsSemanticTokens => SemanticTokens != null;
        public SemanticTokens SemanticTokens { get; }

        public bool IsSemanticTokensEdits => SemanticTokensEdits != null;
        public SemanticTokensEdits SemanticTokensEdits { get; }

        public static implicit operator SemanticTokensOrSemanticTokensEdits(SemanticTokensEdits semanticTokensEdits)
        {
            return new SemanticTokensOrSemanticTokensEdits(semanticTokensEdits);
        }

        public static implicit operator SemanticTokensOrSemanticTokensEdits(SemanticTokens semanticTokens)
        {
            return new SemanticTokensOrSemanticTokensEdits(semanticTokens);
        }
    }
}
