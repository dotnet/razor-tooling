﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading.Tasks;

namespace Microsoft.VisualStudio.Razor;

internal interface ISettingsPersistenceService
{
    ValueTask<bool> GetBooleanOrDefaultAsync(string name, bool defaultValue = false);
}
