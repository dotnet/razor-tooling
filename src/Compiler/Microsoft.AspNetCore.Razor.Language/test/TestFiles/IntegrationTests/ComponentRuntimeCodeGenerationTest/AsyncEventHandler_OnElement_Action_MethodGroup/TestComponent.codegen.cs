﻿// <auto-generated/>
#pragma warning disable 1591
namespace Test
{
    #line hidden
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Linq;
    using global::Microsoft.AspNetCore.Components;
#nullable restore
#line (1,2)-(2,1) "x:\dir\subdir\Test\TestComponent.cshtml"
using System.Threading.Tasks

#line default
#line hidden
#nullable disable
    ;
#nullable restore
#line (2,2)-(3,1) "x:\dir\subdir\Test\TestComponent.cshtml"
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
            __builder.OpenElement(0, "input");
            __builder.AddAttribute(1, "onclick", global::Microsoft.AspNetCore.Components.EventCallback.Factory.Create<global::Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line (3,18)-(3,25) "x:\dir\subdir\Test\TestComponent.cshtml"
OnClick

#line default
#line hidden
#nullable disable
            ));
            __builder.CloseElement();
        }
        #pragma warning restore 1998
#nullable restore
#line (4,8)-(9,1) "x:\dir\subdir\Test\TestComponent.cshtml"

    Task OnClick()
    {
        return Task.CompletedTask;
    }

#line default
#line hidden
#nullable disable

    }
}
#pragma warning restore 1591
