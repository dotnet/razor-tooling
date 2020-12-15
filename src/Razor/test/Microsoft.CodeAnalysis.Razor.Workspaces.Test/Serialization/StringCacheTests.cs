// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Xunit;

namespace Microsoft.VisualStudio.Editor.Razor
{
    public class StringCacheTests
    {
        [Fact]
        public void GetOrAdd_RetrievesFirstReference()
        {
            // Arrange
            var cache = new StringCache();
            // String format to prevent them from being RefEqual
            var str1 = $"stuff {1}";
            var str2 = $"stuff {1}";
            // Sanity check that these aren't already equal
            Assert.False(ReferenceEquals(str1, str2));

            // Act
            // Force a colleciton
            GC.Collect();
            _ = cache.GetOrAdd(str1);
            var result = cache.GetOrAdd(str2);

            // Assert
            Assert.Same(result, str1);
            Assert.False(ReferenceEquals(result, str2));
        }

        [Fact]
        public void GetOrAdd_NullReturnsNull(){
            // Arrange
            var cache = new StringCache();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => cache.GetOrAdd(null));
        }

        [Fact]
        public void GetOrAdd_DisposesReleasedReferencesOnExpand()
        {
            // Arrange
            var cache = new StringCache(1);

            // Act
            StringArea();

            // Force a collection
            GC.Collect();
            var str1 = $"{1}";
            var result = cache.GetOrAdd(str1);

            // Assert
            Assert.Same(result, str1);

            void StringArea()
            {
                cache.GetOrAdd($"{1}");
                cache.GetOrAdd($"{2}");
                cache.GetOrAdd($"{3}");
                cache.GetOrAdd($"{4}");
                cache.GetOrAdd($"{5}");
                cache.GetOrAdd($"{6}");
                cache.GetOrAdd($"{7}");
                cache.GetOrAdd($"{8}");
                cache.GetOrAdd($"{9}");
                cache.GetOrAdd($"{10}");
            }
        }
    }
}
