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
#nullable restore
#line 1 "x:\dir\subdir\Test\TestComponent.cshtml"
using AnotherTest;

#line default
#line hidden
#nullable disable
    public partial class TestComponent : global::Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.OpenComponent<global::Test.HeaderComponent>(0);
            __builder.AddAttribute(1, "Header", (global::Microsoft.AspNetCore.Components.RenderFragment)((__builder2) => {
                __builder2.AddContent(2, "Hi!");
            }
            ));
            __builder.CloseComponent();
            __builder.AddMarkupContent(3, "\r\n");
            __builder.OpenComponent<global::AnotherTest.FooterComponent>(4);
            __builder.AddAttribute(5, "Footer", (global::Microsoft.AspNetCore.Components.RenderFragment<System.DateTime>)((context) => (__builder2) => {
#nullable restore
#line (7,14)-(7,21) 25 "x:\dir\subdir\Test\TestComponent.cshtml"
__builder2.AddContent(6, context);

#line default
#line hidden
#nullable disable
            }
            ));
            __builder.CloseComponent();
            __builder.AddMarkupContent(7, "\r\n");
            __builder.OpenComponent<global::Test.HeaderComponent>(8);
            __builder.AddAttribute(9, "Header", (global::Microsoft.AspNetCore.Components.RenderFragment)((__builder2) => {
                __builder2.AddContent(10, "Hi!");
            }
            ));
            __builder.CloseComponent();
            __builder.AddMarkupContent(11, "\r\n");
            __builder.OpenComponent<global::AnotherTest.FooterComponent>(12);
            __builder.AddAttribute(13, "Footer", (global::Microsoft.AspNetCore.Components.RenderFragment<System.DateTime>)((context) => (__builder2) => {
#nullable restore
#line (13,14)-(13,21) 26 "x:\dir\subdir\Test\TestComponent.cshtml"
__builder2.AddContent(14, context);

#line default
#line hidden
#nullable disable
            }
            ));
            __builder.CloseComponent();
        }
        #pragma warning restore 1998
    }
}
#pragma warning restore 1591
