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
        #pragma warning disable 1998
        protected override void BuildRenderTree(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            global::__Blazor.Test.TestComponent.TypeInference.CreateMyComponent_0(__builder, 0, 1, 
#nullable restore
#line 1 "x:\dir\subdir\Test\TestComponent.cshtml"
                              ParentValue

#line default
#line hidden
#nullable disable
            , 2, __value => ParentValue = __value, 3, () => ParentValue);
        }
        #pragma warning restore 1998
#nullable restore
#line 2 "x:\dir\subdir\Test\TestComponent.cshtml"
       
    public DateTime ParentValue { get; set; } = DateTime.Now;

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
        public static void CreateMyComponent_0<T>(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder, int seq, int __seq0, T __arg0, int __seq1, global::System.Action<T> __arg1, int __seq2, global::System.Linq.Expressions.Expression<global::System.Func<T>> __arg2)
        {
        __builder.OpenComponent<global::Test.MyComponent<T>>(seq);
        __builder.AddAttribute(__seq0, "SomeParam", (object)__arg0);
        __builder.AddAttribute(__seq1, "SomeParamChanged", (object)__arg1);
        __builder.AddAttribute(__seq2, "SomeParamExpression", (object)__arg2);
        __builder.CloseComponent();
        }
    }
}
#pragma warning restore 1591
