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
    public partial class TestComponent<
#nullable restore
#line 1 "x:\dir\subdir\Test\TestComponent.cshtml"
TRenderMode

#line default
#line hidden
#nullable disable
    > : global::Microsoft.AspNetCore.Components.ComponentBase
#nullable restore
#line 1 "x:\dir\subdir\Test\TestComponent.cshtml"
where TRenderMode : Microsoft.AspNetCore.Components.IComponentRenderMode

#line default
#line hidden
#nullable disable
    {
        #pragma warning disable 219
        private void __RazorDirectiveTokenHelpers__() {
        ((global::System.Action)(() => {
        }
        ))();
        ((global::System.Action)(() => {
        }
        ))();
        }
        #pragma warning restore 219
        #pragma warning disable 0414
        private static object __o = null;
        #pragma warning restore 0414
        #pragma warning disable 1998
        protected override void BuildRenderTree(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            var __typeInference_CreateTestComponent_0 = global::__Blazor.Test.TestComponent.TypeInference.CreateTestComponent_0(__builder, -1, -1, 
#nullable restore
#line 3 "x:\dir\subdir\Test\TestComponent.cshtml"
                              RenderModeParam

#line default
#line hidden
#nullable disable
            , -1, 
#nullable restore
#line 3 "x:\dir\subdir\Test\TestComponent.cshtml"
                                                                   Microsoft.AspNetCore.Components.DefaultRenderModes.Server

#line default
#line hidden
#nullable disable
            );
            #pragma warning disable BL0005
            __typeInference_CreateTestComponent_0.
#nullable restore
#line 3 "x:\dir\subdir\Test\TestComponent.cshtml"
                                                RenderModeParam

#line default
#line hidden
#nullable disable
             = default;
            #pragma warning restore BL0005
#nullable restore
#line 3 "x:\dir\subdir\Test\TestComponent.cshtml"
__o = typeof(global::Test.TestComponent<>);

#line default
#line hidden
#nullable disable
        }
        #pragma warning restore 1998
#nullable restore
#line 6 "x:\dir\subdir\Test\TestComponent.cshtml"
 
    [Parameter] public TRenderMode RenderModeParam { get; set;}

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
        public static global::Test.TestComponent<TRenderMode> CreateTestComponent_0<TRenderMode>(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder, int seq, int __seq0, Microsoft.AspNetCore.Components.IComponentRenderMode __arg0, int __seq1, TRenderMode __arg1)
            where TRenderMode : global::Microsoft.AspNetCore.Components.IComponentRenderMode
        {
        __builder.OpenComponent<global::Test.TestComponent<TRenderMode>>(seq);
        __builder.AddComponentParameter(__seq1, "RenderModeParam", __arg1);
        __builder.SetRenderMode(__arg0);
        __builder.CloseComponent();
        return default;
        }
    }
}
#pragma warning restore 1591
