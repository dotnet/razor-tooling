﻿#pragma checksum "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/ExpressionsInCode.cshtml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "c9cc7d097735e4074bc4f71cd46101fc821fdb0592f90d491f09fdecddf3caf3"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(Microsoft.AspNetCore.Razor.Language.IntegrationTests.TestFiles.TestFiles_IntegrationTests_CodeGenerationIntegrationTest_ExpressionsInCode_Runtime), @"default", @"/TestFiles/IntegrationTests/CodeGenerationIntegrationTest/ExpressionsInCode.cshtml")]
namespace Microsoft.AspNetCore.Razor.Language.IntegrationTests.TestFiles
{
    #line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"Sha256", @"c9cc7d097735e4074bc4f71cd46101fc821fdb0592f90d491f09fdecddf3caf3", @"/TestFiles/IntegrationTests/CodeGenerationIntegrationTest/ExpressionsInCode.cshtml")]
    public class TestFiles_IntegrationTests_CodeGenerationIntegrationTest_ExpressionsInCode_Runtime
    {
        #pragma warning disable 1998
        public async System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 1 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/ExpressionsInCode.cshtml"
  
    object foo = null;
    string bar = "Foo";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
#nullable restore
#line 6 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/ExpressionsInCode.cshtml"
 if(foo != null) {
    

#line default
#line hidden
#nullable disable
#nullable restore
#line (7,6)-(7,9) 6 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/ExpressionsInCode.cshtml"
Write(foo);

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/ExpressionsInCode.cshtml"
        
} else {

#line default
#line hidden
#nullable disable
            WriteLiteral("    <p>Foo is Null!</p>\r\n");
#nullable restore
#line 10 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/ExpressionsInCode.cshtml"
}

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<p>\r\n");
#nullable restore
#line 13 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/ExpressionsInCode.cshtml"
 if(!String.IsNullOrEmpty(bar)) {
    

#line default
#line hidden
#nullable disable
#nullable restore
#line (14,7)-(14,28) 6 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/ExpressionsInCode.cshtml"
Write(bar.Replace("F", "B"));

#line default
#line hidden
#nullable disable
#nullable restore
#line 14 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/ExpressionsInCode.cshtml"
                            
}

#line default
#line hidden
#nullable disable
            WriteLiteral("</p>");
        }
        #pragma warning restore 1998
    }
}
#pragma warning restore 1591
