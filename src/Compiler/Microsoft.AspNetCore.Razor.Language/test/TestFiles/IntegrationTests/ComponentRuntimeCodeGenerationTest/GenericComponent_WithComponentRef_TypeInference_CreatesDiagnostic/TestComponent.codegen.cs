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
    #line default
    #line hidden
    #nullable restore
    public partial class TestComponent : global::Microsoft.AspNetCore.Components.ComponentBase
    #nullable disable
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            global::__Blazor.Test.TestComponent.TypeInference.CreateMyComponent_0(__builder, 0, 1, 
#nullable restore
#line (1,20)-(1,21) "x:\dir\subdir\Test\TestComponent.cshtml"
3

#line default
#line hidden
#nullable disable
            , 2, (__value) => {
#nullable restore
#line (1,29)-(1,32) "x:\dir\subdir\Test\TestComponent.cshtml"
_my

#line default
#line hidden
#nullable disable
                 = __value;
            }
            );
        }
        #pragma warning restore 1998
#nullable restore
#line (3,8)-(6,1) "x:\dir\subdir\Test\TestComponent.cshtml"

    private MyComponent<int> _my;
    public void Foo() { System.GC.KeepAlive(_my); }

#line default
#line hidden
#nullable disable

    }
}
namespace __Blazor.Test.TestComponent
{
    #line hidden
    internal static class TypeInference
    {
        public static void CreateMyComponent_0<TItem>(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder, int seq, int __seq0, TItem __arg0, int __seq1, global::System.Action<global::Test.MyComponent<TItem>> __arg1)
        {
        __builder.OpenComponent<global::Test.MyComponent<TItem>>(seq);
        __builder.AddComponentParameter(__seq0, nameof(global::Test.MyComponent<TItem>.Item), __arg0);
        __builder.AddComponentReferenceCapture(__seq1, (__value) => { __arg1((global::Test.MyComponent<TItem>)__value); });
        __builder.CloseComponent();
        }
    }
}
#pragma warning restore 1591
