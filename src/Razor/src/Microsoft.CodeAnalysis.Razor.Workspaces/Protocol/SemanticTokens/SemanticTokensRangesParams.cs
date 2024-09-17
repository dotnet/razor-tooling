﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Text.Json.Serialization;

namespace Microsoft.CodeAnalysis.Razor.Workspaces.Protocol.SemanticTokens;

internal class SemanticTokensRangesParams : SemanticTokensParams
{
    [JsonPropertyName("ranges")]
    public required LspRange[] Ranges { get; set; }
}
