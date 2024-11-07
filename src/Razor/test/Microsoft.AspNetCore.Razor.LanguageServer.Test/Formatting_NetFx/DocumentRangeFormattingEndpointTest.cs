﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Formatting;

public class DocumentRangeFormattingEndpointTest(ITestOutputHelper testOutput) : FormattingLanguageServerTestBase(testOutput)
{
    [Fact]
    public async Task Handle_FormattingEnabled_InvokesFormattingService()
    {
        // Arrange
        var codeDocument = TestRazorCodeDocument.CreateEmpty();
        var uri = new Uri("file://path/test.razor");

        var documentContext = CreateDocumentContext(uri, codeDocument);
        var formattingService = new DummyRazorFormattingService();

        var htmlFormatter = new TestHtmlFormatter();
        var optionsMonitor = GetOptionsMonitor(enableFormatting: true);
        var endpoint = new DocumentRangeFormattingEndpoint(
            formattingService, htmlFormatter, optionsMonitor);
        var @params = new DocumentRangeFormattingParams()
        {
            TextDocument = new TextDocumentIdentifier { Uri = uri, },
            Options = new FormattingOptions(),
            Range = VsLspFactory.DefaultRange
        };
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(@params, requestContext, DisposalToken);

        // Assert
        Assert.NotNull(result);
        Assert.True(formattingService.Called);
    }

    [Fact]
    public async Task Handle_DocumentNotFound_ReturnsNull()
    {
        // Arrange
        var formattingService = new DummyRazorFormattingService();
        var optionsMonitor = GetOptionsMonitor(enableFormatting: true);
        var htmlFormatter = new TestHtmlFormatter();
        var endpoint = new DocumentRangeFormattingEndpoint(formattingService, htmlFormatter, optionsMonitor);
        var uri = new Uri("file://path/test.razor");
        var @params = new DocumentRangeFormattingParams()
        {
            TextDocument = new TextDocumentIdentifier { Uri = uri, }
        };
        var requestContext = CreateRazorRequestContext(documentContext: null);

        // Act
        var result = await endpoint.HandleRequestAsync(@params, requestContext, DisposalToken);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Handle_UnsupportedCodeDocument_ReturnsNull()
    {
        // Arrange
        var codeDocument = TestRazorCodeDocument.CreateEmpty();
        codeDocument.SetUnsupported();
        var uri = new Uri("file://path/test.razor");

        var documentContext = CreateDocumentContext(uri, codeDocument);
        var formattingService = new DummyRazorFormattingService();
        var optionsMonitor = GetOptionsMonitor(enableFormatting: true);
        var htmlFormatter = new TestHtmlFormatter();
        var endpoint = new DocumentRangeFormattingEndpoint(formattingService, htmlFormatter, optionsMonitor);
        var @params = new DocumentRangeFormattingParams()
        {
            TextDocument = new TextDocumentIdentifier { Uri = uri, },
        };
        var requestContext = CreateRazorRequestContext(documentContext);

        // Act
        var result = await endpoint.HandleRequestAsync(@params, requestContext, DisposalToken);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Handle_FormattingDisabled_ReturnsNull()
    {
        // Arrange
        var formattingService = new DummyRazorFormattingService();
        var optionsMonitor = GetOptionsMonitor(enableFormatting: false);
        var htmlFormatter = new TestHtmlFormatter();
        var endpoint = new DocumentRangeFormattingEndpoint(formattingService, htmlFormatter, optionsMonitor);
        var @params = new DocumentRangeFormattingParams();
        var requestContext = CreateRazorRequestContext(documentContext: null);

        // Act
        var result = await endpoint.HandleRequestAsync(@params, requestContext, DisposalToken);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Handle_FormattingOnPasteDisabled_ReturnsNull()
    {
        // Arrange
        var formattingService = new DummyRazorFormattingService();
        var optionsMonitor = GetOptionsMonitor(formatOnPaste: false);
        var htmlFormatter = new TestHtmlFormatter();
        var endpoint = new DocumentRangeFormattingEndpoint(formattingService, htmlFormatter, optionsMonitor);
        var bytes = Encoding.UTF8.GetBytes("\"True\"");
        var reader = new Utf8JsonReader(bytes);
        var @params = new DocumentRangeFormattingParams()
        {
            Options = new()
            {
                OtherOptions = new()
                {
                    { "fromPaste", JsonElement.ParseValue(ref reader) }
                }
            }
        };

        var requestContext = CreateRazorRequestContext(documentContext: null);

        // Act
        var result = await endpoint.HandleRequestAsync(@params, requestContext, DisposalToken);

        // Assert
        Assert.Null(result);
    }
}
