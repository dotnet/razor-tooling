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
        #pragma warning disable 1998
        protected override void BuildRenderTree(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            global::__Blazor.Test.TestComponent.TypeInference.CreateTestComponent_0(__builder, 0, 1, 
#nullable restore
#line 3 "x:\dir\subdir\Test\TestComponent.cshtml"
                            RenderModeParam

#line default
#line hidden
#nullable disable
            , 2, 
#nullable restore
#line 3 "x:\dir\subdir\Test\TestComponent.cshtml"
                                                              Microsoft.AspNetCore.Components.Web.RenderMode.Server

#line default
#line hidden
#nullable disable
            );
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
        public static void CreateTestComponent_0<TRenderMode>(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder, int seq, int __seq0, Microsoft.AspNetCore.Components.IComponentRenderMode __arg0, int __seq1, TRenderMode __arg1)
            where TRenderMode : global::Microsoft.AspNetCore.Components.IComponentRenderMode
        {
        __builder.OpenComponent<global::Test.TestComponent<TRenderMode>>(seq);
        __builder.AddComponentParameter(__seq1, "RenderModeParam", __arg1);
        __builder.AddComponentRenderMode(__arg0);
        __builder.CloseComponent();
        }
    }
}
#pragma warning restore 1591
