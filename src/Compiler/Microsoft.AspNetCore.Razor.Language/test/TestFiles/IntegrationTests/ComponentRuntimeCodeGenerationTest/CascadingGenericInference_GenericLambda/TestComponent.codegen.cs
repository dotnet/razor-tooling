﻿// <auto-generated/>
#pragma warning disable 1591
namespace Test
{
    #line hidden
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Linq;
    using global::System.Threading.Tasks;
    using global::Microsoft.AspNetCore.Components;
    public partial class TestComponent : global::Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            {
                global::__Blazor.Test.TestComponent.TypeInference.CreateGrid_0_CaptureParameters(
#nullable restore
#line (1,16)-(1,39) "x:\dir\subdir\Test\TestComponent.cshtml"
Array.Empty<DateTime>()

#line default
#line hidden
#nullable disable
                , out var __typeInferenceArg_0___arg0);
                global::__Blazor.Test.TestComponent.TypeInference.CreateGrid_0(__builder, 0, 1, __typeInferenceArg_0___arg0, 2, (__builder2) => {
                    global::__Blazor.Test.TestComponent.TypeInference.CreateColumn_1(__builder2, 3, __typeInferenceArg_0___arg0, 4, 
#nullable restore
#line (1,64)-(1,75) "x:\dir\subdir\Test\TestComponent.cshtml"
x => x.Year

#line default
#line hidden
#nullable disable
                    );
                }
                );
                __typeInferenceArg_0___arg0 = default;
            }
        }
        #pragma warning restore 1998
    }
}
namespace __Blazor.Test.TestComponent
{
    #line hidden
    internal static class TypeInference
    {
        public static void CreateGrid_0<TItem>(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder, int seq, int __seq0, global::System.Collections.Generic.IEnumerable<TItem> __arg0, int __seq1, global::Microsoft.AspNetCore.Components.RenderFragment __arg1)
        {
        __builder.OpenComponent<global::Test.Grid<TItem>>(seq);
        __builder.AddComponentParameter(__seq0, "Items", __arg0);
        __builder.AddComponentParameter(__seq1, "ChildContent", __arg1);
        __builder.CloseComponent();
        }

        public static void CreateGrid_0_CaptureParameters<TItem>(global::System.Collections.Generic.IEnumerable<TItem> __arg0, out global::System.Collections.Generic.IEnumerable<TItem> __arg0_out)
        {
            __arg0_out = __arg0;
        }
        public static void CreateColumn_1<TItem, TOutput>(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder, int seq, global::System.Collections.Generic.IEnumerable<TItem> __syntheticArg0, int __seq0, global::System.Func<TItem, TOutput> __arg0)
        {
        __builder.OpenComponent<global::Test.Column<TItem, TOutput>>(seq);
        __builder.AddComponentParameter(__seq0, "SomeLambda", __arg0);
        __builder.CloseComponent();
        }
    }
}
#pragma warning restore 1591
