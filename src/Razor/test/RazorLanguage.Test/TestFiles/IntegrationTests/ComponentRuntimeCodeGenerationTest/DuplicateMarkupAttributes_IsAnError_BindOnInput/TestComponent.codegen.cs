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
            builder.OpenElement(0, "div");
            builder.AddMarkupContent(1, "\r\n  ");
            builder.OpenElement(2, "input");
            builder.AddAttribute(3, "type", "text");
            builder.AddAttribute(4, "oninput", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.UIChangeEventArgs>(this, 
#nullable restore
#line 2 "x:\dir\subdir\Test\TestComponent.cshtml"
                                                           () => {}

#line default
#line hidden
#nullable disable
            ));
            builder.AddAttribute(5, "value", Microsoft.AspNetCore.Components.BindMethods.GetValue(
#nullable restore
#line 2 "x:\dir\subdir\Test\TestComponent.cshtml"
                                          text

#line default
#line hidden
#nullable disable
            ));
            builder.AddAttribute(6, "oninput", Microsoft.AspNetCore.Components.EventCallback.Factory.CreateBinder(this, __value => text = __value, text));
            builder.CloseElement();
            builder.AddMarkupContent(7, "\r\n");
            builder.CloseElement();
        }
        #pragma warning restore 1998
#nullable restore
#line 4 "x:\dir\subdir\Test\TestComponent.cshtml"
            
    private string text = "hi";

#line default
#line hidden
#nullable disable
    }
}
#pragma warning restore 1591
