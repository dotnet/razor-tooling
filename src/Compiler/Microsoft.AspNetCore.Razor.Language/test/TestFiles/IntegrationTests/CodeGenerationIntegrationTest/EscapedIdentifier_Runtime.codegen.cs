﻿#pragma checksum "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/EscapedIdentifier.cshtml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "8fc172b55ced462a1746f1082869701d161cf36fd24dacd8d9fa98cc66d744ad"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(Microsoft.AspNetCore.Razor.Language.IntegrationTests.TestFiles.TestFiles_IntegrationTests_CodeGenerationIntegrationTest_EscapedIdentifier_Runtime), @"default", @"/TestFiles/IntegrationTests/CodeGenerationIntegrationTest/EscapedIdentifier.cshtml")]
namespace Microsoft.AspNetCore.Razor.Language.IntegrationTests.TestFiles
{
    #line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"Sha256", @"8fc172b55ced462a1746f1082869701d161cf36fd24dacd8d9fa98cc66d744ad", @"/TestFiles/IntegrationTests/CodeGenerationIntegrationTest/EscapedIdentifier.cshtml")]
    public class TestFiles_IntegrationTests_CodeGenerationIntegrationTest_EscapedIdentifier_Runtime
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
        private global::InputTagHelper __InputTagHelper;
        #pragma warning disable 1998
        public async System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line (3,3)-(8,1) "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/EscapedIdentifier.cshtml"

    var count = "1";
    var alive = true;
    var obj = new { age = (object)1 };
    var item = new { Items = new System.List<string>() { "one", "two" } };

#line default
#line hidden
#nullable disable

            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("input", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "test", async() => {
            }
            );
            __InputTagHelper = CreateTagHelper<global::InputTagHelper>();
            __tagHelperExecutionContext.Add(__InputTagHelper);
            __InputTagHelper.AgeProp = 
#nullable restore
#line (9,13)-(9,29) "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/EscapedIdentifier.cshtml"
Convert.ToInt32(

#line default
#line hidden
#nullable disable
#nullable restore
#line (9,29)-(9,35) "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/EscapedIdentifier.cshtml"
@count

#line default
#line hidden
#nullable disable
#nullable restore
#line (9,35)-(9,36) "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/EscapedIdentifier.cshtml"
)

#line default
#line hidden
#nullable disable
            ;
            __tagHelperExecutionContext.AddTagHelperAttribute("age", __InputTagHelper.AgeProp, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            __InputTagHelper.AliveProp = 
#nullable restore
#line (9,45)-(9,46) "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/EscapedIdentifier.cshtml"
!

#line default
#line hidden
#nullable disable
#nullable restore
#line (9,46)-(9,52) "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/EscapedIdentifier.cshtml"
@alive

#line default
#line hidden
#nullable disable
            ;
            __tagHelperExecutionContext.AddTagHelperAttribute("alive", __InputTagHelper.AliveProp, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("input", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "test", async() => {
            }
            );
            __InputTagHelper = CreateTagHelper<global::InputTagHelper>();
            __tagHelperExecutionContext.Add(__InputTagHelper);
            __InputTagHelper.AgeProp = 
#nullable restore
#line (10,13)-(10,18) "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/EscapedIdentifier.cshtml"
(int)

#line default
#line hidden
#nullable disable
#nullable restore
#line (10,18)-(10,26) "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/EscapedIdentifier.cshtml"
@obj.age

#line default
#line hidden
#nullable disable
            ;
            __tagHelperExecutionContext.AddTagHelperAttribute("age", __InputTagHelper.AgeProp, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            __InputTagHelper.TagProp = 
#nullable restore
#line (10,33)-(10,36) "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/EscapedIdentifier.cshtml"
new

#line default
#line hidden
#nullable disable
#nullable restore
#line (10,36)-(10,38) "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/EscapedIdentifier.cshtml"
 {

#line default
#line hidden
#nullable disable
#nullable restore
#line (10,38)-(10,39) "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/EscapedIdentifier.cshtml"
 

#line default
#line hidden
#nullable disable
#nullable restore
#line (10,39)-(10,46) "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/EscapedIdentifier.cshtml"
@params

#line default
#line hidden
#nullable disable
#nullable restore
#line (10,46)-(10,48) "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/EscapedIdentifier.cshtml"
 =

#line default
#line hidden
#nullable disable
#nullable restore
#line (10,48)-(10,50) "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/EscapedIdentifier.cshtml"
 1

#line default
#line hidden
#nullable disable
#nullable restore
#line (10,50)-(10,52) "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/EscapedIdentifier.cshtml"
 }

#line default
#line hidden
#nullable disable
            ;
            __tagHelperExecutionContext.AddTagHelperAttribute("tag", __InputTagHelper.TagProp, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("input", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "test", async() => {
            }
            );
            __InputTagHelper = CreateTagHelper<global::InputTagHelper>();
            __tagHelperExecutionContext.Add(__InputTagHelper);
            if (__InputTagHelper.DictionaryOfBoolAndStringTupleProperty == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("tuple-prefix-test", "InputTagHelper", "DictionaryOfBoolAndStringTupleProperty"));
            }
            __InputTagHelper.DictionaryOfBoolAndStringTupleProperty["test"] = 
#nullable restore
#line (11,27)-(11,28) "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/EscapedIdentifier.cshtml"
(

#line default
#line hidden
#nullable disable
#nullable restore
#line (11,28)-(11,33) "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/EscapedIdentifier.cshtml"
@item

#line default
#line hidden
#nullable disable
#nullable restore
#line (11,33)-(11,34) "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/EscapedIdentifier.cshtml"
.

#line default
#line hidden
#nullable disable
#nullable restore
#line (11,34)-(11,69) "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/EscapedIdentifier.cshtml"
 Items.Where(i=>i.Contains("one")).

#line default
#line hidden
#nullable disable
#nullable restore
#line (11,69)-(11,80) "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/EscapedIdentifier.cshtml"
 Count()>0,

#line default
#line hidden
#nullable disable
#nullable restore
#line (11,80)-(11,81) "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/EscapedIdentifier.cshtml"
 

#line default
#line hidden
#nullable disable
#nullable restore
#line (11,81)-(11,86) "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/EscapedIdentifier.cshtml"
@item

#line default
#line hidden
#nullable disable
#nullable restore
#line (11,86)-(11,87) "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/EscapedIdentifier.cshtml"
.

#line default
#line hidden
#nullable disable
#nullable restore
#line (11,87)-(11,132) "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/EscapedIdentifier.cshtml"
 Items.FirstOrDefault(i=>i.Contains("one"))?.

#line default
#line hidden
#nullable disable
#nullable restore
#line (11,132)-(11,151) "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/EscapedIdentifier.cshtml"
 Replace("one",""))

#line default
#line hidden
#nullable disable
            ;
            __tagHelperExecutionContext.AddTagHelperAttribute("tuple-prefix-test", __InputTagHelper.DictionaryOfBoolAndStringTupleProperty["test"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.SingleQuotes);
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
    }
}
#pragma warning restore 1591
