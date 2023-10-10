﻿#pragma checksum "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Markup_InCodeBlocks.cshtml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "c38f4efc9c710292c9fc20d2f5c4cc6e8a91f4fa3ee60ec96c16617f506312b8"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(Microsoft.AspNetCore.Razor.Language.IntegrationTests.TestFiles.TestFiles_IntegrationTests_CodeGenerationIntegrationTest_Markup_InCodeBlocks_Runtime), @"default", @"/TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Markup_InCodeBlocks.cshtml")]
namespace Microsoft.AspNetCore.Razor.Language.IntegrationTests.TestFiles;
#line hidden
[global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA256", @"c38f4efc9c710292c9fc20d2f5c4cc6e8a91f4fa3ee60ec96c16617f506312b8", @"/TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Markup_InCodeBlocks.cshtml")]
public class TestFiles_IntegrationTests_CodeGenerationIntegrationTest_Markup_InCodeBlocks_Runtime
{
    #pragma warning disable 1998
    public async System.Threading.Tasks.Task ExecuteAsync()
    {
        WriteLiteral("\r\n");
#nullable restore
#line 2 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Markup_InCodeBlocks.cshtml"
  
    var people = new Person[]
    {
        new Person() { Name = "Taylor", Age = 95, }
    };

    void PrintName(Person person)
    {

#line default
#line hidden
#nullable disable
        WriteLiteral("        <div>");
#nullable restore
#line (10,14)-(10,25) 6 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Markup_InCodeBlocks.cshtml"
Write(person.Name);

#line default
#line hidden
#nullable disable
        WriteLiteral("</div>\r\n");
#nullable restore
#line 11 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Markup_InCodeBlocks.cshtml"
    }

#line default
#line hidden
#nullable disable
        WriteLiteral("\r\n");
#nullable restore
#line 14 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Markup_InCodeBlocks.cshtml"
   PrintName(people[0]) 

#line default
#line hidden
#nullable disable
#nullable restore
#line 15 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Markup_InCodeBlocks.cshtml"
   AnnounceBirthday(people[0]); 

#line default
#line hidden
#nullable disable
        WriteLiteral("\r\n");
    }
    #pragma warning restore 1998
#nullable restore
#line 17 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Markup_InCodeBlocks.cshtml"
            
    void AnnounceBirthday(Person person)
    {
        var formatted = $"Mr. {person.Name}";

#line default
#line hidden
#nullable disable
    WriteLiteral("        <div>\r\n            <h3>Happy birthday ");
#nullable restore
#line (22,33)-(22,42) 6 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Markup_InCodeBlocks.cshtml"
Write(formatted);

#line default
#line hidden
#nullable disable
    WriteLiteral("!</h3>\r\n        </div>\r\n");
    WriteLiteral("        <ul>\r\n");
#nullable restore
#line 26 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Markup_InCodeBlocks.cshtml"
         for (var i = 0; i < person.Age / 10; i++)
        {

#line default
#line hidden
#nullable disable
    WriteLiteral("            <li>");
#nullable restore
#line (28,18)-(28,19) 6 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Markup_InCodeBlocks.cshtml"
Write(i);

#line default
#line hidden
#nullable disable
    WriteLiteral(" Happy birthday!</li>\r\n");
#nullable restore
#line 29 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Markup_InCodeBlocks.cshtml"
        }

#line default
#line hidden
#nullable disable
    WriteLiteral("        </ul>\r\n");
#nullable restore
#line 31 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Markup_InCodeBlocks.cshtml"

        if (person.Age < 20)
        {
            return;
        }


#line default
#line hidden
#nullable disable
    WriteLiteral("        <h4>Secret message</h4>\r\n");
#nullable restore
#line 38 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Markup_InCodeBlocks.cshtml"
    }

    class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

#line default
#line hidden
#nullable disable
}
#pragma warning restore 1591
