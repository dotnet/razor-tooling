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
    #nullable restore
    public partial class TestComponent : global::Microsoft.AspNetCore.Components.ComponentBase
    #nullable disable
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
#nullable restore
#line (1,3)-(2,44) "x:\dir\subdir\Test\TestComponent.cshtml"

    RenderFragment<Person> p = (person) => 

#line default
#line hidden
#nullable disable

            (__builder2) => {
                __builder2.OpenElement(0, "div");
                __builder2.OpenComponent<global::Test.MyComponent>(1);
                __builder2.AddComponentParameter(2, nameof(global::Test.MyComponent.Name), global::Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<global::System.String>(
#nullable restore
#line (2,70)-(2,81) "x:\dir\subdir\Test\TestComponent.cshtml"
person.Name

#line default
#line hidden
#nullable disable
                ));
                __builder2.CloseComponent();
                __builder2.CloseElement();
            }
#nullable restore
#line (2,90)-(3,1) "x:\dir\subdir\Test\TestComponent.cshtml"
;

#line default
#line hidden
#nullable disable

        }
        #pragma warning restore 1998
#nullable restore
#line (4,8)-(9,1) "x:\dir\subdir\Test\TestComponent.cshtml"

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
