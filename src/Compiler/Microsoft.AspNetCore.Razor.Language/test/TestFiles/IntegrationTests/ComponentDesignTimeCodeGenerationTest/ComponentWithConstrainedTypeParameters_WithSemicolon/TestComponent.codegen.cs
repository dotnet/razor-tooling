﻿// <auto-generated/>
#pragma warning disable 1591
namespace Test;
#line hidden
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::System.Threading.Tasks;
#nullable restore
#line 1 "x:\dir\subdir\Test\TestComponent.cshtml"
using Microsoft.AspNetCore.Components;

#line default
#line hidden
#nullable disable
public partial class TestComponent<
#nullable restore
#line 2 "x:\dir\subdir\Test\TestComponent.cshtml"
TItem1

#line default
#line hidden
#nullable disable
,
#nullable restore
#line 3 "x:\dir\subdir\Test\TestComponent.cshtml"
TItem2

#line default
#line hidden
#nullable disable
,
#nullable restore
#line 4 "x:\dir\subdir\Test\TestComponent.cshtml"
TItem3

#line default
#line hidden
#nullable disable
> : global::Microsoft.AspNetCore.Components.ComponentBase
#nullable restore
#line 2 "x:\dir\subdir\Test\TestComponent.cshtml"
where TItem1 : Image

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "x:\dir\subdir\Test\TestComponent.cshtml"
where TItem2 : ITag

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "x:\dir\subdir\Test\TestComponent.cshtml"
where TItem3 : Image, new()

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
    ((global::System.Action)(() => {
    }
    ))();
    ((global::System.Action)(() => {
    }
    ))();
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
#nullable restore
#line 7 "x:\dir\subdir\Test\TestComponent.cshtml"
 foreach (var item2 in Items2)
{
    

#line default
#line hidden
#nullable disable
#nullable restore
#line 10 "x:\dir\subdir\Test\TestComponent.cshtml"
__o = ChildContent(item2);

#line default
#line hidden
#nullable disable
#nullable restore
#line 11 "x:\dir\subdir\Test\TestComponent.cshtml"
        
}

#line default
#line hidden
#nullable disable
    }
    #pragma warning restore 1998
#nullable restore
#line 16 "x:\dir\subdir\Test\TestComponent.cshtml"
       
    [Parameter] public TItem1 Item1 { get; set; }
    [Parameter] public List<TItem2> Items2 { get; set; }
    [Parameter] public TItem3 Item3 { get; set; }
    [Parameter] public RenderFragment<TItem2> ChildContent { get; set; }

#line default
#line hidden
#nullable disable
}
#pragma warning restore 1591
