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
            , 2, 
#nullable restore
#line (1,45)-(1,46) "x:\dir\subdir\Test\TestComponent.cshtml"
c

#line default
#line hidden
#nullable disable
            );
        }
        #pragma warning restore 1998
#nullable restore
#line (3,8)-(5,1) "x:\dir\subdir\Test\TestComponent.cshtml"

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
        public static void CreateMyComponent_0<T>(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder, int seq, int __seq0, global::Test.MyClass<T> __arg0, int __seq1, global::System.Boolean __arg1)
        {
        __builder.OpenComponent<global::Test.MyComponent<T>>(seq);
        __builder.AddAttribute(__seq0, nameof(global::Test.MyComponent<T>.MyParameter), (object)__arg0);
        __builder.AddAttribute(__seq1, nameof(global::Test.MyComponent<T>.BoolParameter), (object)__arg1);
        __builder.CloseComponent();
        }
    }
}
#pragma warning restore 1591
