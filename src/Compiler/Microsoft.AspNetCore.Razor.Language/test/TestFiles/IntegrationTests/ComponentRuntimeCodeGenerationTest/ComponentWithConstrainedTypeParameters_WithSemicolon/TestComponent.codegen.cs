﻿// <auto-generated/>
#pragma warning disable 1591
namespace Test
{
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
    public partial class TestComponent<TItem1, TItem2, TItem3> : global::Microsoft.AspNetCore.Components.ComponentBase
    where TItem1 : Image
    where TItem2 : ITag
    where TItem3 : Image, new()
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.AddMarkupContent(0, "<h1>Item1</h1>");
#nullable restore
#line 7 "x:\dir\subdir\Test\TestComponent.cshtml"
 foreach (var item2 in Items2)
{

#line default
#line hidden
#nullable disable
            __builder.OpenElement(1, "p");
#nullable restore
#line (10,6)-(10,25) 24 "x:\dir\subdir\Test\TestComponent.cshtml"
__builder.AddContent(2, ChildContent(item2));

#line default
#line hidden
#nullable disable
            __builder.AddMarkupContent(3, ";\r\n    ");
            __builder.CloseElement();
#nullable restore
#line 12 "x:\dir\subdir\Test\TestComponent.cshtml"
}

#line default
#line hidden
#nullable disable
            __builder.AddMarkupContent(4, "<p>Item3</p>");
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
}
#pragma warning restore 1591
