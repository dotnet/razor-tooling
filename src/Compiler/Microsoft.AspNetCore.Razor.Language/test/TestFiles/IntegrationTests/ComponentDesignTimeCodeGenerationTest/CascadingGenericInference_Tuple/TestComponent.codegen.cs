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
            global::__Blazor.Test.TestComponent.TypeInference.CreateParentComponent_0_CaptureParameters(
#nullable restore
#line 1 "x:\dir\subdir\Test\TestComponent.cshtml"
                            (1, 2)

#line default
#line hidden
#nullable disable
            , out var __typeInferenceArg_0___arg0);
            var __typeInference_CreateParentComponent_0 = global::__Blazor.Test.TestComponent.TypeInference.CreateParentComponent_0(__builder, -1, -1, __typeInferenceArg_0___arg0, -1, (__builder2) => {
                var __typeInference_CreateChildComponent_1 = global::__Blazor.Test.TestComponent.TypeInference.CreateChildComponent_1(__builder2, -1, __typeInferenceArg_0___arg0);
#nullable restore
#line 2 "x:\dir\subdir\Test\TestComponent.cshtml"
__o = typeof(global::Test.ChildComponent<>);

#line default
#line hidden
#nullable disable
            }
            );
            #pragma warning disable BL0005
            __typeInference_CreateParentComponent_0.
#nullable restore
#line 1 "x:\dir\subdir\Test\TestComponent.cshtml"
                 Parameter

#line default
#line hidden
#nullable disable
             = default;
            #pragma warning restore BL0005
        }
#nullable restore
#line 1 "x:\dir\subdir\Test\TestComponent.cshtml"
__o = typeof(global::Test.ParentComponent<>);

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
        public static global::Test.ParentComponent<T> CreateParentComponent_0<T>(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder, int seq, int __seq0, (T, T) __arg0, int __seq1, Microsoft.AspNetCore.Components.RenderFragment __arg1)
        {
        __builder.OpenComponent<global::Test.ParentComponent<T>>(seq);
        __builder.AddAttribute(__seq0, "Parameter", (object)__arg0);
        __builder.AddAttribute(__seq1, "ChildContent", (object)__arg1);
        __builder.CloseComponent();
        return default;
        }

        public static void CreateParentComponent_0_CaptureParameters<T>((T, T) __arg0, out (T, T) __arg0_out)
        {
            __arg0_out = __arg0;
        }
        public static global::Test.ChildComponent<T> CreateChildComponent_1<T>(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder, int seq, (T, T) __syntheticArg0)
        {
        __builder.OpenComponent<global::Test.ChildComponent<T>>(seq);
        __builder.CloseComponent();
        return default;
        }
    }
}
#pragma warning restore 1591
