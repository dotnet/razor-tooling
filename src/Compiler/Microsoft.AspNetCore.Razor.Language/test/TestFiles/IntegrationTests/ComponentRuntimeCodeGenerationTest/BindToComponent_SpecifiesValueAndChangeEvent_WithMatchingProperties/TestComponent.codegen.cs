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
            __builder.OpenComponent<global::Test.MyComponent>(0);
            __builder.AddComponentParameter(1, "Value", global::Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<global::System.Int32>(
#nullable restore
#line (1,27)-(1,38) "x:\dir\subdir\Test\TestComponent.cshtml"
ParentValue

#line default
#line hidden
#nullable disable
            ));
            __builder.AddComponentParameter(2, "OnChanged", (global::System.Action<System.Int32>)(__value => ParentValue = __value));
            __builder.CloseComponent();
        }
        #pragma warning restore 1998
#nullable restore
#line (2,8)-(4,1) "x:\dir\subdir\Test\TestComponent.cshtml"

    public int ParentValue { get; set; } = 42;

#line default
#line hidden
#nullable disable

    }
}
#pragma warning restore 1591
