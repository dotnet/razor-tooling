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
#line (1,2)-(2,1) "x:\dir\subdir\Test\TestComponent.cshtml"
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
    > : global::Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.AddMarkupContent(0, "<h1>Item</h1>\r\n\r\n");
            __builder.OpenElement(1, "p");
            __builder.AddContent(2, 
#nullable restore
#line (7,5)-(7,24) "x:\dir\subdir\Test\TestComponent.cshtml"
ChildContent(Item1)

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
#nullable restore
#line (9,2)-(11,1) "x:\dir\subdir\Test\TestComponent.cshtml"
foreach (var item in Items2)
{

#line default
#line hidden
#nullable disable

            __builder.OpenElement(3, "p");
            __builder.AddContent(4, 
#nullable restore
#line (11,9)-(11,27) "x:\dir\subdir\Test\TestComponent.cshtml"
ChildContent(item)

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
#nullable restore
#line (12,1)-(13,1) "x:\dir\subdir\Test\TestComponent.cshtml"
}

#line default
#line hidden
#nullable disable

        }
        #pragma warning restore 1998
#nullable restore
#line (14,8)-(18,1) "x:\dir\subdir\Test\TestComponent.cshtml"

    [Parameter] public (TItem1, TItem2) Item1 { get; set; }
    [Parameter] public List<(TItem1, TItem2)> Items2 { get; set; }
    [Parameter] public RenderFragment<(TItem1, TItem2)> ChildContent { get; set; }

#line default
#line hidden
#nullable disable

    }
}
#pragma warning restore 1591
