﻿#pragma checksum "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/InlineBlocks.cshtml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "d7855bb65d3e6db02c3a0fb02972366eb41151f780bf5b52f5c72a44a4591587"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(Microsoft.AspNetCore.Razor.Language.IntegrationTests.TestFiles.TestFiles_IntegrationTests_CodeGenerationIntegrationTest_InlineBlocks_Runtime), @"default", @"/TestFiles/IntegrationTests/CodeGenerationIntegrationTest/InlineBlocks.cshtml")]
namespace Microsoft.AspNetCore.Razor.Language.IntegrationTests.TestFiles;
#line hidden
[global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA256", @"d7855bb65d3e6db02c3a0fb02972366eb41151f780bf5b52f5c72a44a4591587", @"/TestFiles/IntegrationTests/CodeGenerationIntegrationTest/InlineBlocks.cshtml")]
public class TestFiles_IntegrationTests_CodeGenerationIntegrationTest_InlineBlocks_Runtime
{
    #pragma warning disable 1998
    public async System.Threading.Tasks.Task ExecuteAsync()
    {
        WriteLiteral("(string link) {\r\n    <a");
        BeginWriteAttribute("href", " href=\"", 36, "\"", 94, 1);
        WriteAttributeValue("", 43, new Microsoft.AspNetCore.Mvc.Razor.HelperResult(async(__razor_attribute_value_writer) => {
            PushWriter(__razor_attribute_value_writer);
#nullable restore
#line 2 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/InlineBlocks.cshtml"
              if(link != null) { 

#line default
#line hidden
#nullable disable
#nullable restore
#line (2,34)-(2,38) 6 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/InlineBlocks.cshtml"
Write(link);

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/InlineBlocks.cshtml"
                                       } else { 

#line default
#line hidden
#nullable disable
            WriteLiteral("#");
#nullable restore
#line 2 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/InlineBlocks.cshtml"
                                                               }

#line default
#line hidden
#nullable disable
            PopWriter();
        }
        ), 43, 51, false);
        EndWriteAttribute();
        WriteLiteral(" />\r\n}");
    }
    #pragma warning restore 1998
}
#pragma warning restore 1591
