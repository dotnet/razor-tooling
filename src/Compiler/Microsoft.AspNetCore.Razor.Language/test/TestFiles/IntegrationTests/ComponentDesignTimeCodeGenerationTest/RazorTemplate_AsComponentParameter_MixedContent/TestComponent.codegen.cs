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
#nullable restore
#line 1 "x:\dir\subdir\Test\TestComponent.cshtml"
   RenderFragment<Test.Context> template = (context) => 

#line default
#line hidden
#nullable disable
            (__builder2) => {
#nullable restore
#line 1 "x:\dir\subdir\Test\TestComponent.cshtml"
                                                         __o = context.Index;

#line default
#line hidden
#nullable disable
#nullable restore
#line 1 "x:\dir\subdir\Test\TestComponent.cshtml"
                                                                          __o = context.Item.ToLower();

#line default
#line hidden
#nullable disable
            }
#nullable restore
#line 1 "x:\dir\subdir\Test\TestComponent.cshtml"
                                                                                                           ; 

#line default
#line hidden
#nullable disable
            __o = new global::Microsoft.AspNetCore.Components.RenderFragment<Test.Context>(
#nullable restore
#line 2 "x:\dir\subdir\Test\TestComponent.cshtml"
                        template

#line default
#line hidden
#nullable disable
            );
            __builder.AddAttribute(-1, "ChildContent", (global::Microsoft.AspNetCore.Components.RenderFragment)((__builder2) => {
            }
            ));
            #pragma warning disable BL0005
            ((global::Test.MyComponent)default).
#nullable restore
#line 2 "x:\dir\subdir\Test\TestComponent.cshtml"
             Template

#line default
#line hidden
#nullable disable
             = default;
            #pragma warning restore BL0005
#nullable restore
#line 2 "x:\dir\subdir\Test\TestComponent.cshtml"
__o = typeof(global::Test.MyComponent);

#line default
#line hidden
#nullable disable
        }
        #pragma warning restore 1998
    }
}
#pragma warning restore 1591
