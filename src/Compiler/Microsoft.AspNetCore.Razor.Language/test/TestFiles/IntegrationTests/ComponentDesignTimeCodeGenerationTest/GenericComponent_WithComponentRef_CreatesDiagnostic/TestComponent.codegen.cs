﻿// <auto-generated/>
#pragma warning disable 1591
namespace Test;
#line hidden
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::System.Threading.Tasks;
using global::Microsoft.AspNetCore.Components;
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
        __o = typeof(
#nullable restore
#line 1 "x:\dir\subdir\Test\TestComponent.cshtml"
                   int

#line default
#line hidden
#nullable disable
        );
        __o = global::Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<int>(
#nullable restore
#line 1 "x:\dir\subdir\Test\TestComponent.cshtml"
                             3

#line default
#line hidden
#nullable disable
        );
        __builder.AddAttribute(-1, "ChildContent", (global::Microsoft.AspNetCore.Components.RenderFragment)((__builder2) => {
        }
        ));
#nullable restore
#line 1 "x:\dir\subdir\Test\TestComponent.cshtml"
                                      _my = default(global::Test.MyComponent<int>)!;

#line default
#line hidden
#nullable disable
        #pragma warning disable BL0005
        ((global::Test.MyComponent<int>)default).
#nullable restore
#line 1 "x:\dir\subdir\Test\TestComponent.cshtml"
                       Item

#line default
#line hidden
#nullable disable
         = default;
        #pragma warning restore BL0005
#nullable restore
#line 1 "x:\dir\subdir\Test\TestComponent.cshtml"
__o = typeof(global::Test.MyComponent<>);

#line default
#line hidden
#nullable disable
    }
    #pragma warning restore 1998
#nullable restore
#line 3 "x:\dir\subdir\Test\TestComponent.cshtml"
       
    private MyComponent<int> _my;
    public void Foo() { System.GC.KeepAlive(_my); }

#line default
#line hidden
#nullable disable
}
#pragma warning restore 1591
