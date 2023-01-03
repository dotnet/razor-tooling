﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Common;

internal class ProjectEngineFactory_1_0 : ProjectEngineFactory
{
    protected override string AssemblyName { get; } = "Microsoft.AspNetCore.Mvc.Razor.Extensions.Version1_X";

    public override RazorProjectEngine Create(
        RazorConfiguration configuration,
        RazorProjectFileSystem fileSystem,
        Action<RazorProjectEngineBuilder> configure) => Create(configuration, fileSystem, configure, registerCompilerFeatures: false);
}
