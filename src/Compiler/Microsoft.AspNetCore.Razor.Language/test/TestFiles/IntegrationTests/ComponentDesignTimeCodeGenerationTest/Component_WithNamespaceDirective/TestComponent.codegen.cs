﻿// <auto-generated/>
#pragma warning disable 1591
namespace AnotherTest
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
#nullable restore
#line 1 "x:\dir\subdir\Test\TestComponent.cshtml"
using Test;

#line default
#line hidden
#nullable disable
    public partial class TestComponent : global::Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 219
        private void __RazorDirectiveTokenHelpers__() {
        ((global::System.Action)(() => {
#nullable restore
#line 2 "x:\dir\subdir\Test\TestComponent.cshtml"
global::System.Object __typeHelper = nameof(AnotherTest);

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
        protected override void BuildRenderTree(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __o = "";
            __builder.AddAttribute(-1, "ChildContent", (global::Microsoft.AspNetCore.Components.RenderFragment)((__builder2) => {
            }
            ));
            __o = ((global::Test.HeaderComponent)default).
#nullable restore
#line 4 "x:\dir\subdir\Test\TestComponent.cshtml"
                 Header

#line default
#line hidden
#nullable disable
            ;
#nullable restore
#line 4 "x:\dir\subdir\Test\TestComponent.cshtml"
__o = typeof(global::Test.HeaderComponent);

#line default
#line hidden
#nullable disable
            __o = "";
            __builder.AddAttribute(-1, "ChildContent", (global::Microsoft.AspNetCore.Components.RenderFragment)((__builder2) => {
            }
            ));
            __o = ((global::AnotherTest.FooterComponent)default).
#nullable restore
#line 6 "x:\dir\subdir\Test\TestComponent.cshtml"
                 Footer

#line default
#line hidden
#nullable disable
            ;
#nullable restore
#line 6 "x:\dir\subdir\Test\TestComponent.cshtml"
__o = typeof(global::AnotherTest.FooterComponent);

#line default
#line hidden
#nullable disable
        }
        #pragma warning restore 1998
    }
}
#pragma warning restore 1591
