﻿// <auto-generated/>
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
            global::__Blazor.Test.TestComponent.TypeInference.CreateMyComponent_0(__builder, 0, 1, 
#nullable restore
#line 1 "x:\dir\subdir\Test\TestComponent.cshtml"
                     "hi"

#line default
#line hidden
#nullable disable
            , 2, (context) => (__builder2) => {
#nullable restore
#line (2,21)-(2,38) 25 "x:\dir\subdir\Test\TestComponent.cshtml"
__builder2.AddContent(3, context.ToLower());

#line default
#line hidden
#nullable disable
            }
            , 4, (context) => (__builder2) => {
#nullable restore
#line (3,17)-(3,24) 25 "x:\dir\subdir\Test\TestComponent.cshtml"
__builder2.AddContent(5, context);

#line default
#line hidden
#nullable disable
            }
            );
        }
        #pragma warning restore 1998
    }
}
namespace __Blazor.Test.TestComponent
{
    #line hidden
    internal static class TypeInference
    {
        public static void CreateMyComponent_0<TItem>(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder, int seq, int __seq0, TItem __arg0, int __seq1, global::Microsoft.AspNetCore.Components.RenderFragment<TItem> __arg1, int __seq2, global::Microsoft.AspNetCore.Components.RenderFragment<global::System.Int32> __arg2)
        {
        __builder.OpenComponent<global::Test.MyComponent<TItem>>(seq);
        __builder.AddAttribute(__seq0, "Item", (object)__arg0);
        __builder.AddAttribute(__seq1, "GenericFragment", (object)__arg1);
        __builder.AddAttribute(__seq2, "IntFragment", (object)__arg2);
        __builder.CloseComponent();
        }
    }
}
#pragma warning restore 1591
