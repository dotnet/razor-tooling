﻿// <auto-generated/>
#pragma warning disable 1591
namespace AspNetCoreGeneratedDocument
{
    #line default
    using TModel = global::System.Object;
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Linq;
    using global::System.Threading.Tasks;
    using global::Microsoft.AspNetCore.Mvc;
    using global::Microsoft.AspNetCore.Mvc.Rendering;
    using global::Microsoft.AspNetCore.Mvc.ViewFeatures;
    #line default
    #line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemMetadataAttribute("Identifier", "/TestFiles/IntegrationTests/CodeGenerationIntegrationTest/EscapedIdentifier.cshtml")]
    [global::System.Runtime.CompilerServices.CreateNewOnMetadataUpdateAttribute]
    #nullable restore
    internal sealed class TestFiles_IntegrationTests_CodeGenerationIntegrationTest_EscapedIdentifier : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    #nullable disable
    {
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        private global::InputTagHelper __InputTagHelper;
        #pragma warning disable 219
        private void __RazorDirectiveTokenHelpers__() {
        ((global::System.Action)(() => {
#nullable restore
#line 1 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/EscapedIdentifier.cshtml"
global::System.Object __typeHelper = "*, TestAssembly";

#line default
#line hidden
#nullable disable
        }
        ))();
        }
        #pragma warning restore 219
        #pragma warning disable 0414
        private static object __o = null;
        #pragma warning restore 0414
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 3 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/EscapedIdentifier.cshtml"
  
    var count = "1";
    var alive = true;
    var obj = new { age = (object)1 };
    var item = new { Items = new System.List<string>() { "one", "two" } };

#line default
#line hidden
#nullable disable
            __InputTagHelper = CreateTagHelper<global::InputTagHelper>();
#nullable restore
#line 9 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/EscapedIdentifier.cshtml"
__InputTagHelper.AgeProp = Convert.ToInt32(@count);

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/EscapedIdentifier.cshtml"
               __InputTagHelper.AliveProp = !@alive;

#line default
#line hidden
#nullable disable
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            __InputTagHelper = CreateTagHelper<global::InputTagHelper>();
#nullable restore
#line 10 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/EscapedIdentifier.cshtml"
__InputTagHelper.AgeProp = (int)@obj.age;

#line default
#line hidden
#nullable disable
#nullable restore
#line 10 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/EscapedIdentifier.cshtml"
     __InputTagHelper.TagProp = new { @params = 1 };

#line default
#line hidden
#nullable disable
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            __InputTagHelper = CreateTagHelper<global::InputTagHelper>();
#nullable restore
#line 11 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/EscapedIdentifier.cshtml"
__InputTagHelper.DictionaryOfBoolAndStringTupleProperty["test"] = (@item. Items.Where(i=>i.Contains("one")). Count()>0, @item. Items.FirstOrDefault(i=>i.Contains("one"))?. Replace("one",""));

#line default
#line hidden
#nullable disable
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
        }
        #pragma warning restore 1998
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; } = default!;
        #nullable disable
    }
}
#pragma warning restore 1591
