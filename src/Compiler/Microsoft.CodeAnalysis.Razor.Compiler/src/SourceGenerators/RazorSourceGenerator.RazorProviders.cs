﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Text;
using System.Threading;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Microsoft.NET.Sdk.Razor.SourceGenerators
{
    public partial class RazorSourceGenerator
    {
        private (RazorSourceGenerationOptions?, Diagnostic?) ComputeRazorSourceGeneratorOptions(((AnalyzerConfigOptionsProvider, ParseOptions), bool) pair, CancellationToken ct)
        {
            var ((options, parseOptions), isSuppressed) = pair;
            var globalOptions = options.GlobalOptions;
            
            if (isSuppressed)
            {
                return default;
            }

            Log.ComputeRazorSourceGeneratorOptions();

            globalOptions.TryGetValue("build_property.RazorConfiguration", out var configurationName);
            globalOptions.TryGetValue("build_property.RootNamespace", out var rootNamespace);
            globalOptions.TryGetValue("build_property.SupportLocalizedComponentNames", out var supportLocalizedComponentNames);
            globalOptions.TryGetValue("build_property.GenerateRazorMetadataSourceChecksumAttributes", out var generateMetadataSourceChecksumAttributes);

            Diagnostic? diagnostic = null;
            if (!globalOptions.TryGetValue("build_property.RazorLangVersion", out var razorLanguageVersionString) ||
                !RazorLanguageVersion.TryParse(razorLanguageVersionString, out var razorLanguageVersion))
            {
                diagnostic = Diagnostic.Create(
                    RazorDiagnostics.InvalidRazorLangVersionDescriptor,
                    Location.None,
                    razorLanguageVersionString);
                razorLanguageVersion = RazorLanguageVersion.Latest;
            }

            var razorConfiguration = new RazorConfiguration(razorLanguageVersion, configurationName ?? "default", Extensions: [], UseConsolidatedMvcViews: true, SuppressAddComponentParameter: false);

            var razorSourceGenerationOptions = new RazorSourceGenerationOptions()
            {
                Configuration = razorConfiguration,
                GenerateMetadataSourceChecksumAttributes = generateMetadataSourceChecksumAttributes == "true",
                RootNamespace = rootNamespace ?? "ASP",
                SupportLocalizedComponentNames = supportLocalizedComponentNames == "true",
                CSharpLanguageVersion = ((CSharpParseOptions)parseOptions).LanguageVersion,
                TestSuppressUniqueIds = _testSuppressUniqueIds,
            };

            return (razorSourceGenerationOptions, diagnostic);
        }

        private static (SourceGeneratorProjectItem?, Diagnostic?) ComputeProjectItems((AdditionalText, AnalyzerConfigOptionsProvider) pair, CancellationToken ct)
        {
            var (additionalText, globalOptions) = pair;
            var options = globalOptions.GetOptions(additionalText);

            if (!options.TryGetValue("build_metadata.AdditionalFiles.TargetPath", out var encodedRelativePath) ||
                string.IsNullOrWhiteSpace(encodedRelativePath))
            {
                var diagnostic = Diagnostic.Create(
                    RazorDiagnostics.TargetPathNotProvided,
                    Location.None,
                    additionalText.Path);
                return (null, diagnostic);
            }

            options.TryGetValue("build_metadata.AdditionalFiles.CssScope", out var cssScope);
            var relativePath = Encoding.UTF8.GetString(Convert.FromBase64String(encodedRelativePath));

            var projectItem = new SourceGeneratorProjectItem(
                basePath: "/",
                filePath: '/' + relativePath
                    .Replace(Path.DirectorySeparatorChar, '/')
                    .Replace("//", "/"),
                relativePhysicalPath: relativePath,
                fileKind: additionalText.Path.EndsWith(".razor", StringComparison.OrdinalIgnoreCase) ? FileKinds.Component : FileKinds.Legacy,
                additionalText: additionalText,
                cssScope: cssScope);
            return (projectItem, null);
        }
    }
}
