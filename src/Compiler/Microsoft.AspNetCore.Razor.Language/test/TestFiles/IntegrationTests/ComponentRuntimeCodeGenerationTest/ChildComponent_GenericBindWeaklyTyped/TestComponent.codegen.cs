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
        #pragma warning disable 1998
        protected override void BuildRenderTree(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.OpenComponent<global::Test.MyComponent<string>>(0);
            __builder.AddAttribute(1, "Item", (object)(
#nullable restore
#line 1 "x:\dir\subdir\Test\TestComponent.cshtml"
                                     Value

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(2, "ItemChanged", (object)(global::Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.CreateInferredEventCallback(this, __value => Value = __value, Value)));
            __builder.CloseComponent();
        }
        #pragma warning restore 1998
#nullable restore
#line 2 "x:\dir\subdir\Test\TestComponent.cshtml"
       
    string Value;

#line default
#line hidden
#nullable disable
    }
}
#pragma warning restore 1591
