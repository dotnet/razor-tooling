﻿#pragma checksum "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/InlineBlocks.cshtml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "d7855bb65d3e6db02c3a0fb02972366eb41151f780bf5b52f5c72a44a4591587"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.TestFiles_IntegrationTests_CodeGenerationIntegrationTest_InlineBlocks), @"mvc.1.0.view", @"/TestFiles/IntegrationTests/CodeGenerationIntegrationTest/InlineBlocks.cshtml")]
namespace AspNetCore
{
    #line default
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Linq;
    using global::System.Threading.Tasks;
    using global::Microsoft.AspNetCore.Mvc;
    using global::Microsoft.AspNetCore.Mvc.Rendering;
    using global::Microsoft.AspNetCore.Mvc.ViewFeatures;
    #line default
    #line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"Sha256", @"d7855bb65d3e6db02c3a0fb02972366eb41151f780bf5b52f5c72a44a4591587", @"/TestFiles/IntegrationTests/CodeGenerationIntegrationTest/InlineBlocks.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemMetadataAttribute("Identifier", "/TestFiles/IntegrationTests/CodeGenerationIntegrationTest/InlineBlocks.cshtml")]
    [global::System.Runtime.CompilerServices.CreateNewOnMetadataUpdateAttribute]
    #nullable restore
    public class TestFiles_IntegrationTests_CodeGenerationIntegrationTest_InlineBlocks : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    #nullable disable
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("(string link) {\r\n    <a");
            BeginWriteAttribute("href", " href=\"", 36, "\"", 94, 1);
            WriteAttributeValue("", 43, new Microsoft.AspNetCore.Mvc.Razor.HelperResult(async(__razor_attribute_value_writer) => {
                PushWriter(__razor_attribute_value_writer);
#nullable restore
#line (2,15)-(2,34) "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/InlineBlocks.cshtml"
if(link != null) { 

#line default
#line hidden
#nullable disable
                Write(
#nullable restore
#line (2,35)-(2,39) "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/InlineBlocks.cshtml"
link

#line default
#line hidden
#nullable disable
                );
#nullable restore
#line (2,39)-(2,49) "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/InlineBlocks.cshtml"
 } else { 

#line default
#line hidden
#nullable disable
                WriteLiteral("#");
#nullable restore
#line (2,63)-(2,65) "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/InlineBlocks.cshtml"
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
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; } = default!;
        #nullable disable
    }
}
#pragma warning restore 1591
