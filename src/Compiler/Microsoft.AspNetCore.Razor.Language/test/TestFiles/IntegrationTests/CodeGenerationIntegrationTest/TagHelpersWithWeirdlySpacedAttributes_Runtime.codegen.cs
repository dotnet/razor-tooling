﻿#pragma checksum "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/TagHelpersWithWeirdlySpacedAttributes.cshtml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "d9b84e2f98477223e105fd1bb35f0717dea379bb406844d36d35a9220d093ff3"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(Microsoft.AspNetCore.Razor.Language.IntegrationTests.TestFiles.TestFiles_IntegrationTests_CodeGenerationIntegrationTest_TagHelpersWithWeirdlySpacedAttributes_Runtime), @"default", @"/TestFiles/IntegrationTests/CodeGenerationIntegrationTest/TagHelpersWithWeirdlySpacedAttributes.cshtml")]
namespace Microsoft.AspNetCore.Razor.Language.IntegrationTests.TestFiles;
#line hidden
[global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA256", @"d9b84e2f98477223e105fd1bb35f0717dea379bb406844d36d35a9220d093ff3", @"/TestFiles/IntegrationTests/CodeGenerationIntegrationTest/TagHelpersWithWeirdlySpacedAttributes.cshtml")]
public class TestFiles_IntegrationTests_CodeGenerationIntegrationTest_TagHelpersWithWeirdlySpacedAttributes_Runtime
{
    private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("Hello World"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
    private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("type", "text", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.SingleQuotes);
    private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("data-content", new global::Microsoft.AspNetCore.Html.HtmlString("hello"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
    private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("data-content", new global::Microsoft.AspNetCore.Html.HtmlString("hello2"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.SingleQuotes);
    private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_4 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("type", "password", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
    private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_5 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("data-content", new global::Microsoft.AspNetCore.Html.HtmlString("blah"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
    private global::TestNamespace.PTagHelper __TestNamespace_PTagHelper;
    private global::TestNamespace.InputTagHelper __TestNamespace_InputTagHelper;
    private global::TestNamespace.InputTagHelper2 __TestNamespace_InputTagHelper2;
    #pragma warning disable 1998
    public async System.Threading.Tasks.Task ExecuteAsync()
    {
        WriteLiteral("\r\n");
        __tagHelperExecutionContext = __tagHelperScopeManager.Begin("p", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "test", async() => {
            WriteLiteral("Body of Tag");
        }
        );
        __TestNamespace_PTagHelper = CreateTagHelper<global::TestNamespace.PTagHelper>();
        __tagHelperExecutionContext.Add(__TestNamespace_PTagHelper);
        __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
#nullable restore
#line 6 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/TagHelpersWithWeirdlySpacedAttributes.cshtml"
__TestNamespace_PTagHelper.Age = 1337;

#line default
#line hidden
#nullable disable
        __tagHelperExecutionContext.AddTagHelperAttribute("age", __TestNamespace_PTagHelper.Age, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        BeginWriteTagHelperAttribute();
#nullable restore
#line (7,19)-(7,23) 6 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/TagHelpersWithWeirdlySpacedAttributes.cshtml"
Write(true);

#line default
#line hidden
#nullable disable
        __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
        __tagHelperExecutionContext.AddHtmlAttribute("data-content", Html.Raw(__tagHelperStringValueBuffer), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
        if (!__tagHelperExecutionContext.Output.IsContentModified)
        {
            await __tagHelperExecutionContext.SetOutputContentAsync();
        }
        Write(__tagHelperExecutionContext.Output);
        __tagHelperExecutionContext = __tagHelperScopeManager.End();
        WriteLiteral("\r\n\r\n");
        __tagHelperExecutionContext = __tagHelperScopeManager.Begin("input", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "test", async() => {
        }
        );
        __TestNamespace_InputTagHelper = CreateTagHelper<global::TestNamespace.InputTagHelper>();
        __tagHelperExecutionContext.Add(__TestNamespace_InputTagHelper);
        __TestNamespace_InputTagHelper2 = CreateTagHelper<global::TestNamespace.InputTagHelper2>();
        __tagHelperExecutionContext.Add(__TestNamespace_InputTagHelper2);
        __TestNamespace_InputTagHelper.Type = (string)__tagHelperAttribute_1.Value;
        __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
        __TestNamespace_InputTagHelper2.Type = (string)__tagHelperAttribute_1.Value;
        __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
        __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
        await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
        if (!__tagHelperExecutionContext.Output.IsContentModified)
        {
            await __tagHelperExecutionContext.SetOutputContentAsync();
        }
        Write(__tagHelperExecutionContext.Output);
        __tagHelperExecutionContext = __tagHelperScopeManager.End();
        WriteLiteral("\r\n\r\n");
        __tagHelperExecutionContext = __tagHelperScopeManager.Begin("p", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "test", async() => {
        }
        );
        __TestNamespace_PTagHelper = CreateTagHelper<global::TestNamespace.PTagHelper>();
        __tagHelperExecutionContext.Add(__TestNamespace_PTagHelper);
#nullable restore
#line 11 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/TagHelpersWithWeirdlySpacedAttributes.cshtml"
__TestNamespace_PTagHelper.Age = 1234;

#line default
#line hidden
#nullable disable
        __tagHelperExecutionContext.AddTagHelperAttribute("age", __TestNamespace_PTagHelper.Age, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_3);
        await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
        if (!__tagHelperExecutionContext.Output.IsContentModified)
        {
            await __tagHelperExecutionContext.SetOutputContentAsync();
        }
        Write(__tagHelperExecutionContext.Output);
        __tagHelperExecutionContext = __tagHelperScopeManager.End();
        WriteLiteral("\r\n\r\n");
        __tagHelperExecutionContext = __tagHelperScopeManager.Begin("input", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "test", async() => {
        }
        );
        __TestNamespace_InputTagHelper = CreateTagHelper<global::TestNamespace.InputTagHelper>();
        __tagHelperExecutionContext.Add(__TestNamespace_InputTagHelper);
        __TestNamespace_InputTagHelper2 = CreateTagHelper<global::TestNamespace.InputTagHelper2>();
        __tagHelperExecutionContext.Add(__TestNamespace_InputTagHelper2);
        __TestNamespace_InputTagHelper.Type = (string)__tagHelperAttribute_4.Value;
        __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_4);
        __TestNamespace_InputTagHelper2.Type = (string)__tagHelperAttribute_4.Value;
        __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_4);
        __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_5);
        await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
        if (!__tagHelperExecutionContext.Output.IsContentModified)
        {
            await __tagHelperExecutionContext.SetOutputContentAsync();
        }
        Write(__tagHelperExecutionContext.Output);
        __tagHelperExecutionContext = __tagHelperScopeManager.End();
    }
    #pragma warning restore 1998
}
#pragma warning restore 1591
