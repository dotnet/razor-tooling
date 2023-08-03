﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable enable

using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Razor.Language.Intermediate;

namespace Microsoft.AspNetCore.Razor.Language.Components;

internal class ComponentRenderModeDirectivePass : IntermediateNodePassBase, IRazorDirectiveClassifierPass
{
    protected override void ExecuteCore(RazorCodeDocument codeDocument, DocumentIntermediateNode documentNode)
    {
        var @namespace = documentNode.FindPrimaryNamespace();
        var @class = documentNode.FindPrimaryClass();
        if (@namespace == null || @class == null)
        {
            return;
        }

        var directives = documentNode.FindDirectiveReferences(ComponentRenderModeDirective.Directive);
        if (directives.Count == 0)
        {
            return;
        }

        // We don't need to worry about duplicate attributes as we have already replaced any multiples with MalformedDirective
        Debug.Assert(directives.Count == 1);

        var token = ((DirectiveIntermediateNode)directives[0].Node).Tokens.FirstOrDefault();
        if (token == null)
        {
            return;
        }

        // generate the inner attribute class
        // PROTOTYPE: fully qualify type names and extract them out to consts
        var classDecl = new ClassDeclarationIntermediateNode()
        {
            ClassName = "PrivateComponentRenderModeAttribute",
            BaseType = "RenderModeAttribute",
        };
        classDecl.Modifiers.Add("private");

        // PROTOTYPE: PropertyDeclarationIntermediateNode doens't support emitting anything other than autoprops right now so just use an interpolated string
        //var propertyDecl = new PropertyDeclarationIntermediateNode()
        //{
        //    PropertyName = "Mode",
        //    PropertyType = "IComponentRenderMode",
        //    Modifiers =
        //    {
        //        "public",
        //        "override"
        //    }
        //};

        var propertyDecl = new CSharpCodeIntermediateNode();
        propertyDecl.Source = token.Source; // PROTOYPE: this is the best we can do until we can emit properties properly
        propertyDecl.Children.Add(new IntermediateToken()
        {
            Kind = TokenKind.CSharp,
            Content = $"public override IComponentRenderMode Mode => {token.Content};"
        });
        
        classDecl.Children.Add(propertyDecl);
        @class.Children.Add(classDecl);

        // generate the attribute usage on top of the class
        var attributeNode = new CSharpCodeIntermediateNode();
        attributeNode.Children.Add(new IntermediateToken()
        {
            Kind = TokenKind.CSharp,
            Content = $"[{@namespace.Content}.{@class.ClassName}.PrivateComponentRenderModeAttribute]",
        });

        // Insert the new attribute on top of the class
        for (var i = 0; i < @namespace.Children.Count; i++)
        {
            if (object.ReferenceEquals(@namespace.Children[i], @class))
            {
                @namespace.Children.Insert(i, attributeNode);
                break;
            }
        }
    }
}
