﻿#pragma checksum "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/FunctionsBlock.cshtml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "87718d380117c487f66665bd4c191fcab3bbf6821e64caa809bf2fd1ad6af664"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(Microsoft.AspNetCore.Razor.Language.IntegrationTests.TestFiles.TestFiles_IntegrationTests_CodeGenerationIntegrationTest_FunctionsBlock_Runtime), @"default", @"/TestFiles/IntegrationTests/CodeGenerationIntegrationTest/FunctionsBlock.cshtml")]
namespace Microsoft.AspNetCore.Razor.Language.IntegrationTests.TestFiles;
#line hidden
[global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA256", @"87718d380117c487f66665bd4c191fcab3bbf6821e64caa809bf2fd1ad6af664", @"/TestFiles/IntegrationTests/CodeGenerationIntegrationTest/FunctionsBlock.cshtml")]
public class TestFiles_IntegrationTests_CodeGenerationIntegrationTest_FunctionsBlock_Runtime
{
    #pragma warning disable 1998
    public async System.Threading.Tasks.Task ExecuteAsync()
    {
        WriteLiteral("\r\n");
        WriteLiteral("\r\nHere\'s a random number: ");
#nullable restore
#line (12,25)-(12,36) 6 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/FunctionsBlock.cshtml"
Write(RandomInt());

#line default
#line hidden
#nullable disable
    }
    #pragma warning restore 1998
#nullable restore
#line 5 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/FunctionsBlock.cshtml"
            
    Random _rand = new Random();
    private int RandomInt() {
        return _rand.Next();
    }

#line default
#line hidden
#nullable disable
}
#pragma warning restore 1591
