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
    public partial class TestComponent : global::Microsoft.AspNetCore.Components.ComponentBase
    #nullable disable
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
#nullable restore
#line 1 "x:\dir\subdir\Test\TestComponent.cshtml"
   RenderFragment<Test.Context> template = (context) => 

#line default
#line hidden
#nullable disable
            (__builder2) => {
                __builder2.OpenElement(0, "li");
                __builder2.AddContent(1, "#");
#nullable restore
#line (1,64)-(1,77) 25 "x:\dir\subdir\Test\TestComponent.cshtml"
__builder2.AddContent(2, context.Index);

#line default
#line hidden
#nullable disable
                __builder2.AddContent(3, " - ");
#nullable restore
#line (1,81)-(1,103) 25 "x:\dir\subdir\Test\TestComponent.cshtml"
__builder2.AddContent(4, context.Item.ToLower());

#line default
#line hidden
#nullable disable
                __builder2.CloseElement();
            }
#nullable restore
#line 1 "x:\dir\subdir\Test\TestComponent.cshtml"
                                                                                                           ; 

#line default
#line hidden
#nullable disable
            __builder.OpenComponent<global::Test.MyComponent>(5);
            __builder.AddComponentParameter(6, "Template", (global::Microsoft.AspNetCore.Components.RenderFragment<Test.Context>)(
#nullable restore
#line 2 "x:\dir\subdir\Test\TestComponent.cshtml"
                        template

#line default
#line hidden
#nullable disable
            ));
            __builder.CloseComponent();
        }
        #pragma warning restore 1998
    }
}
#pragma warning restore 1591
