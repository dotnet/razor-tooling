﻿// <auto-generated/>
#pragma warning disable 1591
namespace Microsoft.AspNetCore.Razor.Language.IntegrationTests.TestFiles
{
    #line hidden
    public class TestFiles_IntegrationTests_CodeGenerationIntegrationTest_Templates_DesignTime
    {
        #pragma warning disable 219
        private void __RazorDirectiveTokenHelpers__() {
        }
        #pragma warning restore 219
        #pragma warning disable 0414
        private static System.Object __o = null;
        #pragma warning restore 0414
        #pragma warning disable 1998
        public async System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 11 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Templates.cshtml"
  
    Func<dynamic, object> foo = 

#line default
#line hidden
#nullable disable
            item => new Template(async(__razor_template_writer) => {
#nullable restore
#line 12 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Templates.cshtml"
                                             __o = item;

#line default
#line hidden
#nullable disable
            }
            )
#nullable restore
#line 12 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Templates.cshtml"
                                                               ;
    

#line default
#line hidden
#nullable disable
#nullable restore
#line 13 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Templates.cshtml"
__o = foo("");

#line default
#line hidden
#nullable disable
#nullable restore
#line 13 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Templates.cshtml"
            

#line default
#line hidden
#nullable disable
#nullable restore
#line 16 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Templates.cshtml"
   
    Func<dynamic, object> bar = 

#line default
#line hidden
#nullable disable
            item => new Template(async(__razor_template_writer) => {
#nullable restore
#line 17 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Templates.cshtml"
                                      __o = item;

#line default
#line hidden
#nullable disable
            }
            )
#nullable restore
#line 17 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Templates.cshtml"
                                                           ;
    

#line default
#line hidden
#nullable disable
#nullable restore
#line 18 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Templates.cshtml"
__o = bar("myclass");

#line default
#line hidden
#nullable disable
#nullable restore
#line 18 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Templates.cshtml"
                   

#line default
#line hidden
#nullable disable
#nullable restore
#line 22 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Templates.cshtml"
__o = Repeat(10, item => new Template(async(__razor_template_writer) => {
#nullable restore
#line 22 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Templates.cshtml"
                   __o = item;

#line default
#line hidden
#nullable disable
}
));

#line default
#line hidden
#nullable disable
#nullable restore
#line 26 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Templates.cshtml"
__o = Repeat(10,
    item => new Template(async(__razor_template_writer) => {
#nullable restore
#line 27 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Templates.cshtml"
               __o = item;

#line default
#line hidden
#nullable disable
}
));

#line default
#line hidden
#nullable disable
#nullable restore
#line 32 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Templates.cshtml"
__o = Repeat(10,
    item => new Template(async(__razor_template_writer) => {
#nullable restore
#line 33 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Templates.cshtml"
                __o = item;

#line default
#line hidden
#nullable disable
}
));

#line default
#line hidden
#nullable disable
#nullable restore
#line 38 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Templates.cshtml"
__o = Repeat(10,
    item => new Template(async(__razor_template_writer) => {
#nullable restore
#line 39 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Templates.cshtml"
                 __o = item;

#line default
#line hidden
#nullable disable
}
));

#line default
#line hidden
#nullable disable
#nullable restore
#line 45 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Templates.cshtml"
__o = Repeat(10, item => new Template(async(__razor_template_writer) => {
#nullable restore
#line 46 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Templates.cshtml"
         __o = item;

#line default
#line hidden
#nullable disable
#nullable restore
#line 47 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Templates.cshtml"
          var parent = item;

#line default
#line hidden
#nullable disable
}
));

#line default
#line hidden
#nullable disable
        }
        #pragma warning restore 1998
#nullable restore
#line 1 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Templates.cshtml"
            
    public HelperResult Repeat(int times, Func<int, object> template) {
        return new HelperResult((writer) => {
            for(int i = 0; i < times; i++) {
                ((HelperResult)template(i)).WriteTo(writer);
            }
        });
    }

#line default
#line hidden
#nullable disable
    }
}
#pragma warning restore 1591
