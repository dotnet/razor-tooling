﻿#pragma checksum "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/NestedCSharp.cshtml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "c1273eb7fee44c32ab00da03db6ac9a1bf83c1c3b09f32fb242db42978fc4d17"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.TestFiles_IntegrationTests_CodeGenerationIntegrationTest_NestedCSharp), @"mvc.1.0.view", @"/TestFiles/IntegrationTests/CodeGenerationIntegrationTest/NestedCSharp.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"Sha256", @"c1273eb7fee44c32ab00da03db6ac9a1bf83c1c3b09f32fb242db42978fc4d17", @"/TestFiles/IntegrationTests/CodeGenerationIntegrationTest/NestedCSharp.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemMetadataAttribute("Identifier", "/TestFiles/IntegrationTests/CodeGenerationIntegrationTest/NestedCSharp.cshtml")]
    [global::System.Runtime.CompilerServices.CreateNewOnMetadataUpdateAttribute]
    #nullable restore
    public class TestFiles_IntegrationTests_CodeGenerationIntegrationTest_NestedCSharp : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    #nullable disable
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line (2,6)-(4,1) "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/NestedCSharp.cshtml"
foreach (var result in (dynamic)Url)
    {

#line default
#line hidden
#nullable disable

            WriteLiteral("        <div>\r\n            ");
            Write(
#nullable restore
#line (5,14)-(5,30) "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/NestedCSharp.cshtml"
result.SomeValue

#line default
#line hidden
#nullable disable
            );
            WriteLiteral(".\r\n        </div>\r\n");
#nullable restore
#line (7,1)-(7,6) "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/NestedCSharp.cshtml"
    }

#line default
#line hidden
#nullable disable

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
