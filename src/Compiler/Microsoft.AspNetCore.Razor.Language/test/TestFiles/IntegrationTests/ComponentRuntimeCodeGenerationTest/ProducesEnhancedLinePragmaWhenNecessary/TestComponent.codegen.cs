﻿// <auto-generated/>
#pragma warning disable 1591
namespace Test
{
    #line hidden
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Linq;
    using global::System.Threading.Tasks;
    using global::Microsoft.AspNetCore.Components;
    public partial class TestComponent : global::Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.AddMarkupContent(0, "<h1>Single line statement</h1>\r\n\r\nTime: ");
            __builder.AddContent(1, 
#nullable restore
#line (3,8)-(3,20) "x:\dir\subdir\Test\TestComponent.cshtml"
DateTime.Now

#line default
#line hidden
#nullable disable
            );
            __builder.AddMarkupContent(2, "\r\n\r\n");
            __builder.AddMarkupContent(3, "<h1>Multiline block statement</h1>\r\n\r\n");
            __builder.AddContent(4, 
#nullable restore
#line (7,2)-(10,4) "x:\dir\subdir\Test\TestComponent.cshtml"
JsonToHtml(@"{
  'key1': 'value1'
  'key2': 'value2'
}")

#line default
#line hidden
#nullable disable
            );
        }
        #pragma warning restore 1998
#nullable restore
#line (12,8)-(17,1) "x:\dir\subdir\Test\TestComponent.cshtml"

    public string JsonToHtml(string foo)
    {
        return foo;
    }

#line default
#line hidden
#nullable disable

    }
}
#pragma warning restore 1591
