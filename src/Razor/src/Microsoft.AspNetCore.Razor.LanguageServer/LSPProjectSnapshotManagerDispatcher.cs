﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable enable

using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.AspNetCore.Razor.LanguageServer
{
    internal class LSPProjectSnapshotManagerDispatcher : ProjectSnapshotManagerDispatcherBase
    {
        private const string ThreadName = "Razor." + nameof(LSPProjectSnapshotManagerDispatcher);

        public LSPProjectSnapshotManagerDispatcher() : base(ThreadName)
        {
        }
    }
}
