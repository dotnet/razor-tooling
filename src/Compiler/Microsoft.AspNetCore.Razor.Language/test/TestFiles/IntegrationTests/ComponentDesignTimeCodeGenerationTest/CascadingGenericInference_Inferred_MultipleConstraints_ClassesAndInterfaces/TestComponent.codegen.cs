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
#nullable restore
#line 1 "x:\dir\subdir\Test\TestComponent.cshtml"
using Models;

#line default
#line hidden
#nullable disable
    public partial class TestComponent : global::Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 219
        private void __RazorDirectiveTokenHelpers__() {
        }
        #pragma warning restore 219
        #pragma warning disable 0414
        private static System.Object __o = null;
        #pragma warning restore 0414
        #pragma warning disable 1998
        protected override void BuildRenderTree(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __o = typeof(
#nullable restore
#line 3 "x:\dir\subdir\Test\TestComponent.cshtml"
             WeatherForecast

#line default
#line hidden
#nullable disable
            );
            __o = 
#nullable restore
#line 3 "x:\dir\subdir\Test\TestComponent.cshtml"
                                       Array.Empty<WeatherForecast>()

#line default
#line hidden
#nullable disable
            ;
            __builder.AddAttribute(-1, "ColumnsTemplate", (global::Microsoft.AspNetCore.Components.RenderFragment)((__builder2) => {
                global::__Blazor.Test.TestComponent.TypeInference.CreateColumn_0(__builder2, -1, default(WeatherForecast), -1, "", -1, "", -1, "", -1, "");
#nullable restore
#line 5 "x:\dir\subdir\Test\TestComponent.cshtml"
__o = typeof(global::Test.Column<>);

#line default
#line hidden
#nullable disable
                __o = nameof(global::Test.Column<string>.
#nullable restore
#line 5 "x:\dir\subdir\Test\TestComponent.cshtml"
                             FieldName

#line default
#line hidden
#nullable disable
                );
            }
            ));
#nullable restore
#line 3 "x:\dir\subdir\Test\TestComponent.cshtml"
__o = typeof(global::Test.Grid<>);

#line default
#line hidden
#nullable disable
        }
        #pragma warning restore 1998
    }
}
namespace __Blazor.Test.TestComponent
{
    #line hidden
    internal static class TypeInference
    {
        public static void CreateColumn_0<TItem>(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder, int seq, TItem __syntheticArg0, int __seq0, global::System.Object __arg0, int __seq1, global::System.String __arg1, int __seq2, global::System.Object __arg2, int __seq3, global::System.Object __arg3)
            where TItem : global::Models.WeatherForecast, new()
        {
        __builder.OpenComponent<global::Test.Column<TItem>>(seq);
        __builder.AddAttribute(__seq0, "Title", __arg0);
        __builder.AddAttribute(__seq1, "FieldName", __arg1);
        __builder.AddAttribute(__seq2, "Format", __arg2);
        __builder.AddAttribute(__seq3, "Width", __arg3);
        __builder.CloseComponent();
        }
    }
}
#pragma warning restore 1591
