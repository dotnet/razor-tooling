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
#nullable restore
#line (1,2)-(2,1) 1 "x:\dir\subdir\Test\TestComponent.cshtml"
using Microsoft.AspNetCore.Components.Web

#line default
#line hidden
#nullable disable
    ;
    public partial class TestComponent : global::Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.OpenElement(0, "form");
            __builder.AddAttribute(1, "method", "post");
            __builder.AddAttribute(2, "onsubmit", global::Microsoft.AspNetCore.Components.EventCallback.Factory.Create<global::System.EventArgs>(this, 
#nullable restore
#line 2 "x:\dir\subdir\Test\TestComponent.cshtml"
                               () => { }

#line default
#line hidden
#nullable disable
            ));
            string __formName = global::Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<string>(
#nullable restore
#line 2 "x:\dir\subdir\Test\TestComponent.cshtml"
                                                      x

#line default
#line hidden
#nullable disable
            );
            __builder.AddNamedEvent("onsubmit", __formName);
            __builder.CloseElement();
            __builder.AddMarkupContent(3, "\r\n");
            __builder.OpenElement(4, "form");
            __builder.AddAttribute(5, "method", "post");
            __builder.AddAttribute(6, "onsubmit", global::Microsoft.AspNetCore.Components.EventCallback.Factory.Create<global::System.EventArgs>(this, 
#nullable restore
#line 3 "x:\dir\subdir\Test\TestComponent.cshtml"
                               () => { }

#line default
#line hidden
#nullable disable
            ));
            string __formName1_1 = global::Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<string>(
#nullable restore
#line 3 "x:\dir\subdir\Test\TestComponent.cshtml"
                                                      y

#line default
#line hidden
#nullable disable
            );
            __builder.AddNamedEvent("onsubmit", __formName1_1);
            __builder.CloseElement();
        }
        #pragma warning restore 1998
#nullable restore
#line 4 "x:\dir\subdir\Test\TestComponent.cshtml"
       
    string x = "a";
    string y = "b";

#line default
#line hidden
#nullable disable
    }
}
#pragma warning restore 1591
