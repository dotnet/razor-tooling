﻿// <auto-generated/>
#pragma warning disable 1591
namespace Test
{
    #line hidden
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Linq;
    using global::System.Threading.Tasks;
    using global::Microsoft.AspNetCore.Components;
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
#nullable restore
#line 1 "x:\dir\subdir\Test\TestComponent.cshtml"
  
    RenderFragment<Person> p = (person) => 

#line default
#line hidden
#nullable disable
            (__builder2) => {
                __o = global::Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<global::System.String>(
#nullable restore
#line 2 "x:\dir\subdir\Test\TestComponent.cshtml"
                                                                     person.Name

#line default
#line hidden
#nullable disable
                );
                __builder2.AddAttribute(-1, "ChildContent", (global::Microsoft.AspNetCore.Components.RenderFragment)((__builder3) => {
                }
                ));
                __o = ((global::Test.MyComponent)default).
#nullable restore
#line 2 "x:\dir\subdir\Test\TestComponent.cshtml"
                                                              Name

#line default
#line hidden
#nullable disable
                ;
#nullable restore
#line 2 "x:\dir\subdir\Test\TestComponent.cshtml"
__o = typeof(global::Test.MyComponent);

#line default
#line hidden
#nullable disable
            }
#nullable restore
#line 2 "x:\dir\subdir\Test\TestComponent.cshtml"
                                                                                         ;

#line default
#line hidden
#nullable disable
            __builder.AddAttribute(-1, "ChildContent", (global::Microsoft.AspNetCore.Components.RenderFragment)((__builder2) => {
#nullable restore
#line 5 "x:\dir\subdir\Test\TestComponent.cshtml"
__o = "hello, world!";

#line default
#line hidden
#nullable disable
            }
            ));
#nullable restore
#line 4 "x:\dir\subdir\Test\TestComponent.cshtml"
__o = typeof(global::Test.MyComponent);

#line default
#line hidden
#nullable disable
        }
        #pragma warning restore 1998
#nullable restore
#line 8 "x:\dir\subdir\Test\TestComponent.cshtml"
       
    class Person
    {
        public string Name { get; set; }
    }

#line default
#line hidden
#nullable disable
    }
}
#pragma warning restore 1591
