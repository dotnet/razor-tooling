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
    public class TestComponent : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.RenderTree.RenderTreeBuilder builder)
        {
            builder.OpenElement(0, "elem");
            builder.AddAttribute(1, "attributebefore", "before");
            builder.AddAttribute(2, "attributeafter", "after");
            builder.AddElementReferenceCapture(3, (__value) => {
#nullable restore
#line 1 "x:\dir\subdir\Test\TestComponent.cshtml"
                                     myElem = __value;

#line default
#line hidden
#nullable disable
            }
            );
            builder.AddContent(4, "Hello");
            builder.CloseElement();
        }
        #pragma warning restore 1998
#nullable restore
#line 3 "x:\dir\subdir\Test\TestComponent.cshtml"
       
    private Microsoft.AspNetCore.Components.ElementRef myElem;
    public void Foo() { System.GC.KeepAlive(myElem); }

#line default
#line hidden
#nullable disable
    }
}
#pragma warning restore 1591
