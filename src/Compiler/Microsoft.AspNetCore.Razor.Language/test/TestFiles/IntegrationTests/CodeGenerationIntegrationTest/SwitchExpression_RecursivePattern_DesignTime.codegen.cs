﻿// <auto-generated/>
#pragma warning disable 1591
namespace Microsoft.AspNetCore.Razor.Language.IntegrationTests.TestFiles;
#line hidden
public class TestFiles_IntegrationTests_CodeGenerationIntegrationTest_SwitchExpression_RecursivePattern_DesignTime
{
    #pragma warning disable 219
    private void __RazorDirectiveTokenHelpers__() {
    }
    #pragma warning restore 219
    #pragma warning disable 0414
    private static object __o = null;
    #pragma warning restore 0414
    #pragma warning disable 1998
    public async System.Threading.Tasks.Task ExecuteAsync()
    {
#nullable restore
#line 1 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/SwitchExpression_RecursivePattern.cshtml"
 switch (DateTimeOffset.UtcNow)
{
    case { Year: 2022 }:
        

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/SwitchExpression_RecursivePattern.cshtml"
                                                                                                 
        break;
    case [{ Test: true }]:
        break;
    case ({ Test: { Nested.Pattern: global::Qualifier } }):
        break;
    case global::Test:
        break;
}

#line default
#line hidden
#nullable disable
    }
    #pragma warning restore 1998
}
#pragma warning restore 1591
