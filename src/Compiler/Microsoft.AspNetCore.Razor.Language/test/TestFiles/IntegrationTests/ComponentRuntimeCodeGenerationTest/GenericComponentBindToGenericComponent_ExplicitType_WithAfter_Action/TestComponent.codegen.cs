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
    public partial class TestComponent<TParam> : global::Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.OpenComponent<global::Test.MyComponent<TParam>>(0);
            __builder.AddAttribute(1, "Value", global::Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<TParam>(
#nullable restore
#line 2 "x:\dir\subdir\Test\TestComponent.cshtml"
                                              ParentValue

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(2, "ValueChanged", (global::System.Action<TParam>)(__value => { ParentValue = __value; global::Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.InvokeSynchronousDelegate(Update); }));
            __builder.CloseComponent();
        }
        #pragma warning restore 1998
#nullable restore
#line 3 "x:\dir\subdir\Test\TestComponent.cshtml"
       
    public TParam ParentValue { get; set; }

    public void Update() { }

#line default
#line hidden
#nullable disable
    }
}
#pragma warning restore 1591
