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
            __builder.OpenComponent<global::Test.TestComponent>(0);
            __builder.AddComponentReferenceCapture(1, (__value) => {
#nullable restore
#line (1,22)-(1,33) "x:\dir\subdir\Test\TestComponent.cshtml"
myComponent

#line default
#line hidden
#nullable disable
                 = (Test.TestComponent)__value;
            }
            );
            __builder.CloseComponent();
        }
        #pragma warning restore 1998
#nullable restore
#line (3,8)-(6,1) "x:\dir\subdir\Test\TestComponent.cshtml"

    private TestComponent myComponent = null!;
    public void Use() { System.GC.KeepAlive(myComponent); }

#line default
#line hidden
#nullable disable

    }
}
#pragma warning restore 1591
