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
            __builder.OpenComponent<global::Test.MyComponent>(0);
            __builder.AddComponentParameter(1, "ParamBefore", "before");
            __builder.AddComponentParameter(2, "ParamAfter", "after");
            __builder.SetKey(
#nullable restore
#line (1,41)-(1,53) "x:\dir\subdir\Test\TestComponent.cshtml"
someDate.Day

#line default
#line hidden
#nullable disable

            );
            __builder.CloseComponent();
        }
        #pragma warning restore 1998
#nullable restore
#line (3,8)-(5,1) "x:\dir\subdir\Test\TestComponent.cshtml"

    private DateTime someDate = DateTime.Now;

#line default
#line hidden
#nullable disable

    }
}
#pragma warning restore 1591
