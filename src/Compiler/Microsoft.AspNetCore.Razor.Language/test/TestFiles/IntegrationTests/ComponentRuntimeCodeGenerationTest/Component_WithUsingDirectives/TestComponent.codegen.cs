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
#line (3,2)-(4,1) "x:\dir\subdir\Test\TestComponent.cshtml"
using 

#line default
#line hidden
#nullable disable
#nullable restore
#line (3,2)-(4,1) "x:\dir\subdir\Test\TestComponent.cshtml"
Test2;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Components.RouteAttribute(
    // language=Route,Component
#nullable restore
#line (1,7)-(1,16) "x:\dir\subdir\Test\TestComponent.cshtml"
"/MyPage"

#line default
#line hidden
#nullable disable
    )]
    [global::Microsoft.AspNetCore.Components.RouteAttribute(
    // language=Route,Component
#nullable restore
#line (2,7)-(2,27) "x:\dir\subdir\Test\TestComponent.cshtml"
"/AnotherRoute/{id}"

#line default
#line hidden
#nullable disable
    )]
    #nullable restore
    public partial class TestComponent : global::Microsoft.AspNetCore.Components.ComponentBase
    #nullable disable
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.OpenComponent<global::Test.MyComponent>(0);
            __builder.CloseComponent();
            __builder.AddMarkupContent(1, "\r\n");
            __builder.OpenComponent<global::Test2.MyComponent2>(2);
            __builder.CloseComponent();
        }
        #pragma warning restore 1998
    }
}
#pragma warning restore 1591
