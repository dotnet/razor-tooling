﻿// <auto-generated/>
#pragma warning disable 1591
namespace Microsoft.AspNetCore.Razor.Language.IntegrationTests.TestFiles
{
    #line hidden
    public class TestFiles_IntegrationTests_CodeGenerationIntegrationTest_NestedTagHelpers_DesignTime
    {
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        private global::SpanTagHelper __SpanTagHelper;
        private global::DivTagHelper __DivTagHelper;
        private global::InputTagHelper __InputTagHelper;
        #pragma warning disable 219
        private void __RazorDirectiveTokenHelpers__() {
        ((System.Action)(() => {
#nullable restore
#line 1 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/NestedTagHelpers.cshtml"
global::System.Object __typeHelper = "*, TestAssembly";

#line default
#line hidden
#nullable disable
        }
        ))();
        }
        #pragma warning restore 219
        #pragma warning disable 0414
        private static object __o = null;
        #pragma warning restore 0414
        #pragma warning disable 1998
        public async System.Threading.Tasks.Task ExecuteAsync()
        {
            __SpanTagHelper = CreateTagHelper<global::SpanTagHelper>();
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            __InputTagHelper = CreateTagHelper<global::InputTagHelper>();
            __InputTagHelper.FooProp = "Hello";
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            __DivTagHelper = CreateTagHelper<global::DivTagHelper>();
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
        }
        #pragma warning restore 1998
    }
}
#pragma warning restore 1591
