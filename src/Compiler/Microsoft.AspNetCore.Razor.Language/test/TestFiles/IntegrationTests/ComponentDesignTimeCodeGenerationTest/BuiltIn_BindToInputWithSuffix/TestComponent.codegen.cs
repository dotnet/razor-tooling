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
#nullable restore
#line 1 "x:\dir\subdir\Test\TestComponent.cshtml"
using 

#line default
#line hidden
#nullable disable
#nullable restore
#line 1 "x:\dir\subdir\Test\TestComponent.cshtml"
Microsoft.AspNetCore.Components.Web;

#line default
#line hidden
#nullable disable
    #nullable restore
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
            __o = global::Microsoft.AspNetCore.Components.BindConverter.FormatValue(
#nullable restore
#line 2 "x:\dir\subdir\Test\TestComponent.cshtml"
                     CurrentDate

#line default
#line hidden
#nullable disable
            , format: "MM/dd");
            __o = global::Microsoft.AspNetCore.Components.EventCallback.Factory.CreateBinder(this, __value => CurrentDate = __value, CurrentDate, format: "MM/dd");
        }
        #pragma warning restore 1998
#nullable restore
#line 3 "x:\dir\subdir\Test\TestComponent.cshtml"
       
    public DateTime CurrentDate { get; set; } = new DateTime(2018, 1, 1);

#line default
#line hidden
#nullable disable
    }
}
#pragma warning restore 1591
