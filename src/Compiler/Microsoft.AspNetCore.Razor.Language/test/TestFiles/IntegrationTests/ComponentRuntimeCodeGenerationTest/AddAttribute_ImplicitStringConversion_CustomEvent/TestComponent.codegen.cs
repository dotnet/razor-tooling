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
#line (1,27)-(1,28) "x:\dir\subdir\Test\TestComponent.cshtml"
c

#line default
#line hidden
#nullable disable
            , 2, global::Microsoft.AspNetCore.Components.EventCallback.Factory.Create(this, 
#nullable restore
#line (2,14)-(2,23) "x:\dir\subdir\Test\TestComponent.cshtml"
() => { }

#line default
#line hidden
#nullable disable
            ), 3, 
#nullable restore
#line (3,20)-(3,24) "x:\dir\subdir\Test\TestComponent.cshtml"
true

#line default
#line hidden
#nullable disable
            , 4, "str", 5, 
#nullable restore
#line (5,24)-(5,33) "x:\dir\subdir\Test\TestComponent.cshtml"
() => { }

#line default
#line hidden
#nullable disable
            , 6, 
#nullable restore
#line (6,22)-(6,23) "x:\dir\subdir\Test\TestComponent.cshtml"
c

#line default
#line hidden
#nullable disable
            );
        }
        #pragma warning restore 1998
#nullable restore
#line (8,8)-(10,1) "x:\dir\subdir\Test\TestComponent.cshtml"

    private MyClass<string> c = new();

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
        public static void CreateMyComponent_0<T>(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder, int seq, int __seq0, global::Test.MyClass<T> __arg0, int __seq1, global::Microsoft.AspNetCore.Components.EventCallback __arg1, int __seq2, global::System.Boolean __arg2, int __seq3, global::System.String __arg3, int __seq4, global::System.Delegate __arg4, int __seq5, global::System.Object __arg5)
        {
        __builder.OpenComponent<global::Test.MyComponent<T>>(seq);
        __builder.AddAttribute(__seq0, nameof(global::Test.MyComponent<T>.MyParameter), (object)__arg0);
        __builder.AddAttribute(__seq1, nameof(global::Test.MyComponent<T>.MyEvent), (object)__arg1);
        __builder.AddAttribute(__seq2, nameof(global::Test.MyComponent<T>.BoolParameter), (object)__arg2);
        __builder.AddAttribute(__seq3, nameof(global::Test.MyComponent<T>.StringParameter), (object)__arg3);
        __builder.AddAttribute(__seq4, nameof(global::Test.MyComponent<T>.DelegateParameter), (object)__arg4);
        __builder.AddAttribute(__seq5, nameof(global::Test.MyComponent<T>.ObjectParameter), (object)__arg5);
        __builder.CloseComponent();
        }
    }
}
#pragma warning restore 1591
