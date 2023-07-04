﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Moq;
using Xunit;

namespace Microsoft.AspNetCore.Razor.Language.CodeGeneration;

public class DefaultDocumentWriterTest
{
    [Fact] // This test covers the whole process including actual hashing.
    public void WriteDocument_EndToEnd_WritesChecksumAndMarksAutoGenerated()
    {
        // Arrange
        var document = new DocumentIntermediateNode();

        var codeDocument = TestRazorCodeDocument.CreateEmpty();
        var options = RazorCodeGenerationOptions.CreateDefault();

        var target = CodeTarget.CreateDefault(codeDocument, options);
        var writer = new DefaultDocumentWriter(target, options);

        // Act
        var result = writer.WriteDocument(codeDocument, document);

        // Assert
        var csharp = result.GeneratedCode;
        Assert.Equal(
@"#pragma checksum ""test.cshtml"" ""{8829d00f-11b8-4213-878b-770e8597ac16}"" ""e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855""
// <auto-generated/>
#pragma warning disable 1591
#pragma warning restore 1591
",
            csharp,
            ignoreLineEndingDifferences: true);
    }

    [Fact]
    public void WriteDocument_SHA1_WritesChecksumAndMarksAutoGenerated()
    {
        // Arrange
        var checksumBytes = new byte[] { (byte)'t', (byte)'e', (byte)'s', (byte)'t', };

        var sourceDocument = Mock.Of<RazorSourceDocument>(d =>
            d.FilePath == "test.cshtml" &&
            d.GetChecksum() == checksumBytes &&
            d.GetChecksumAlgorithm() == "SHA1");

        var document = new DocumentIntermediateNode();

        var codeDocument = RazorCodeDocument.Create(sourceDocument);
        var options = RazorCodeGenerationOptions.CreateDefault();

        var target = CodeTarget.CreateDefault(codeDocument, options);
        var writer = new DefaultDocumentWriter(target, options);

        // Act
        var result = writer.WriteDocument(codeDocument, document);

        // Assert
        var csharp = result.GeneratedCode;
        Assert.Equal(
@"#pragma checksum ""test.cshtml"" ""{ff1816ec-aa5e-4d10-87f7-6f4963833460}"" ""74657374""
// <auto-generated/>
#pragma warning disable 1591
#pragma warning restore 1591
",
            csharp,
            ignoreLineEndingDifferences: true);
    }

    [Fact]
    public void WriteDocument_SHA256_WritesChecksumAndMarksAutoGenerated()
    {
        // Arrange
        var checksumBytes = new byte[] { (byte)'t', (byte)'e', (byte)'s', (byte)'t', };

        var sourceDocument = Mock.Of<RazorSourceDocument>(d =>
            d.FilePath == "test.cshtml" &&
            d.GetChecksum() == checksumBytes &&
            d.GetChecksumAlgorithm() == "SHA256");

        var document = new DocumentIntermediateNode();

        var codeDocument = RazorCodeDocument.Create(sourceDocument);
        var options = RazorCodeGenerationOptions.CreateDefault();

        var target = CodeTarget.CreateDefault(codeDocument, options);
        var writer = new DefaultDocumentWriter(target, options);

        // Act
        var result = writer.WriteDocument(codeDocument, document);

        // Assert
        var csharp = result.GeneratedCode;
        Assert.Equal(
@"#pragma checksum ""test.cshtml"" ""{8829d00f-11b8-4213-878b-770e8597ac16}"" ""74657374""
// <auto-generated/>
#pragma warning disable 1591
#pragma warning restore 1591
",
            csharp,
            ignoreLineEndingDifferences: true);
    }

