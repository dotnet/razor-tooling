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
        {
            global::__Blazor.Test.TestComponent.TypeInference.CreateParentOne_0_CaptureParameters(
#nullable restore
#line 1 "x:\dir\subdir\Test\TestComponent.cshtml"
                   int.MaxValue

#line default
#line hidden
#nullable disable
            , out var __typeInferenceArg_0___arg0);
            var __typeInference_CreateParentOne_0 = global::__Blazor.Test.TestComponent.TypeInference.CreateParentOne_0(__builder, -1, -1, __typeInferenceArg_0___arg0, -1, (__builder2) => {
                {
                    global::__Blazor.Test.TestComponent.TypeInference.CreateParentTwo_1_CaptureParameters(
#nullable restore
#line 2 "x:\dir\subdir\Test\TestComponent.cshtml"
                        "Hello"

#line default
#line hidden
#nullable disable
                    , out var __typeInferenceArg_1___arg0);
                    var __typeInference_CreateParentTwo_1 = global::__Blazor.Test.TestComponent.TypeInference.CreateParentTwo_1(__builder2, -1, -1, __typeInferenceArg_1___arg0, -1, (__builder3) => {
                        var __typeInference_CreateChild_2 = global::__Blazor.Test.TestComponent.TypeInference.CreateChild_2(__builder3, -1, __typeInferenceArg_0___arg0, __typeInferenceArg_1___arg0);
#nullable restore
#line 3 "x:\dir\subdir\Test\TestComponent.cshtml"
__o = typeof(global::Test.Child<,>);

#line default
#line hidden
#nullable disable
                    }
                    );
                    #pragma warning disable BL0005
                    __typeInference_CreateParentTwo_1.
#nullable restore
#line 2 "x:\dir\subdir\Test\TestComponent.cshtml"
               Value

#line default
#line hidden
#nullable disable
                     = default;
                    #pragma warning restore BL0005
                }
#nullable restore
#line 2 "x:\dir\subdir\Test\TestComponent.cshtml"
__o = typeof(global::Test.ParentTwo<>);

#line default
#line hidden
#nullable disable
            }
            );
            #pragma warning disable BL0005
            __typeInference_CreateParentOne_0.
#nullable restore
#line 1 "x:\dir\subdir\Test\TestComponent.cshtml"
           Value

#line default
#line hidden
#nullable disable
             = default;
            #pragma warning restore BL0005
        }
#nullable restore
#line 1 "x:\dir\subdir\Test\TestComponent.cshtml"
__o = typeof(global::Test.ParentOne<>);

#line default
#line hidden
#nullable disable
    }
    #pragma warning restore 1998
}
namespace __Blazor.Test.TestComponent
{
    #line hidden
    internal static class TypeInference
    {
        public static global::Test.ParentOne<TOne> CreateParentOne_0<TOne>(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder, int seq, int __seq0, TOne __arg0, int __seq1, global::Microsoft.AspNetCore.Components.RenderFragment __arg1)
        {
        __builder.OpenComponent<global::Test.ParentOne<TOne>>(seq);
        __builder.AddAttribute(__seq0, "Value", (object)__arg0);
        __builder.AddAttribute(__seq1, "ChildContent", (object)__arg1);
        __builder.CloseComponent();
        return default;
        }

        public static void CreateParentOne_0_CaptureParameters<TOne>(TOne __arg0, out TOne __arg0_out)
        {
            __arg0_out = __arg0;
        }
        public static global::Test.ParentTwo<TTwo> CreateParentTwo_1<TTwo>(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder, int seq, int __seq0, TTwo __arg0, int __seq1, global::Microsoft.AspNetCore.Components.RenderFragment __arg1)
        {
        __builder.OpenComponent<global::Test.ParentTwo<TTwo>>(seq);
        __builder.AddAttribute(__seq0, "Value", (object)__arg0);
        __builder.AddAttribute(__seq1, "ChildContent", (object)__arg1);
        __builder.CloseComponent();
        return default;
        }

        public static void CreateParentTwo_1_CaptureParameters<TTwo>(TTwo __arg0, out TTwo __arg0_out)
        {
            __arg0_out = __arg0;
        }
        public static global::Test.Child<TOne, TTwo> CreateChild_2<TOne, TTwo>(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder, int seq, TOne __syntheticArg0, TTwo __syntheticArg1)
        {
        __builder.OpenComponent<global::Test.Child<TOne, TTwo>>(seq);
        __builder.CloseComponent();
        return default;
        }
    }
}
#pragma warning restore 1591
