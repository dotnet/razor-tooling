// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using BenchmarkDotNet.Attributes;

namespace Microsoft.AspNetCore.Razor.Microbenchmarks.Generator;
internal class ParamsAllExceptDebugAttribute
#if DEBUG
    : ParamsAttribute
#else
    : ParamsAllValuesAttribute
#endif

{
    internal ParamsAllExceptDebugAttribute(params object[] args)
#if DEBUG
        : base(args[0])
#else
        : base()
#endif
    {

    }
}
