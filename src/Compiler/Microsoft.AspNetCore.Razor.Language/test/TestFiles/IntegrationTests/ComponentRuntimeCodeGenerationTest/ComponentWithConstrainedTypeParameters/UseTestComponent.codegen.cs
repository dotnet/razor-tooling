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
#line (1,2)-(2,1) "x:\dir\subdir\Test\UseTestComponent.cshtml"
using Test

#line default
#line hidden
#nullable disable
    ;
    #nullable restore
    public partial class UseTestComponent : global::Microsoft.AspNetCore.Components.ComponentBase
    #nullable disable
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            global::__Blazor.Test.UseTestComponent.TypeInference.CreateTestComponent_0(__builder, 0, 1, 
#nullable restore
#line (2,23)-(2,28) "x:\dir\subdir\Test\UseTestComponent.cshtml"
item1

#line default
#line hidden
#nullable disable
            , 2, 
#nullable restore
#line (2,37)-(2,42) "x:\dir\subdir\Test\UseTestComponent.cshtml"
items

#line default
#line hidden
#nullable disable
            , 3, 
#nullable restore
#line (2,50)-(2,55) "x:\dir\subdir\Test\UseTestComponent.cshtml"
item1

#line default
#line hidden
#nullable disable
            , 4, (context) => (__builder2) => {
                __builder2.OpenElement(5, "p");
                __builder2.AddContent(6, 
#nullable restore
#line (3,9)-(3,16) "x:\dir\subdir\Test\UseTestComponent.cshtml"
context

#line default
#line hidden
#nullable disable
                );
                __builder2.CloseElement();
            }
            );
        }
        #pragma warning restore 1998
#nullable restore
#line (6,8)-(11,1) "x:\dir\subdir\Test\UseTestComponent.cshtml"

    Image item1 = new Image() { id = 1, url="https://example.com"};
    static Tag tag1 = new Tag() { description = "A description."};
    static Tag tag2 = new Tag() { description = "Another description."};
    List<Tag> items = new List<Tag>() { tag1, tag2 };

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
        public static void CreateTestComponent_0<TItem1, TItem2, TItem3>(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder, int seq, int __seq0, TItem1 __arg0, int __seq1, global::System.Collections.Generic.List<TItem2> __arg1, int __seq2, TItem3 __arg2, int __seq3, global::Microsoft.AspNetCore.Components.RenderFragment<TItem2> __arg3)
            where TItem1 : global::Image
            where TItem2 : global::ITag
            where TItem3 : global::Image, new()
        {
        __builder.OpenComponent<global::Test.TestComponent<TItem1, TItem2, TItem3>>(seq);
        __builder.AddComponentParameter(__seq0, nameof(global::Test.TestComponent<TItem1, TItem2, TItem3>.Item1), __arg0);
        __builder.AddComponentParameter(__seq1, nameof(global::Test.TestComponent<TItem1, TItem2, TItem3>.Items2), __arg1);
        __builder.AddComponentParameter(__seq2, nameof(global::Test.TestComponent<TItem1, TItem2, TItem3>.Item3), __arg2);
        __builder.AddComponentParameter(__seq3, "ChildContent", __arg3);
        __builder.CloseComponent();
        }
    }
}
#pragma warning restore 1591
