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
    #line default
    #line hidden
    #nullable restore
    public partial class TestComponent : global::Microsoft.AspNetCore.Components.ComponentBase
    #nullable disable
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.OpenComponent<global::Test.MyComponent>(0);
            __builder.AddComponentParameter(1, nameof(global::Test.MyComponent.MessageChanged), global::Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<global::Microsoft.AspNetCore.Components.EventCallback<global::System.String>>(global::Microsoft.AspNetCore.Components.EventCallback.Factory.Create<global::System.String>(this, 
#nullable restore
#line (1,32)-(1,41) "x:\dir\subdir\Test\TestComponent.cshtml"
(s) => {}

#line default
#line hidden
#nullable disable
            )));
            __builder.AddComponentParameter(2, nameof(global::Test.MyComponent.Message), global::Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<global::System.String>(
#nullable restore
#line (1,60)-(1,67) "x:\dir\subdir\Test\TestComponent.cshtml"
message

#line default
#line hidden
#nullable disable
            ));
            __builder.AddComponentParameter(3, nameof(global::Test.MyComponent.MessageChanged), global::Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<global::Microsoft.AspNetCore.Components.EventCallback<global::System.String>>(global::Microsoft.AspNetCore.Components.EventCallback.Factory.Create<global::System.String>(this, global::Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.CreateInferredEventCallback(this, __value => message = __value, message))));
            __builder.AddComponentParameter(4, nameof(global::Test.MyComponent.MessageExpression), global::Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<global::System.Linq.Expressions.Expression<System.Action<System.String>>>(() => message));
            __builder.CloseComponent();
        }
        #pragma warning restore 1998
#nullable restore
#line (2,13)-(4,1) "x:\dir\subdir\Test\TestComponent.cshtml"

    string message = "hi";

#line default
#line hidden
#nullable disable

    }
}
#pragma warning restore 1591
