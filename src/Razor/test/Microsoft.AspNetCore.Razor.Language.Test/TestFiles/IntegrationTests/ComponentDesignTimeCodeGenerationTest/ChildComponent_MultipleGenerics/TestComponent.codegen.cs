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
        #pragma warning disable 219
        private void __RazorDirectiveTokenHelpers__() {
        }
        #pragma warning restore 219
        #pragma warning disable 0414
        private static System.Object __o = null;
        #pragma warning restore 0414
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.RenderTree.RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
            __o = typeof(
#line 1 "x:\dir\subdir\Test\TestComponent.cshtml"
                    string

#line default
#line hidden
            );
            __o = typeof(
#line 1 "x:\dir\subdir\Test\TestComponent.cshtml"
                                  int

#line default
#line hidden
            );
            __o = Microsoft.AspNetCore.Components.RuntimeHelpers.TypeCheck<string>(
#line 1 "x:\dir\subdir\Test\TestComponent.cshtml"
                                              "hi"

#line default
#line hidden
            );
            builder.AddAttribute(-1, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment<string>)((context) => (builder2) => {
#line 2 "x:\dir\subdir\Test\TestComponent.cshtml"
                __o = context.ToLower();

#line default
#line hidden
            }
            ));
            builder.AddAttribute(-1, "AnotherChildContent", (Microsoft.AspNetCore.Components.RenderFragment<Test.MyComponent<string, int>.Context>)((item) => (builder2) => {
#line 4 "x:\dir\subdir\Test\TestComponent.cshtml"
__o = System.Math.Max(0, item.Item);

#line default
#line hidden
            }
            ));
#line 1 "x:\dir\subdir\Test\TestComponent.cshtml"
__o = typeof(MyComponent<,>);

#line default
#line hidden
        }
        #pragma warning restore 1998
    }
}
#pragma warning restore 1591
