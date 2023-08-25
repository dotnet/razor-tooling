﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Xunit;

namespace Microsoft.AspNetCore.Razor.Language.IntegrationTests;

public class ComponentRenderModeAttributeIntegrationTests : RazorIntegrationTestBase
{
    internal override string FileKind => FileKinds.Component;

    internal string ComponentName = "TestComponent";

    internal override string DefaultFileName => ComponentName + ".cshtml";

    internal override bool UseTwoPhaseCompilation => true;

    [Fact]
    public void RenderMode_Attribute_With_Diagnostics()
    {
        var generated = CompileToCSharp($$"""
                <{{ComponentName}} @rendermode="@Microsoft.AspNetCore.Components.Web.RenderMode.Server)" />
                """, throwOnFailure: true);

        // Assert

        //x:\dir\subdir\Test\TestComponent.cshtml(1, 29): Error RZ9986: Component attributes do not support complex content(mixed C# and markup). Attribute: '@rendermode', text: 'Microsoft.AspNetCore.Components.Web.RenderMode.Server)'
        var diagnostic = Assert.Single(generated.Diagnostics);
        Assert.Equal("RZ9986", diagnostic.Id);
    }

    [Fact]
    public void RenderMode_Attribute_On_Html_Element()
    {
        var generated = CompileToCSharp("""
                <input @rendermode="Microsoft.AspNetCore.Components.Web.RenderMode.Server" />
                """, throwOnFailure: false);

        // Assert
        //x:\dir\subdir\Test\TestComponent.cshtml(1,21): Error RZ10021: Attribute 'rendermode' is only valid when used on a component.
        var diag = Assert.Single(generated.Diagnostics);
        Assert.Equal("RZ10021", diag.Id);
    }

    [Fact]
    public void RenderMode_Attribute_On_Component_With_Directive()
    {
        var generated = CompileToCSharp($$"""
                @rendermode Microsoft.AspNetCore.Components.DefaultRenderModes.Server

                <{{ComponentName}} @rendermode="Microsoft.AspNetCore.Components.DefaultRenderModes.Server" />
                """, throwOnFailure: false);

        var diagnostic = Assert.Single(generated.Diagnostics);
        Assert.Equal("RZ10022", diagnostic.Id);
    }
}

