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
        private static System.Object __o = null;
        #pragma warning restore 0414
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
                var __typeInference_CreateAncestor_0 = global::__Blazor.Test.TestComponent.TypeInference.CreateAncestor_0(__builder, -1, -1, __typeInferenceArg_0___arg0, -1, (__builder2) => {
                    __builder2.AddAttribute(-1, "ChildContent", (global::Microsoft.AspNetCore.Components.RenderFragment)((__builder3) => {
                        var __typeInference_CreateChild_1 = global::__Blazor.Test.TestComponent.TypeInference.CreateChild_1(__builder3, -1, __typeInferenceArg_0___arg0);
#nullable restore
#line 1 "x:\dir\subdir\Test\TestComponent.cshtml"
__o = typeof(global::Test.Child<>);

#line default
#line hidden
#nullable disable
                    }
                    ));
#nullable restore
#line 1 "x:\dir\subdir\Test\TestComponent.cshtml"
__o = typeof(global::Test.Passthrough);

#line default
#line hidden
#nullable disable
                }
                );
                __o = __typeInference_CreateAncestor_0.
#nullable restore
#line 1 "x:\dir\subdir\Test\TestComponent.cshtml"
          Items

#line default
#line hidden
#nullable disable
                ;
            }
#nullable restore
#line 1 "x:\dir\subdir\Test\TestComponent.cshtml"
__o = typeof(global::Test.Ancestor<>);

#line default
#line hidden
#nullable disable
        }
        #pragma warning restore 1998
    }
}
namespace __Blazor.Test.TestComponent
{
    #line hidden
    internal static class TypeInference
    {
        public static global::Test.Ancestor<TItem> CreateAncestor_0<TItem>(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder, int seq, int __seq0, global::System.Collections.Generic.IEnumerable<TItem> __arg0, int __seq1, global::Microsoft.AspNetCore.Components.RenderFragment __arg1)
        {
        __builder.OpenComponent<global::Test.Ancestor<TItem>>(seq);
        __builder.AddAttribute(__seq0, "Items", __arg0);
        __builder.AddAttribute(__seq1, "ChildContent", __arg1);
        __builder.CloseComponent();
        return default;
        }

        public static void CreateAncestor_0_CaptureParameters<TItem>(global::System.Collections.Generic.IEnumerable<TItem> __arg0, out global::System.Collections.Generic.IEnumerable<TItem> __arg0_out)
        {
            __arg0_out = __arg0;
        }
        public static global::Test.Child<TItem> CreateChild_1<TItem>(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder, int seq, global::System.Collections.Generic.IEnumerable<TItem> __syntheticArg0)
        {
        __builder.OpenComponent<global::Test.Child<TItem>>(seq);
        __builder.CloseComponent();
        return default;
        }
    }
}
#pragma warning restore 1591
