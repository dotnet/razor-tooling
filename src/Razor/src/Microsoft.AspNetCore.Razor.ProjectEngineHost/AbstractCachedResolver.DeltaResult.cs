﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Diagnostics;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.AspNetCore.Razor.Serialization;
using Microsoft.AspNetCore.Razor.Utilities;

namespace Microsoft.AspNetCore.Razor;
internal abstract partial class AbstractCachedResolver<T>
{
    public record DeltaResult(
        bool IsDelta,
        int ResultId,
        ImmutableArray<Checksum> Added,
        ImmutableArray<Checksum> Removed)
    {
        public ImmutableArray<Checksum> Apply(ImmutableArray<Checksum> baseChecksums)
        {
            if (Added.Length == 0 && Removed.Length == 0)
            {
                return baseChecksums;
            }

            using var _ = ArrayBuilderPool<Checksum>.GetPooledObject(out var result);
            result.SetCapacityIfLarger(baseChecksums.Length + Added.Length - Removed.Length);

            result.AddRange(Added);
            result.AddRange(Delta.Compute(Removed, baseChecksums));

#if DEBUG
            // Ensure that there are no duplicate tag helpers in the result.
            using var pooledSet = HashSetPool<Checksum>.GetPooledObject();
            var set = pooledSet.Object;

            foreach (var item in result)
            {
                Debug.Assert(set.Add(item), $"{nameof(TagHelperDeltaResult)}.{nameof(Apply)} should not contain any duplicates!");
            }
#endif

            return result.DrainToImmutable();
        }
    }
}
