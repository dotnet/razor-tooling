﻿// <auto-generated/>
#pragma warning disable 1591
namespace Microsoft.AspNetCore.Razor.Language.IntegrationTests.TestFiles
{
    #line hidden
    public class TestFiles_IntegrationTests_CodeGenerationIntegrationTest_BasicTagHelpers_DesignTime
    {
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        private global::TestNamespace.PTagHelper __TestNamespace_PTagHelper;
        private global::TestNamespace.InputTagHelper __TestNamespace_InputTagHelper;
        private global::TestNamespace.InputTagHelper2 __TestNamespace_InputTagHelper2;
        #pragma warning disable 219
        private void __RazorDirectiveTokenHelpers__() {
        ((global::System.Action)(() => {
#nullable restore
#line 1 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/BasicTagHelpers.cshtml"
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
            __TestNamespace_PTagHelper = CreateTagHelper<global::TestNamespace.PTagHelper>();
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            __TestNamespace_InputTagHelper = CreateTagHelper<global::TestNamespace.InputTagHelper>();
            __TestNamespace_InputTagHelper2 = CreateTagHelper<global::TestNamespace.InputTagHelper2>();
#nullable restore
#line 6 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/BasicTagHelpers.cshtml"
                                __o = ViewBag.DefaultInterval;

#line default
#line hidden
#nullable disable
            __TestNamespace_InputTagHelper.Type = "text";
            __TestNamespace_InputTagHelper2.Type = __TestNamespace_InputTagHelper.Type;
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            __TestNamespace_InputTagHelper = CreateTagHelper<global::TestNamespace.InputTagHelper>();
            __TestNamespace_InputTagHelper2 = CreateTagHelper<global::TestNamespace.InputTagHelper2>();
            __TestNamespace_InputTagHelper.Type = "checkbox";
            __TestNamespace_InputTagHelper2.Type = __TestNamespace_InputTagHelper.Type;
#nullable restore
#line 7 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/BasicTagHelpers.cshtml"
__TestNamespace_InputTagHelper2.Checked = true;

#line default
#line hidden
#nullable disable
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            __TestNamespace_PTagHelper = CreateTagHelper<global::TestNamespace.PTagHelper>();
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
        }
        #pragma warning restore 1998
    }
}
#pragma warning restore 1591
