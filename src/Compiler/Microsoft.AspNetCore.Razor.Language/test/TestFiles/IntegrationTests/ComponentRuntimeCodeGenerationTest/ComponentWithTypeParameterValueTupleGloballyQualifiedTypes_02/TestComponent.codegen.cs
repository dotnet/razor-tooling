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
    public partial class TestComponent<TDomain, TValue> : global::Microsoft.AspNetCore.Components.ComponentBase
    where TDomain : struct
    where TValue : struct
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.OpenComponent<global::Test.TestComponent<decimal, decimal>>(0);
            __builder.AddAttribute(1, "Data", global::Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<global::System.Collections.Generic.List<(decimalDomain, decimalValue)>>(
#nullable restore
#line 4 "x:\dir\subdir\Test\TestComponent.cshtml"
                     null

#line default
#line hidden
#nullable disable
            ));
            __builder.CloseComponent();
        }
        #pragma warning restore 1998
#nullable restore
#line 6 "x:\dir\subdir\Test\TestComponent.cshtml"
       
    [Parameter]
    public List<(TDomain Domain, TValue Value)> Data { get; set; }

#line default
#line hidden
#nullable disable
    }
}
#pragma warning restore 1591
