﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Reflection;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.CodeAnalysis.Razor.Workspaces;

[ExportCustomProjectEngineFactory("MVC-3.0", SupportsSerialization = true)]
internal class LegacyProjectEngineFactory_3_0 : IProjectEngineFactory
{
    private const string AssemblyName = "Microsoft.CodeAnalysis.Razor.Compiler.Mvc";
    public RazorProjectEngine Create(RazorConfiguration configuration, RazorProjectFileSystem fileSystem, Action<RazorProjectEngineBuilder> configure)
    {
        // Rewrite the assembly name into a full name just like this one, but with the name of the MVC design time assembly.
        var assemblyName = new AssemblyName(typeof(RazorProjectEngine).Assembly.FullName)
        {
            Name = AssemblyName
        };

        var extension = new AssemblyExtension(configuration.ConfigurationName, Assembly.Load(assemblyName));
        var initializer = extension.CreateInitializer();

        return RazorProjectEngine.Create(configuration, fileSystem, b =>
        {
            CompilerFeatures.Register(b);

            initializer.Initialize(b);
            configure?.Invoke(b);
        });
    }
}