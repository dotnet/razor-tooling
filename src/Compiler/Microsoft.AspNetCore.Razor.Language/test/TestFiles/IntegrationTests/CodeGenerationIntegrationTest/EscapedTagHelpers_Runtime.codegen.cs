﻿#pragma checksum "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/EscapedTagHelpers.cshtml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "602b63a03b7490d9350a834c628e36b03134479ea417de4aca379f59b8089f35"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(Microsoft.AspNetCore.Razor.Language.IntegrationTests.TestFiles.TestFiles_IntegrationTests_CodeGenerationIntegrationTest_EscapedTagHelpers_Runtime), @"default", @"/TestFiles/IntegrationTests/CodeGenerationIntegrationTest/EscapedTagHelpers.cshtml")]
namespace Microsoft.AspNetCore.Razor.Language.IntegrationTests.TestFiles
{
    #line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA256", @"602b63a03b7490d9350a834c628e36b03134479ea417de4aca379f59b8089f35", @"/TestFiles/IntegrationTests/CodeGenerationIntegrationTest/EscapedTagHelpers.cshtml")]
    public class TestFiles_IntegrationTests_CodeGenerationIntegrationTest_EscapedTagHelpers_Runtime
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
        private global::TestNamespace.InputTagHelper __TestNamespace_InputTagHelper;
        private global::TestNamespace.InputTagHelper2 __TestNamespace_InputTagHelper2;
        #pragma warning disable 1998
        public async System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n<");
            WriteLiteral("div class=\"randomNonTagHelperAttribute\">\r\n    <");
            WriteLiteral("p class=\"Hello World\" ");
#nullable restore
#line (4,29)-(4,41) 6 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/EscapedTagHelpers.cshtml"
Write(DateTime.Now);

#line default
#line hidden
#nullable disable
            WriteLiteral(">\r\n        <");
            WriteLiteral("input type=\"text\" />\r\n        <");
            WriteLiteral("em>Not a TagHelper: </");
            WriteLiteral("em> ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("input", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "test", async() => {
            }
            );
            __TestNamespace_InputTagHelper = CreateTagHelper<global::TestNamespace.InputTagHelper>();
            __tagHelperExecutionContext.Add(__TestNamespace_InputTagHelper);
            __TestNamespace_InputTagHelper2 = CreateTagHelper<global::TestNamespace.InputTagHelper2>();
            __tagHelperExecutionContext.Add(__TestNamespace_InputTagHelper2);
            BeginWriteTagHelperAttribute();
#nullable restore
#line (6,51)-(6,63) 13 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/EscapedTagHelpers.cshtml"
WriteLiteral(DateTime.Now);

#line default
#line hidden
#nullable disable
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __TestNamespace_InputTagHelper.Type = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("type", __TestNamespace_InputTagHelper.Type, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            __TestNamespace_InputTagHelper2.Type = __TestNamespace_InputTagHelper.Type;
#nullable restore
#line 6 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/EscapedTagHelpers.cshtml"
__TestNamespace_InputTagHelper2.Checked = true;

#line default
#line hidden
#nullable disable
            __tagHelperExecutionContext.AddTagHelperAttribute("checked", __TestNamespace_InputTagHelper2.Checked, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n    </");
            WriteLiteral("p>\r\n</");
            WriteLiteral("div>");
        }
        #pragma warning restore 1998
    }
}
#pragma warning restore 1591
