﻿#pragma checksum "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/ModelExpressionTagHelper.cshtml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "b96e944bd86a2acecd5a176708eedb3cdc8eef05122fd51aa5c4fe58d4069af7"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.TestFiles_IntegrationTests_CodeGenerationIntegrationTest_ModelExpressionTagHelper), @"mvc.1.0.view", @"/TestFiles/IntegrationTests/CodeGenerationIntegrationTest/ModelExpressionTagHelper.cshtml")]
namespace AspNetCore
{
    #line hidden
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Linq;
    using global::System.Threading.Tasks;
    using global::Microsoft.AspNetCore.Mvc;
    using global::Microsoft.AspNetCore.Mvc.Rendering;
    using global::Microsoft.AspNetCore.Mvc.ViewFeatures;
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"Sha256", @"b96e944bd86a2acecd5a176708eedb3cdc8eef05122fd51aa5c4fe58d4069af7", @"/TestFiles/IntegrationTests/CodeGenerationIntegrationTest/ModelExpressionTagHelper.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemMetadataAttribute("Identifier", "/TestFiles/IntegrationTests/CodeGenerationIntegrationTest/ModelExpressionTagHelper.cshtml")]
    [global::System.Runtime.CompilerServices.CreateNewOnMetadataUpdateAttribute]
    #nullable restore
    public class TestFiles_IntegrationTests_CodeGenerationIntegrationTest_ModelExpressionTagHelper : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<DateTime>
    #nullable disable
    {
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::InputTestTagHelper __InputTestTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
            WriteLiteral("\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("input-test", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "test", async() => {
            }
            );
            __InputTestTagHelper = CreateTagHelper<global::InputTestTagHelper>();
            __tagHelperExecutionContext.Add(__InputTestTagHelper);
            __InputTestTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, __model => __model.
#nullable restore
#line (5,18)-(5,22) "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/ModelExpressionTagHelper.cshtml"
Date

#line default
#line hidden
#nullable disable
            );
            __tagHelperExecutionContext.AddTagHelperAttribute("for", __InputTestTagHelper.For, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("input-test", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "test", async() => {
            }
            );
            __InputTestTagHelper = CreateTagHelper<global::InputTestTagHelper>();
            __tagHelperExecutionContext.Add(__InputTestTagHelper);
            __InputTestTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, __model => 
#nullable restore
#line (6,19)-(6,24) "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/ModelExpressionTagHelper.cshtml"
Model

#line default
#line hidden
#nullable disable
            );
            __tagHelperExecutionContext.AddTagHelperAttribute("for", __InputTestTagHelper.For, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<DateTime> Html { get; private set; } = default!;
        #nullable disable
    }
}
#pragma warning restore 1591
