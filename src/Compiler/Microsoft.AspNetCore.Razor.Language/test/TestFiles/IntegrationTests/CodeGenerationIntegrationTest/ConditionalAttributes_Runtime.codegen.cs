﻿#pragma checksum "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/ConditionalAttributes.cshtml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "06e553c9dddd22392e81b4db3d16d097612b3552ffeaa58d015a2e3dedcf6c3a"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(Microsoft.AspNetCore.Razor.Language.IntegrationTests.TestFiles.TestFiles_IntegrationTests_CodeGenerationIntegrationTest_ConditionalAttributes_Runtime), @"default", @"/TestFiles/IntegrationTests/CodeGenerationIntegrationTest/ConditionalAttributes.cshtml")]
namespace Microsoft.AspNetCore.Razor.Language.IntegrationTests.TestFiles
{
    #line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"Sha256", @"06e553c9dddd22392e81b4db3d16d097612b3552ffeaa58d015a2e3dedcf6c3a", @"/TestFiles/IntegrationTests/CodeGenerationIntegrationTest/ConditionalAttributes.cshtml")]
    public class TestFiles_IntegrationTests_CodeGenerationIntegrationTest_ConditionalAttributes_Runtime
    {
        #pragma warning disable 1998
        public async System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line (1,3)-(4,1) "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/ConditionalAttributes.cshtml"

    var ch = true;
    var cls = "bar";

#line default
#line hidden
#nullable disable

            WriteLiteral("    <a href=\"Foo\" />\r\n    <p");
            BeginWriteAttribute("class", " class=\"", 74, "\"", 86, 1);
#nullable restore
#line (5,15)-(5,19) 28 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/ConditionalAttributes.cshtml"
WriteAttributeValue("", 82, cls, 82, 4, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" />\r\n    <p");
            BeginWriteAttribute("class", " class=\"", 98, "\"", 114, 2);
            WriteAttributeValue("", 106, "foo", 106, 3, true);
#nullable restore
#line (6,18)-(6,23) 30 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/ConditionalAttributes.cshtml"
WriteAttributeValue(" ", 109, cls, 110, 4, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" />\r\n    <p");
            BeginWriteAttribute("class", " class=\"", 126, "\"", 142, 2);
#nullable restore
#line (7,15)-(7,19) 29 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/ConditionalAttributes.cshtml"
WriteAttributeValue("", 134, cls, 134, 4, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue(" ", 138, "foo", 139, 4, true);
            EndWriteAttribute();
            WriteLiteral(" />\r\n    <input type=\"checkbox\"");
            BeginWriteAttribute("checked", " checked=\"", 174, "\"", 187, 1);
#nullable restore
#line (8,37)-(8,40) 29 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/ConditionalAttributes.cshtml"
WriteAttributeValue("", 184, ch, 184, 3, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" />\r\n    <input type=\"checkbox\"");
            BeginWriteAttribute("checked", " checked=\"", 219, "\"", 236, 2);
            WriteAttributeValue("", 229, "foo", 229, 3, true);
#nullable restore
#line (9,40)-(9,44) 30 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/ConditionalAttributes.cshtml"
WriteAttributeValue(" ", 232, ch, 233, 3, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" />\r\n    <p");
            BeginWriteAttribute("class", " class=\"", 248, "\"", 281, 1);
            WriteAttributeValue("", 256, new Microsoft.AspNetCore.Mvc.Razor.HelperResult(async(__razor_attribute_value_writer) => {
                PushWriter(__razor_attribute_value_writer);
#nullable restore
#line 10 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/ConditionalAttributes.cshtml"
               if(cls != null) { 

#line default
#line hidden
#nullable disable
                Write(
#nullable restore
#line (10,35)-(10,38) "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/ConditionalAttributes.cshtml"
cls

#line default
#line hidden
#nullable disable
                );
#nullable restore
#line 10 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/ConditionalAttributes.cshtml"
                                      }

#line default
#line hidden
#nullable disable
                PopWriter();
            }
            ), 256, 25, false);
            EndWriteAttribute();
            WriteLiteral(" />\r\n    <a href=\"~/Foo\" />\r\n    <script");
            BeginWriteAttribute("src", " src=\"", 322, "\"", 373, 1);
#nullable restore
#line (12,18)-(12,63) 29 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/ConditionalAttributes.cshtml"
WriteAttributeValue("", 328, Url.Content("~/Scripts/jquery-1.6.2.min.js"), 328, 45, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" type=\"text/javascript\"></script>\r\n    <script");
            BeginWriteAttribute("src", " src=\"", 420, "\"", 487, 1);
#nullable restore
#line (13,18)-(13,79) 29 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/ConditionalAttributes.cshtml"
WriteAttributeValue("", 426, Url.Content("~/Scripts/modernizr-2.0.6-development-only.js"), 426, 61, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" type=\"text/javascript\"></script>\r\n    <script src=\"http://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.16/jquery-ui.min.js\" type=\"text/javascript\"></script>\r\n");
        }
        #pragma warning restore 1998
    }
}
#pragma warning restore 1591
