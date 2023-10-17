﻿#pragma checksum "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Await.cshtml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "347cf5b257c3885845256697175dd94c0ef0bef29e4fca7e4ec1a009ff29d9a6"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(Microsoft.AspNetCore.Razor.Language.IntegrationTests.TestFiles.TestFiles_IntegrationTests_CodeGenerationIntegrationTest_Await_Runtime), @"default", @"/TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Await.cshtml")]
namespace Microsoft.AspNetCore.Razor.Language.IntegrationTests.TestFiles
{
    #line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA256", @"347cf5b257c3885845256697175dd94c0ef0bef29e4fca7e4ec1a009ff29d9a6", @"/TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Await.cshtml")]
    public class TestFiles_IntegrationTests_CodeGenerationIntegrationTest_Await_Runtime
    {
        #pragma warning disable 1998
        public async System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n<section>\r\n    <h1>Basic Asynchronous Expression Test</h1>\r\n    <p>Basic Asynchronous Expression: ");
#nullable restore
#line (10,40)-(10,51) 6 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Await.cshtml"
Write(await Foo());

#line default
#line hidden
#nullable disable
            WriteLiteral("</p>\r\n    <p>Basic Asynchronous Template: ");
#nullable restore
#line (11,39)-(11,50) 6 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Await.cshtml"
Write(await Foo());

#line default
#line hidden
#nullable disable
            WriteLiteral("</p>\r\n    <p>Basic Asynchronous Statement: ");
#nullable restore
#line 12 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Await.cshtml"
                                        await Foo(); 

#line default
#line hidden
#nullable disable
            WriteLiteral("</p>\r\n    <p>Basic Asynchronous Statement Nested: ");
            WriteLiteral(" <b>");
#nullable restore
#line (13,52)-(13,63) 6 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Await.cshtml"
Write(await Foo());

#line default
#line hidden
#nullable disable
            WriteLiteral("</b> ");
            WriteLiteral("</p>\r\n    <p>Basic Incomplete Asynchronous Statement: ");
#nullable restore
#line (14,50)-(14,55) 6 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Await.cshtml"
Write(await);

#line default
#line hidden
#nullable disable
            WriteLiteral("</p>\r\n</section>\r\n\r\n<section>\r\n    <h1>Advanced Asynchronous Expression Test</h1>\r\n    <p>Advanced Asynchronous Expression: ");
#nullable restore
#line (19,43)-(19,58) 6 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Await.cshtml"
Write(await Foo(1, 2));

#line default
#line hidden
#nullable disable
            WriteLiteral("</p>\r\n    <p>Advanced Asynchronous Expression Extended: ");
#nullable restore
#line (20,52)-(20,71) 6 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Await.cshtml"
Write(await Foo.Bar(1, 2));

#line default
#line hidden
#nullable disable
            WriteLiteral("</p>\r\n    <p>Advanced Asynchronous Template: ");
#nullable restore
#line (21,42)-(21,64) 6 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Await.cshtml"
Write(await Foo("bob", true));

#line default
#line hidden
#nullable disable
            WriteLiteral("</p>\r\n    <p>Advanced Asynchronous Statement: ");
#nullable restore
#line 22 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Await.cshtml"
                                           await Foo(something, hello: "world"); 

#line default
#line hidden
#nullable disable
            WriteLiteral("</p>\r\n    <p>Advanced Asynchronous Statement Extended: ");
#nullable restore
#line 23 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Await.cshtml"
                                                    await Foo.Bar(1, 2) 

#line default
#line hidden
#nullable disable
            WriteLiteral("</p>\r\n    <p>Advanced Asynchronous Statement Nested: ");
            WriteLiteral(" <b>");
#nullable restore
#line (24,55)-(24,82) 6 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Await.cshtml"
Write(await Foo(boolValue: false));

#line default
#line hidden
#nullable disable
            WriteLiteral("</b> ");
            WriteLiteral("</p>\r\n    <p>Advanced Incomplete Asynchronous Statement: ");
#nullable restore
#line (25,53)-(25,72) 6 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Await.cshtml"
Write(await ("wrrronggg"));

#line default
#line hidden
#nullable disable
            WriteLiteral("</p>\r\n</section>");
        }
        #pragma warning restore 1998
#nullable restore
#line 1 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Await.cshtml"
            
    public async Task<string> Foo()
    {
        return "Bar";
    }

#line default
#line hidden
#nullable disable
    }
}
#pragma warning restore 1591
