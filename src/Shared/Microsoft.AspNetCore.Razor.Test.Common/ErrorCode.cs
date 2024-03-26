﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.CodeAnalysis;

/// <summary>
/// Keep names and codes in sync with Roslyn's ErrorCode.cs. Add as necessary.
/// </summary>
public enum ErrorCode
{
    ERR_NameNotInContext = 103,
    ERR_BadSKunknown = 119,
    ERR_ObjectRequired = 120,
    WRN_UnreferencedField = 169,
    ERR_SingleTypeNameNotFound = 246,
    ERR_CantInferMethTypeArgs = 411,
    WRN_UnreferencedFieldAssg = 414,
    ERR_SyntaxError = 1003,
    ERR_BadArgCount = 1501,
    ERR_BadArgType = 1503,
    WRN_AsyncLacksAwaits = 1998,
    ERR_NoCorrespondingArgument = 7036,
    WRN_NullReferenceReceiver = 8602,
    WRN_UninitializedNonNullableField = 8618,
    WRN_MissingNonNullTypesContextForAnnotationInGeneratedCode = 8669,
}
