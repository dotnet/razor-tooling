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
            {
                global::__Blazor.Test.TestComponent.TypeInference.CreateParent_0_CaptureParameters(
#nullable restore
#line (1,17)-(1,73) "x:\dir\subdir\Test\TestComponent.cshtml"
new System.Collections.Generic.Dictionary<int, string>()

#line default
#line hidden
#nullable disable
                , out var __typeInferenceArg_0___arg0, 
#nullable restore
#line (1,84)-(1,101) "x:\dir\subdir\Test\TestComponent.cshtml"
DateTime.MinValue

#line default
#line hidden
#nullable disable
                , out var __typeInferenceArg_0___arg1);
                global::__Blazor.Test.TestComponent.TypeInference.CreateParent_0(__builder, 0, 1, __typeInferenceArg_0___arg0, 2, __typeInferenceArg_0___arg1, 3, (__builder2) => {
                    global::__Blazor.Test.TestComponent.TypeInference.CreateChild_1(__builder2, 4, __typeInferenceArg_0___arg1, __typeInferenceArg_0___arg0, __typeInferenceArg_0___arg0, 5, 
#nullable restore
#line (2,30)-(2,53) "x:\dir\subdir\Test\TestComponent.cshtml"
new[] { 'a', 'b', 'c' }

#line default
#line hidden
#nullable disable
                    );
                }
                );
                __typeInferenceArg_0___arg0 = default;
                __typeInferenceArg_0___arg1 = default;
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
        public static void CreateParent_0<TKey, TValue, TOther>(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder, int seq, int __seq0, global::System.Collections.Generic.Dictionary<TKey, TValue> __arg0, int __seq1, TOther __arg1, int __seq2, global::Microsoft.AspNetCore.Components.RenderFragment __arg2)
        {
        __builder.OpenComponent<global::Test.Parent<TKey, TValue, TOther>>(seq);
        __builder.AddComponentParameter(__seq0, "Data", __arg0);
        __builder.AddComponentParameter(__seq1, "Other", __arg1);
        __builder.AddComponentParameter(__seq2, "ChildContent", __arg2);
        __builder.CloseComponent();
        }

        public static void CreateParent_0_CaptureParameters<TKey, TValue, TOther>(global::System.Collections.Generic.Dictionary<TKey, TValue> __arg0, out global::System.Collections.Generic.Dictionary<TKey, TValue> __arg0_out, TOther __arg1, out TOther __arg1_out)
        {
            __arg0_out = __arg0;
            __arg1_out = __arg1;
        }
        public static void CreateChild_1<TOther, TValue, TKey, TChildOnly>(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder, int seq, TOther __syntheticArg0, global::System.Collections.Generic.Dictionary<TKey, TValue> __syntheticArg1, global::System.Collections.Generic.Dictionary<TKey, TValue> __syntheticArg2, int __seq0, global::System.Collections.Generic.ICollection<TChildOnly> __arg0)
        {
        __builder.OpenComponent<global::Test.Child<TOther, TValue, TKey, TChildOnly>>(seq);
        __builder.AddComponentParameter(__seq0, "ChildOnlyItems", __arg0);
        __builder.CloseComponent();
        }
    }
}
#pragma warning restore 1591
