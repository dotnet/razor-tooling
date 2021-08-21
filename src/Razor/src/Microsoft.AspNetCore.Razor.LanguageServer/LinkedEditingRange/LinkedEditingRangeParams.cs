﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using MediatR;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace Microsoft.AspNetCore.Razor.LanguageServer.LinkedEditingRange
{
    internal class LinkedEditingRangeParams : TextDocumentPositionParams, IRequest<LinkedEditingRanges>, IBaseRequest
    {
    }
}
