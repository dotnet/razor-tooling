﻿// <auto-generated/>
#pragma warning disable 1591
namespace Test
{
    #line default
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Linq;
    using global::System.Threading.Tasks;
    using global::Microsoft.AspNetCore.Components;
#nullable restore
#line (1,2)-(2,1) "x:\dir\subdir\Test\TestComponent.cshtml"
using 

#line default
#line hidden
#nullable disable
#nullable restore
#line (1,2)-(2,1) "x:\dir\subdir\Test\TestComponent.cshtml"
Microsoft.AspNetCore.Components.Web;

#line default
#line hidden
#nullable disable
    #nullable restore
    public partial class TestComponent : global::Microsoft.AspNetCore.Components.ComponentBase
    #nullable disable
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.OpenElement(0, "div");
            __builder.OpenElement(1, "input");
            __builder.AddAttribute(2, "type", "text");
            __builder.AddAttribute(3, "value", "17");
            __builder.AddAttribute(4, "value", global::Microsoft.AspNetCore.Components.BindConverter.FormatValue(
#nullable restore
#line (3,41)-(3,45) "x:\dir\subdir\Test\TestComponent.cshtml"
text

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(5, "onchange", global::Microsoft.AspNetCore.Components.EventCallback.Factory.CreateBinder(this, __value => text = __value, text));
            __builder.SetUpdatesAttributeName("value");
            __builder.CloseElement();
            __builder.CloseElement();
        }
        #pragma warning restore 1998
#nullable restore
#line (5,13)-(7,1) "x:\dir\subdir\Test\TestComponent.cshtml"

    private string text = "hi";

#line default
#line hidden
#nullable disable

    }
}
#pragma warning restore 1591
