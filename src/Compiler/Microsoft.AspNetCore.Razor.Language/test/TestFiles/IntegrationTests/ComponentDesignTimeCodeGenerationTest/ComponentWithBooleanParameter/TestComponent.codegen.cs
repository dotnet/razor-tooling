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
   __o = TestBool;

#line default
#line hidden
#nullable disable
            __o = global::Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<global::System.Boolean>(
#nullable restore
#line 3 "x:\dir\subdir\Test\TestComponent.cshtml"
                         true

#line default
#line hidden
#nullable disable
            );
            __builder.AddAttribute(-1, "ChildContent", (global::Microsoft.AspNetCore.Components.RenderFragment)((__builder2) => {
            }
            ));
            #pragma warning disable BL0005
            ((global::Test.TestComponent)default).
#nullable restore
#line 3 "x:\dir\subdir\Test\TestComponent.cshtml"
               TestBool

#line default
#line hidden
#nullable disable
             = default;
            #pragma warning restore BL0005
#nullable restore
#line 3 "x:\dir\subdir\Test\TestComponent.cshtml"
__o = typeof(global::Test.TestComponent);

#line default
#line hidden
#nullable disable
        }
        #pragma warning restore 1998
#nullable restore
#line 5 "x:\dir\subdir\Test\TestComponent.cshtml"
       
    [Parameter]
    public bool TestBool { get; set; }

#line default
#line hidden
#nullable disable
    }
}
#pragma warning restore 1591