    [Fact]
    public void WriteDocument_UnsupportedChecksumAlgorithm_Throws()
    {
        // Arrange
        var checksumBytes = new byte[] { (byte)'t', (byte)'e', (byte)'s', (byte)'t', };

        var sourceDocument = Mock.Of<RazorSourceDocument>(d =>
            d.FilePath == "test.cshtml" &&
            d.GetChecksum() == checksumBytes &&
            d.GetChecksumAlgorithm() == "SHA3");

        var document = new DocumentIntermediateNode();

        var codeDocument = RazorCodeDocument.Create(sourceDocument);
        var options = RazorCodeGenerationOptions.CreateDefault();

        var target = CodeTarget.CreateDefault(codeDocument, options);
        var writer = new DefaultDocumentWriter(target, options);

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() =>
        {
            var result = writer.WriteDocument(codeDocument, document);
        });
        Assert.Equal(
            "The hash algorithm 'SHA3' is not supported for checksum generation. Supported algorithms are: 'SHA1 SHA256'. " +
            "Set 'RazorCodeGenerationOptions.SuppressChecksum' to 'True' to suppress automatic checksum generation.",
            exception.Message);
    }



    [Fact]
    public void WriteDocument_Empty_SuppressChecksumTrue_DoesnotWriteChecksum()
    {
        // Arrange
        var document = new DocumentIntermediateNode();

        var codeDocument = TestRazorCodeDocument.CreateEmpty();
        var optionsBuilder = new DefaultRazorCodeGenerationOptionsBuilder(designTime: false)
        {
            SuppressChecksum = true
        };
        var options = optionsBuilder.Build();

        var target = CodeTarget.CreateDefault(codeDocument, options);
        var writer = new DefaultDocumentWriter(target, options);

        // Act
        var result = writer.WriteDocument(codeDocument, document);

        // Assert
        var csharp = result.GeneratedCode;
        Assert.Equal(
@"// <auto-generated/>
#pragma warning disable 1591
#pragma warning restore 1591
",
            csharp,
            ignoreLineEndingDifferences: true);
    }

    [Fact]
    public void WriteDocument_WritesNamespace()
    {
        // Arrange
        var document = new DocumentIntermediateNode();
        var builder = IntermediateNodeBuilder.Create(document);
        builder.Add(new NamespaceDeclarationIntermediateNode()
        {
            Content = "TestNamespace",
        });

        var codeDocument = TestRazorCodeDocument.CreateEmpty();
        var options = RazorCodeGenerationOptions.CreateDefault();

        var target = CodeTarget.CreateDefault(codeDocument, options);
        var writer = new DefaultDocumentWriter(target, options);

        // Act
        var result = writer.WriteDocument(codeDocument, document);

        // Assert
        var csharp = result.GeneratedCode;
        Assert.Equal(
@"#pragma checksum ""test.cshtml"" ""{8829d00f-11b8-4213-878b-770e8597ac16}"" ""e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855""
// <auto-generated/>
#pragma warning disable 1591
namespace TestNamespace
{
    #line hidden
}
#pragma warning restore 1591
",
            csharp,
            ignoreLineEndingDifferences: true);
    }

    [Fact]
    public void WriteDocument_WritesClass()
    {
        // Arrange
        var document = new DocumentIntermediateNode();
        var builder = IntermediateNodeBuilder.Create(document);
        builder.Add(new ClassDeclarationIntermediateNode()
        {
            Modifiers =
            {
                "internal"
            },
            BaseType = "TestBase",
            Interfaces = new List<string> { "IFoo", "IBar", },
            TypeParameters = new List<TypeParameter>
            {
                new TypeParameter() { ParameterName = "TKey", },
                new TypeParameter() { ParameterName = "TValue", },
            },
            ClassName = "TestClass",
        });

        var codeDocument = TestRazorCodeDocument.CreateEmpty();
        var options = RazorCodeGenerationOptions.CreateDefault();

        var target = CodeTarget.CreateDefault(codeDocument, options);
        var writer = new DefaultDocumentWriter(target, options);

        // Act
        var result = writer.WriteDocument(codeDocument, document);

        // Assert
        var csharp = result.GeneratedCode;
        Assert.Equal(
@"#pragma checksum ""test.cshtml"" ""{8829d00f-11b8-4213-878b-770e8597ac16}"" ""e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855""
// <auto-generated/>
#pragma warning disable 1591
internal class TestClass<TKey, TValue> : TestBase, IFoo, IBar
{
}
#pragma warning restore 1591
",
            csharp,
            ignoreLineEndingDifferences: true);
    }

    [Fact]
    public void WriteDocument_WithNullableContext_WritesClass()
    {
        // Arrange
        var document = new DocumentIntermediateNode();
        var builder = IntermediateNodeBuilder.Create(document);
        builder.Add(new ClassDeclarationIntermediateNode()
        {
            Modifiers =
            {
                "internal"
            },
            BaseType = "TestBase",
            Interfaces = new List<string> { "IFoo", "IBar", },
            TypeParameters = new List<TypeParameter>
            {
                new TypeParameter() { ParameterName = "TKey", },
                new TypeParameter() { ParameterName = "TValue", },
            },
            ClassName = "TestClass",
            Annotations =
            {
                [CommonAnnotations.NullableContext] = CommonAnnotations.NullableContext,
            },
        });

        var codeDocument = TestRazorCodeDocument.CreateEmpty();
        var options = RazorCodeGenerationOptions.CreateDefault();

        var target = CodeTarget.CreateDefault(codeDocument, options);
        var writer = new DefaultDocumentWriter(target, options);

        // Act
        var result = writer.WriteDocument(codeDocument, document);

        // Assert
        var csharp = result.GeneratedCode;
        Assert.Equal(
@"#pragma checksum ""test.cshtml"" ""{8829d00f-11b8-4213-878b-770e8597ac16}"" ""e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855""
// <auto-generated/>
#pragma warning disable 1591
#nullable restore
internal class TestClass<TKey, TValue> : TestBase, IFoo, IBar
#nullable disable
{
}
#pragma warning restore 1591
",
            csharp,
            ignoreLineEndingDifferences: true);
    }

    [Fact]
    public void WriteDocument_WritesClass_ConstrainedGenericTypeParameters()
    {
        // Arrange
        var document = new DocumentIntermediateNode();
        var builder = IntermediateNodeBuilder.Create(document);
        builder.Add(new ClassDeclarationIntermediateNode()
        {
            Modifiers =
                {
                    "internal"
                },
            BaseType = "TestBase",
            Interfaces = new List<string> { "IFoo", "IBar", },
            TypeParameters = new List<TypeParameter>
                {
                    new TypeParameter() { ParameterName = "TKey", Constraints = "where TKey : class" },
                    new TypeParameter() { ParameterName = "TValue", Constraints = "where TValue : class" },
                },
            ClassName = "TestClass",
        });

        var codeDocument = TestRazorCodeDocument.CreateEmpty();
        var options = RazorCodeGenerationOptions.CreateDefault();

        var target = CodeTarget.CreateDefault(codeDocument, options);
        var writer = new DefaultDocumentWriter(target, options);

        // Act
        var result = writer.WriteDocument(codeDocument, document);

        // Assert
        var csharp = result.GeneratedCode;
        Assert.Equal(
@"#pragma checksum ""test.cshtml"" ""{8829d00f-11b8-4213-878b-770e8597ac16}"" ""e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855""
// <auto-generated/>
#pragma warning disable 1591
internal class TestClass<TKey, TValue> : TestBase, IFoo, IBar
where TKey : class
where TValue : class
{
}
#pragma warning restore 1591
",
            csharp,
            ignoreLineEndingDifferences: true);
    }

    [Fact]
    public void WriteDocument_WritesMethod()
    {
        // Arrange
        var document = new DocumentIntermediateNode();
        var builder = IntermediateNodeBuilder.Create(document);
        builder.Add(new MethodDeclarationIntermediateNode()
        {
            Modifiers =
                {
                    "internal",
                    "virtual",
                    "async",
                },
            MethodName = "TestMethod",
            Parameters =
                {
                    new MethodParameter()
                    {
                        Modifiers =
                        {
                            "readonly",
                            "ref",
                        },
                        ParameterName = "a",
                        TypeName = "int",
                    },
                    new MethodParameter()
                    {
                        ParameterName = "b",
                        TypeName = "string",
                    }
                },
            ReturnType = "string",
        });

        var codeDocument = TestRazorCodeDocument.CreateEmpty();
        var options = RazorCodeGenerationOptions.CreateDefault();

        var target = CodeTarget.CreateDefault(codeDocument, options);
        var writer = new DefaultDocumentWriter(target, options);

        // Act
        var result = writer.WriteDocument(codeDocument, document);

        // Assert
        var csharp = result.GeneratedCode;
        Assert.Equal(
@"#pragma checksum ""test.cshtml"" ""{8829d00f-11b8-4213-878b-770e8597ac16}"" ""e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855""
// <auto-generated/>
#pragma warning disable 1591
#pragma warning disable 1998
internal virtual async string TestMethod(readonly ref int a, string b)
{
}
#pragma warning restore 1998
#pragma warning restore 1591
",
            csharp,
            ignoreLineEndingDifferences: true);
    }

    [Fact]
    public void WriteDocument_WritesField()
    {
        // Arrange
        var document = new DocumentIntermediateNode();
        var builder = IntermediateNodeBuilder.Create(document);
        builder.Add(new FieldDeclarationIntermediateNode()
        {
            Modifiers =
                {
                    "internal",
                    "readonly",
                },
            FieldName = "_foo",
            FieldType = "string",
        });

        var codeDocument = TestRazorCodeDocument.CreateEmpty();
        var options = RazorCodeGenerationOptions.CreateDefault();

        var target = CodeTarget.CreateDefault(codeDocument, options);
        var writer = new DefaultDocumentWriter(target, options);

        // Act
        var result = writer.WriteDocument(codeDocument, document);

        // Assert
        var csharp = result.GeneratedCode;
        Assert.Equal(
@"#pragma checksum ""test.cshtml"" ""{8829d00f-11b8-4213-878b-770e8597ac16}"" ""e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855""
// <auto-generated/>
#pragma warning disable 1591
internal readonly string _foo;
#pragma warning restore 1591
",
            csharp,
            ignoreLineEndingDifferences: true);
    }

    [Fact]
    public void WriteDocument_WritesProperty()
    {
        // Arrange
        var document = new DocumentIntermediateNode();
        var builder = IntermediateNodeBuilder.Create(document);
        builder.Add(new PropertyDeclarationIntermediateNode()
        {
            Modifiers =
                {
                    "internal",
                    "virtual",
                },
            PropertyName = "Foo",
            PropertyType = "string",
        });

        var codeDocument = TestRazorCodeDocument.CreateEmpty();
        var options = RazorCodeGenerationOptions.CreateDefault();

        var target = CodeTarget.CreateDefault(codeDocument, options);
        var writer = new DefaultDocumentWriter(target, options);

        // Act
        var result = writer.WriteDocument(codeDocument, document);

        // Assert
        var csharp = result.GeneratedCode;
        Assert.Equal(
@"#pragma checksum ""test.cshtml"" ""{8829d00f-11b8-4213-878b-770e8597ac16}"" ""e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855""
// <auto-generated/>
#pragma warning disable 1591
internal virtual string Foo { get; set; }
#pragma warning restore 1591
",
            csharp,
            ignoreLineEndingDifferences: true);
    }
}
