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
        var __typeInference_CreateGrid_0 = __TypeInferenceGrid_0.CreateGrid_0(__builder, -1, -1, 
#nullable restore
#line 1 "x:\dir\subdir\Test\TestComponent.cshtml"
               Array.Empty<DateTime>()

#line default
#line hidden
#nullable disable
        , -1, (__builder2) => {
#nullable restore
#line 1 "x:\dir\subdir\Test\TestComponent.cshtml"
__o = typeof(global::Test.Column<>);

#line default
#line hidden
#nullable disable
        }
        );
        #pragma warning disable BL0005
        __typeInference_CreateGrid_0.
#nullable restore
#line 1 "x:\dir\subdir\Test\TestComponent.cshtml"
      Items

#line default
#line hidden
#nullable disable
         = default;
        #pragma warning restore BL0005
#nullable restore
#line 1 "x:\dir\subdir\Test\TestComponent.cshtml"
__o = typeof(global::Test.Grid<>);

#line default
#line hidden
#nullable disable
    }
    #pragma warning restore 1998
}
internal static class __TypeInferenceGrid_0
{
    public static global::Test.Grid<TItem> CreateGrid_0<TItem>(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder, int seq, int __seq0, global::System.Collections.Generic.IEnumerable<TItem> __arg0, int __seq1, global::Microsoft.AspNetCore.Components.RenderFragment __arg1)
    {
    __builder.OpenComponent<global::Test.Grid<TItem>>(seq);
    __builder.AddAttribute(__seq0, "Items", (object)__arg0);
    __builder.AddAttribute(__seq1, "ChildContent", (object)__arg1);
    __builder.CloseComponent();
    return default;
    }
}
internal static class __TypeInferenceColumn_1
{
    public static global::Test.Column<System.Object> CreateColumn_1<TItem>(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder, int seq)
    {
    __builder.OpenComponent<global::Test.Column<System.Object>>(seq);
    __builder.CloseComponent();
    return default;
    }
}
#pragma warning restore 1591
