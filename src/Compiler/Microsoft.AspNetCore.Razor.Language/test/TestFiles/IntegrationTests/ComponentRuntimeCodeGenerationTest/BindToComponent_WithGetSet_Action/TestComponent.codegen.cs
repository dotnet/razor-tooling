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
            __builder.OpenComponent<global::Test.MyComponent>(0);
            __builder.AddComponentParameter(1, nameof(global::Test.MyComponent.Value), global::Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<global::System.Int32>(
#nullable restore
#line (1,31)-(1,42) "x:\dir\subdir\Test\TestComponent.cshtml"
ParentValue

#line default
#line hidden
#nullable disable
            ));
            __builder.AddComponentParameter(2, nameof(global::Test.MyComponent.ValueChanged), (global::System.Action<System.Int32>)(
#nullable restore
#line (1,61)-(1,72) "x:\dir\subdir\Test\TestComponent.cshtml"
UpdateValue

#line default
#line hidden
#nullable disable
            ));
            __builder.CloseComponent();
        }
        #pragma warning restore 1998
#nullable restore
#line (2,8)-(6,1) "x:\dir\subdir\Test\TestComponent.cshtml"

    public int ParentValue { get; set; } = 42;

    public void UpdateValue(int value) => ParentValue = value;

#line default
#line hidden
#nullable disable

    }
}
#pragma warning restore 1591
