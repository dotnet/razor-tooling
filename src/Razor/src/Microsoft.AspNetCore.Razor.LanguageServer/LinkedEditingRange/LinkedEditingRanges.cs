﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable enable

using Newtonsoft.Json;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace Microsoft.AspNetCore.Razor.LanguageServer.LinkedEditingRange
{
    internal class LinkedEditingRanges
    {
        [JsonProperty("ranges")]
        public Range[]? Ranges { get; set; }

        [JsonProperty("wordPattern")]
        public string? WordPattern { get; set; }
    }
}
