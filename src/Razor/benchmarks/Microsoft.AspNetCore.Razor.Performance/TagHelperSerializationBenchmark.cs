﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using BenchmarkDotNet.Attributes;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Razor.Serialization;
using Newtonsoft.Json;

namespace Microsoft.AspNetCore.Razor.Performance
{
    public class TagHelperSerializationBenchmark : TagHelperBenchmarkBase
    {
        [Benchmark(Description = "Razor TagHelper Roundtrip Serialization")]
        public void TagHelper_Serialization_RoundTrip()
        {
            // Serialize back to json.
            MemoryStream originalStream;
            using (originalStream = new MemoryStream())
            using (var writer = new StreamWriter(originalStream, Encoding.UTF8, bufferSize: 4096))
            {
                DefaultSerializer.Serialize(writer, DefaultTagHelpers);
            }

            IReadOnlyList<TagHelperDescriptor> reDeserializedTagHelpers;
            var stream = new MemoryStream(originalStream.GetBuffer());
            using (stream)
            using (var reader = new JsonTextReader(new StreamReader(stream)))
            {
                reDeserializedTagHelpers = DefaultSerializer.Deserialize<IReadOnlyList<TagHelperDescriptor>>(reader);
            }
        }

        [Benchmark(Description = "Razor TagHelper Serialization")]
        public void TagHelper_Serialization()
        {
            using (var stream = new MemoryStream())
            using (var writer = new StreamWriter(stream, Encoding.UTF8, bufferSize: 4096))
            {
                DefaultSerializer.Serialize(writer, DefaultTagHelpers);
            }
        }

        [Benchmark(Description = "Razor TagHelper Deserialization")]
        public void TagHelper_Deserialization()
        {
            // Deserialize from json file.
            IReadOnlyList<TagHelperDescriptor> tagHelpers;
            using (var stream = new MemoryStream(_tagHelperBuffer))
            using (var reader = new JsonTextReader(new StreamReader(stream)))
            {
                tagHelpers = DefaultSerializer.Deserialize<IReadOnlyList<TagHelperDescriptor>>(reader);
            }
        }
    }
}
