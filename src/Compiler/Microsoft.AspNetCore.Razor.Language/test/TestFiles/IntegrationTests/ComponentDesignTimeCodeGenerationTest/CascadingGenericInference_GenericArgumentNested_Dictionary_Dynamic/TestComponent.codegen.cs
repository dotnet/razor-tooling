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
    public partial class TestComponent : global::Microsoft.AspNetCore.Components.ComponentBase
    #nullable disable
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
                global::__Blazor.Test.TestComponent.TypeInference.CreateGrid_0_CaptureParameters(
#nullable restore
#line 1 "x:\dir\subdir\Test\TestComponent.cshtml"
              new Dictionary<dynamic, string>()

#line default
#line hidden
#nullable disable
                , out var __typeInferenceArg_0___arg0);
                var __typeInference_CreateGrid_0 = global::__Blazor.Test.TestComponent.TypeInference.CreateGrid_0(__builder, -1, -1, __typeInferenceArg_0___arg0, -1, (__builder2) => {
                    var __typeInference_CreateGridColumn_1 = global::__Blazor.Test.TestComponent.TypeInference.CreateGridColumn_1(__builder2, -1, __typeInferenceArg_0___arg0);
#nullable restore
#line 2 "x:\dir\subdir\Test\TestComponent.cshtml"
__o = typeof(global::Test.GridColumn<>);

#line default
#line hidden
#nullable disable
                }
                );
                #pragma warning disable BL0005
                __typeInference_CreateGrid_0.
#nullable restore
#line 1 "x:\dir\subdir\Test\TestComponent.cshtml"
      Data

#line default
#line hidden
#nullable disable
                 = default;
                #pragma warning restore BL0005
            }
#nullable restore
#line 1 "x:\dir\subdir\Test\TestComponent.cshtml"
__o = typeof(global::Test.Grid<>);

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
        public static global::Test.Grid<T> CreateGrid_0<T>(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder, int seq, int __seq0, global::System.Collections.Generic.Dictionary<dynamic, T> __arg0, int __seq1, Microsoft.AspNetCore.Components.RenderFragment __arg1)
        {
        __builder.OpenComponent<global::Test.Grid<T>>(seq);
        __builder.AddAttribute(__seq0, "Data", (object)__arg0);
        __builder.AddAttribute(__seq1, "ChildContent", (object)__arg1);
        __builder.CloseComponent();
        return default;
        }

        public static void CreateGrid_0_CaptureParameters<T>(global::System.Collections.Generic.Dictionary<dynamic, T> __arg0, out global::System.Collections.Generic.Dictionary<dynamic, T> __arg0_out)
        {
            __arg0_out = __arg0;
        }
        public static global::Test.GridColumn<T> CreateGridColumn_1<T>(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder, int seq, global::System.Collections.Generic.Dictionary<dynamic, T> __syntheticArg0)
        {
        __builder.OpenComponent<global::Test.GridColumn<T>>(seq);
        __builder.CloseComponent();
        return default;
        }
    }
}
#pragma warning restore 1591
