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
#nullable restore
#line 1 "x:\dir\subdir\Test\UseTestComponent.cshtml"
using Test;

#line default
#line hidden
#nullable disable
    public partial class UseTestComponent : global::Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            global::__Blazor.Test.UseTestComponent.TypeInference.CreateTestComponent_0(__builder, 0, 1, 
#nullable restore
#line 2 "x:\dir\subdir\Test\UseTestComponent.cshtml"
                     item1

#line default
#line hidden
#nullable disable
            , 2, 
#nullable restore
#line 2 "x:\dir\subdir\Test\UseTestComponent.cshtml"
                                  items2

#line default
#line hidden
#nullable disable
            , 3, (context) => (__builder2) => {
                __builder2.OpenElement(4, "p");
#nullable restore
#line (3,9)-(3,16) 25 "x:\dir\subdir\Test\UseTestComponent.cshtml"
__builder2.AddContent(5, context);

#line default
#line hidden
#nullable disable
                __builder2.CloseElement();
            }
            );
        }
        #pragma warning restore 1998
#nullable restore
#line 6 "x:\dir\subdir\Test\UseTestComponent.cshtml"
       
    (string, int) item1 = ("A string", 42);
    static (string, int) item2 = ("Another string", 42);
    List<(string, int)> items2 = new List<(string, int)>() { item2 };

#line default
#line hidden
#nullable disable
    }
}
namespace __Blazor.Test.UseTestComponent
{
    #line hidden
    internal static class TypeInference
    {
        public static void CreateTestComponent_0<TItem1, TItem2>(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder, int seq, int __seq0, (TItem1, TItem2) __arg0, int __seq1, global::System.Collections.Generic.List<(TItem1, TItem2)> __arg1, int __seq2, global::Microsoft.AspNetCore.Components.RenderFragment<(TItem1, TItem2)> __arg2)
        {
        __builder.OpenComponent<global::Test.TestComponent<TItem1, TItem2>>(seq);
        __builder.AddAttribute(__seq0, "Item1", __arg0);
        __builder.AddAttribute(__seq1, "Items2", __arg1);
        __builder.AddAttribute(__seq2, "ChildContent", __arg2);
        __builder.CloseComponent();
        }
    }
}
#pragma warning restore 1591
