﻿// <auto-generated/>
#pragma warning disable 1591
namespace Test;
#line hidden
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::System.Threading.Tasks;
using global::Microsoft.AspNetCore.Components;
#nullable restore
#line 1 "x:\dir\subdir\Test\TestComponent.cshtml"
using System.Globalization;

#line default
#line hidden
#nullable disable
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
        __o = global::Microsoft.AspNetCore.Components.BindConverter.FormatValue(
#nullable restore
#line 2 "x:\dir\subdir\Test\TestComponent.cshtml"
                                   ParentValue

#line default
#line hidden
#nullable disable
        );
        __o = global::Microsoft.AspNetCore.Components.EventCallback.Factory.CreateBinder(this, __value => ParentValue = __value, ParentValue);
    }
    #pragma warning restore 1998
#nullable restore
#line 3 "x:\dir\subdir\Test\TestComponent.cshtml"
       
    public int ParentValue { get; set; }

#line default
#line hidden
#nullable disable
}
#pragma warning restore 1591
