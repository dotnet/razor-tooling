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
            {
                global::__Blazor.Test.TestComponent.TypeInference.CreateAncestor_0_CaptureParameters(
#nullable restore
#line 1 "x:\dir\subdir\Test\TestComponent.cshtml"
                   Array.Empty<DateTime>()

#line default
#line hidden
#nullable disable
                , out var __typeInferenceArg_0___arg0);
                global::__Blazor.Test.TestComponent.TypeInference.CreateAncestor_0(__builder, 0, 1, __typeInferenceArg_0___arg0, 2, (__builder2) => {
                    __builder2.OpenComponent<global::Test.Passthrough>(3);
                    __builder2.AddAttribute(4, "ChildContent", (global::Microsoft.AspNetCore.Components.RenderFragment)((__builder3) => {
                        global::__Blazor.Test.TestComponent.TypeInference.CreateChild_1(__builder3, 5, __typeInferenceArg_0___arg0);
                    }
                    ));
                    __builder2.CloseComponent();
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
        public static void CreateAncestor_0<TItem>(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder, int seq, int __seq0, global::System.Collections.Generic.IEnumerable<TItem> __arg0, int __seq1, global::Microsoft.AspNetCore.Components.RenderFragment __arg1)
        {
        __builder.OpenComponent<global::Test.Ancestor<TItem>>(seq);
        __builder.AddAttribute(__seq0, "Items", (object)__arg0);
        __builder.AddAttribute(__seq1, "ChildContent", (object)__arg1);
        __builder.CloseComponent();
        }

        public static void CreateAncestor_0_CaptureParameters<TItem>(global::System.Collections.Generic.IEnumerable<TItem> __arg0, out global::System.Collections.Generic.IEnumerable<TItem> __arg0_out)
        {
            __arg0_out = __arg0;
        }
        public static void CreateChild_1<TItem>(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder, int seq, System.Collections.Generic.IEnumerable<TItem> __syntheticArg0)
        {
        __builder.OpenComponent<global::Test.Child<TItem>>(seq);
        __builder.CloseComponent();
        }
    }
}
#pragma warning restore 1591
