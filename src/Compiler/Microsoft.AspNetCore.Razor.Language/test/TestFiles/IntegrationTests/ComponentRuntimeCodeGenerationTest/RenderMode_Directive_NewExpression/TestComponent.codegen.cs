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
    [global::Test.TestComponent.__PrivateComponentRenderModeAttribute]
    #nullable restore
    public partial class TestComponent : global::Microsoft.AspNetCore.Components.ComponentBase
    #nullable disable
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
        }
        #pragma warning restore 1998
#nullable restore
#line 4 "x:\dir\subdir\Test\TestComponent.cshtml"
 
#pragma warning disable CS9113
    public class MyRenderMode(string Text) : Microsoft.AspNetCore.Components.IComponentRenderMode { }

#line default
#line hidden
#nullable disable
        private sealed class __PrivateComponentRenderModeAttribute : global::Microsoft.AspNetCore.Components.RenderModeAttribute
        {
            private static global::Microsoft.AspNetCore.Components.IComponentRenderMode ModeImpl => 
#nullable restore
#line 1 "x:\dir\subdir\Test\TestComponent.cshtml"
              new TestComponent.MyRenderMode("This is some text")

#line default
#line hidden
#nullable disable
            ;
            public override global::Microsoft.AspNetCore.Components.IComponentRenderMode Mode => ModeImpl;
        }
    }
}
#pragma warning restore 1591
