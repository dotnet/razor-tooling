﻿// <auto-generated/>
#pragma warning disable 1591
namespace Test
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
    public partial class TestComponent : global::Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 219
        private void __RazorDirectiveTokenHelpers__() {
        }
        #pragma warning restore 219
        #pragma warning disable 0414
        private static object __o = null;
        #pragma warning restore 0414
        #pragma warning disable 1998
        protected override void BuildRenderTree(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __o = global::Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<(System.Int32 Horizontal, System.Int32 Vertical)>(
#nullable restore
#line 5 "x:\dir\subdir\Test\TestComponent.cshtml"
                       (32, 16)

#line default
#line hidden
#nullable disable
            );
            __builder.AddAttribute(-1, "ChildContent", (global::Microsoft.AspNetCore.Components.RenderFragment)((__builder2) => {
            }
            ));
            __o = ((global::Test.TestComponent)default).
#nullable restore
#line 5 "x:\dir\subdir\Test\TestComponent.cshtml"
               Gutter

#line default
#line hidden
#nullable disable
            ;
#nullable restore
#line 5 "x:\dir\subdir\Test\TestComponent.cshtml"
__o = typeof(global::Test.TestComponent);

#line default
#line hidden
#nullable disable
        }
        #pragma warning restore 1998
#nullable restore
#line 1 "x:\dir\subdir\Test\TestComponent.cshtml"
       
    [Parameter] public (int Horizontal, int Vertical) Gutter { get; set; }

#line default
#line hidden
#nullable disable
    }
}
#pragma warning restore 1591
