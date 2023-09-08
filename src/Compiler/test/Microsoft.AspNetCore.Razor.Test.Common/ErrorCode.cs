﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.CodeAnalysis;

/// <summary>
/// Keep names and codes in sync with Roslyn's ErrorCode.cs. Add as necessary.
/// </summary>
public enum ErrorCode
{
    ERR_NameNotInContext = 103,
    WRN_UnreferencedField = 169,
    ERR_SingleTypeNameNotFound = 246,
    ERR_CantInferMethTypeArgs = 411,
    ERR_BadArgType = 1503,
    WRN_AsyncLacksAwaits = 1998,
    WRN_UninitializedNonNullableField = 8618,
}
