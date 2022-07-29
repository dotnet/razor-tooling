﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.AspNetCore.Razor.LanguageServer
{
    internal class DefaultLanguageServerFeatureOptions : LanguageServerFeatureOptions
    {
        public override bool SupportsFileManipulation { get; } = true;

        public override string ProjectConfigurationFileName { get; } = LanguageServerConstants.DefaultProjectConfigurationFile;

        public override string CSharpVirtualDocumentSuffix => ".ide.g.cs";

        public override string HtmlVirtualDocumentSuffix => "__virtual.html";

        public override bool SingleServerCompletionSupport { get; } = false;
    }
}
