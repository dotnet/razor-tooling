// <auto-generated/>
#pragma warning disable 1591
namespace Test
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
    public partial class TestComponent : global::Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
#nullable restore
#line 1 "x:\dir\subdir\Test\TestComponent.cshtml"
   RenderFragment<Person> template = (person) => 

#line default
#line hidden
#nullable disable
            (__builder2) => {
                __builder2.OpenElement(0, "div");
#nullable restore
#line (1,57)-(1,68) 25 "x:\dir\subdir\Test\TestComponent.cshtml"
__builder2.AddContent(1, person.Name);

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
            __builder.OpenComponent<global::Test.MyComponent>(2);
            __builder.AddAttribute(3, "PersonTemplate", (global::Microsoft.AspNetCore.Components.RenderFragment<Test.Person>)(
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
