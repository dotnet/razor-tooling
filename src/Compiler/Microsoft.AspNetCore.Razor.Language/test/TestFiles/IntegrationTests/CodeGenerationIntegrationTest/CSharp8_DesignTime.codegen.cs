﻿// <auto-generated/>
#pragma warning disable 1591
namespace Microsoft.AspNetCore.Razor.Language.IntegrationTests.TestFiles;
#line hidden
#nullable restore
#line 1 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/CSharp8.cshtml"
using System.Collections.Generic;

#line default
#line hidden
#nullable disable
public class TestFiles_IntegrationTests_CodeGenerationIntegrationTest_CSharp8_DesignTime
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
#line 3 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/CSharp8.cshtml"
  
    IAsyncEnumerable<bool> GetAsyncEnumerable()
    {
        return null;
    }

    await foreach (var val in GetAsyncEnumerable())
    {

    }

    Range range = 1..5;
    using var disposable = GetLastDisposableInRange(range);

    var words = Array.Empty<string>();
    var testEnum = GetEnum();
    static TestEnum GetEnum()
    {
        return TestEnum.First;
    }

#line default
#line hidden
#nullable disable
#nullable restore
#line 25 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/CSharp8.cshtml"
__o = words[1..2];

#line default
#line hidden
#nullable disable
#nullable restore
#line 26 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/CSharp8.cshtml"
__o = words[^2..^0];

#line default
#line hidden
#nullable disable
#nullable restore
#line 28 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/CSharp8.cshtml"
__o = testEnum switch
{
    TestEnum.First => "The First!",
    TestEnum.Second => "The Second!",
    _ => "The others",
};

#line default
#line hidden
#nullable disable
#nullable restore
#line 35 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/CSharp8.cshtml"
 await foreach (var val in GetAsyncEnumerable())
{
    

#line default
#line hidden
#nullable disable
#nullable restore
#line 37 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/CSharp8.cshtml"
__o = val;

#line default
#line hidden
#nullable disable
#nullable restore
#line 37 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/CSharp8.cshtml"
        
}

#line default
#line hidden
#nullable disable
#nullable restore
#line 40 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/CSharp8.cshtml"
__o = Person!.Name;

#line default
#line hidden
#nullable disable
#nullable restore
#line 41 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/CSharp8.cshtml"
__o = People![0]!.Name![1];

#line default
#line hidden
#nullable disable
#nullable restore
#line 42 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/CSharp8.cshtml"
__o = DoSomething!(Person!);

#line default
#line hidden
#nullable disable
    }
    #pragma warning restore 1998
#nullable restore
#line 44 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/CSharp8.cshtml"
            
    enum TestEnum
    {
        First,
        Second
    }

    IDisposable GetLastDisposableInRange(Range range)
    {
        var disposables = (IDisposable[])ViewData["disposables"];
        return disposables[range][^1];
    }

    private Human? Person { get; set; }

    private Human?[]? People { get; set; }

    private Func<Human, string>? DoSomething { get; set; }

    private class Human
    {
        public string? Name { get; set; }
    }

#line default
#line hidden
#nullable disable
}
#pragma warning restore 1591
