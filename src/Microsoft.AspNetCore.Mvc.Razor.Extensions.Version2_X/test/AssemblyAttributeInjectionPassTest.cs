﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Xunit;

namespace Microsoft.AspNetCore.Mvc.Razor.Extensions.Version2_X;

public class AssemblyAttributeInjectionPassTest : RazorProjectEngineTestBase
{
    protected override RazorLanguageVersion Version => RazorLanguageVersion.Version_2_1;

    [Fact]
    public void Execute_NoOps_IfNamespaceNodeIsMissing()
    {
        // Arrange
        var irDocument = new DocumentIntermediateNode()
        {
            Options = RazorCodeGenerationOptions.CreateDefault(),
        };

        var pass = new AssemblyAttributeInjectionPass
        {
            Engine = CreateProjectEngine().Engine,
        };

        // Act
        pass.Execute(TestRazorCodeDocument.CreateEmpty(), irDocument);

        // Assert
        Assert.Empty(irDocument.Children);
    }

    [Fact]
    public void Execute_NoOps_IfNamespaceNodeHasEmptyContent()
    {
        // Arrange
        var irDocument = new DocumentIntermediateNode()
        {
            Options = RazorCodeGenerationOptions.CreateDefault(),
        };
        var builder = IntermediateNodeBuilder.Create(irDocument);
        var @namespace = new NamespaceDeclarationIntermediateNode() { Content = string.Empty };
        @namespace.Annotations[CommonAnnotations.PrimaryNamespace] = CommonAnnotations.PrimaryNamespace;
        builder.Push(@namespace);

        var pass = new AssemblyAttributeInjectionPass
        {
            Engine = CreateProjectEngine().Engine,
        };

        // Act
        pass.Execute(TestRazorCodeDocument.CreateEmpty(), irDocument);

        // Assert
        Assert.Collection(irDocument.Children,
            node => Assert.Same(@namespace, node));
    }

    [Fact]
    public void Execute_NoOps_IfClassNameNodeIsMissing()
    {
        // Arrange
        var irDocument = new DocumentIntermediateNode()
        {
            Options = RazorCodeGenerationOptions.CreateDefault(),
        };

        var builder = IntermediateNodeBuilder.Create(irDocument);
        var @namespace = new NamespaceDeclarationIntermediateNode() { Content = "SomeNamespace" };
        builder.Push(@namespace);

        var pass = new AssemblyAttributeInjectionPass
        {
            Engine = CreateProjectEngine().Engine,
        };

        // Act
        pass.Execute(TestRazorCodeDocument.CreateEmpty(), irDocument);

        // Assert
        Assert.Collection(
            irDocument.Children,
            node => Assert.Same(@namespace, node));
    }

    [Fact]
    public void Execute_NoOps_IfClassNameIsEmpty()
    {
        // Arrange
        var irDocument = new DocumentIntermediateNode()
        {
            Options = RazorCodeGenerationOptions.CreateDefault(),
        };
        var builder = IntermediateNodeBuilder.Create(irDocument);
        var @namespace = new NamespaceDeclarationIntermediateNode
        {
            Content = "SomeNamespace",
            Annotations =
                {
                    [CommonAnnotations.PrimaryNamespace] = CommonAnnotations.PrimaryNamespace,
                },
        };
        builder.Push(@namespace);
        var @class = new ClassDeclarationIntermediateNode
        {
            Annotations =
                {
                    [CommonAnnotations.PrimaryClass] = CommonAnnotations.PrimaryClass,
                },
        };
        builder.Add(@class);

        var pass = new AssemblyAttributeInjectionPass
        {
            Engine = CreateProjectEngine().Engine,
        };

        // Act
        pass.Execute(TestRazorCodeDocument.CreateEmpty(), irDocument);

        // Assert
        Assert.Collection(irDocument.Children,
            node => Assert.Same(@namespace, node));
    }

    [Fact]
    public void Execute_NoOps_IfDocumentIsNotViewOrPage()
    {
        // Arrange
        var irDocument = new DocumentIntermediateNode
        {
            DocumentKind = "Default",
            Options = RazorCodeGenerationOptions.CreateDefault(),
        };
        var builder = IntermediateNodeBuilder.Create(irDocument);
        var @namespace = new NamespaceDeclarationIntermediateNode() { Content = "SomeNamespace" };
        builder.Push(@namespace);
        var @class = new ClassDeclarationIntermediateNode
        {
            ClassName = "SomeName",
            Annotations =
                {
                    [CommonAnnotations.PrimaryClass] = CommonAnnotations.PrimaryClass,
                },
        };
        builder.Add(@class);

        var pass = new AssemblyAttributeInjectionPass
        {
            Engine = CreateProjectEngine().Engine,
        };

        // Act
        pass.Execute(TestRazorCodeDocument.CreateEmpty(), irDocument);

        // Assert
        Assert.Collection(
            irDocument.Children,
            node => Assert.Same(@namespace, node));
    }

    [Fact]
    public void Execute_NoOps_ForDesignTime()
    {
        // Arrange
        var irDocument = new DocumentIntermediateNode
        {
            DocumentKind = MvcViewDocumentClassifierPass.MvcViewDocumentKind,
            Options = RazorCodeGenerationOptions.CreateDesignTimeDefault(),
        };
        var builder = IntermediateNodeBuilder.Create(irDocument);
        var @namespace = new NamespaceDeclarationIntermediateNode
        {
            Content = "SomeNamespace",
            Annotations =
                {
                    [CommonAnnotations.PrimaryNamespace] = CommonAnnotations.PrimaryNamespace
                },
        };
        builder.Push(@namespace);
        var @class = new ClassDeclarationIntermediateNode
        {
            ClassName = "SomeName",
            Annotations =
                {
                    [CommonAnnotations.PrimaryClass] = CommonAnnotations.PrimaryClass,
                },
        };
        builder.Add(@class);

        var pass = new AssemblyAttributeInjectionPass
        {
            Engine = CreateProjectEngine().Engine,
        };

        var source = TestRazorSourceDocument.Create("test", new RazorSourceDocumentProperties(filePath: null, relativePath: "/Views/Index.cshtml"));
        var document = RazorCodeDocument.Create(source);

        // Act
        pass.Execute(document, irDocument);

        // Assert
        Assert.Collection(
            irDocument.Children,
            node => Assert.Same(@namespace, node));
    }

    [Fact]
    public void Execute_AddsRazorViewAttribute_ToViews()
    {
        // Arrange
        var expectedAttribute = "[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@\"/Views/Index.cshtml\", typeof(SomeNamespace.SomeName))]";
        var irDocument = new DocumentIntermediateNode
        {
            DocumentKind = MvcViewDocumentClassifierPass.MvcViewDocumentKind,
            Options = RazorCodeGenerationOptions.CreateDefault(),
        };
        var builder = IntermediateNodeBuilder.Create(irDocument);
        var @namespace = new NamespaceDeclarationIntermediateNode
        {
            Content = "SomeNamespace",
            Annotations =
                {
                    [CommonAnnotations.PrimaryNamespace] = CommonAnnotations.PrimaryNamespace
                },
        };
        builder.Push(@namespace);
        var @class = new ClassDeclarationIntermediateNode
        {
            ClassName = "SomeName",
            Annotations =
                {
                    [CommonAnnotations.PrimaryClass] = CommonAnnotations.PrimaryClass,
                },
        };
        builder.Add(@class);

        var pass = new AssemblyAttributeInjectionPass
        {
            Engine = CreateProjectEngine().Engine,
        };

        var source = TestRazorSourceDocument.Create("test", new RazorSourceDocumentProperties(filePath: null, relativePath: "/Views/Index.cshtml"));
        var document = RazorCodeDocument.Create(source);

        // Act
        pass.Execute(document, irDocument);

        // Assert
        Assert.Collection(irDocument.Children,
            node =>
            {
                var csharpCode = Assert.IsType<CSharpCodeIntermediateNode>(node);
                var token = Assert.IsAssignableFrom<IntermediateToken>(Assert.Single(csharpCode.Children));
                Assert.Equal(TokenKind.CSharp, token.Kind);
                Assert.Equal(expectedAttribute, token.Content);
            },
            node => Assert.Same(@namespace, node));
    }

    [Fact]
    public void Execute_EscapesViewPathWhenAddingAttributeToViews()
    {
        // Arrange
        var expectedAttribute = "[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@\"/test/\"\"Index.cshtml\", typeof(SomeNamespace.SomeName))]";
        var irDocument = new DocumentIntermediateNode
        {
            DocumentKind = MvcViewDocumentClassifierPass.MvcViewDocumentKind,
            Options = RazorCodeGenerationOptions.CreateDefault(),
        };
        var builder = IntermediateNodeBuilder.Create(irDocument);
        var @namespace = new NamespaceDeclarationIntermediateNode
        {
            Content = "SomeNamespace",
            Annotations =
                {
                    [CommonAnnotations.PrimaryNamespace] = CommonAnnotations.PrimaryNamespace
                },
        };
        builder.Push(@namespace);
        var @class = new ClassDeclarationIntermediateNode
        {
            ClassName = "SomeName",
            Annotations =
                {
                    [CommonAnnotations.PrimaryClass] = CommonAnnotations.PrimaryClass,
                },
        };
        builder.Add(@class);

        var pass = new AssemblyAttributeInjectionPass
        {
            Engine = CreateProjectEngine().Engine,
        };

        var source = TestRazorSourceDocument.Create("test", new RazorSourceDocumentProperties(filePath: null, relativePath: "\\test\\\"Index.cshtml"));
        var document = RazorCodeDocument.Create(source);

        // Act
        pass.Execute(document, irDocument);

        // Assert
        Assert.Collection(irDocument.Children,
            node =>
            {
                var csharpCode = Assert.IsType<CSharpCodeIntermediateNode>(node);
                var token = Assert.IsAssignableFrom<IntermediateToken>(Assert.Single(csharpCode.Children));
                Assert.Equal(TokenKind.CSharp, token.Kind);
                Assert.Equal(expectedAttribute, token.Content);
            },
            node => Assert.Same(@namespace, node));
    }

    [Fact]
    public void Execute_AddsRazorPagettribute_ToPage()
    {
        // Arrange
        var expectedAttribute = "[assembly:global::Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure.RazorPageAttribute(@\"/Views/Index.cshtml\", typeof(SomeNamespace.SomeName), null)]";
        var irDocument = new DocumentIntermediateNode
        {
            DocumentKind = RazorPageDocumentClassifierPass.RazorPageDocumentKind,
            Options = RazorCodeGenerationOptions.CreateDefault(),
        };
        var builder = IntermediateNodeBuilder.Create(irDocument);
        var pageDirective = new DirectiveIntermediateNode
        {
            Directive = PageDirective.Directive,
        };
        builder.Add(pageDirective);

        var @namespace = new NamespaceDeclarationIntermediateNode
        {
            Content = "SomeNamespace",
            Annotations =
                {
                    [CommonAnnotations.PrimaryNamespace] = CommonAnnotations.PrimaryNamespace
                },
        };
        builder.Push(@namespace);
        var @class = new ClassDeclarationIntermediateNode
        {
            ClassName = "SomeName",
            Annotations =
                {
                    [CommonAnnotations.PrimaryClass] = CommonAnnotations.PrimaryClass,
                },
        };
        builder.Add(@class);

        var pass = new AssemblyAttributeInjectionPass
        {
            Engine = CreateProjectEngine().Engine,
        };

        var source = TestRazorSourceDocument.Create("test", new RazorSourceDocumentProperties(filePath: null, relativePath: "/Views/Index.cshtml"));
        var document = RazorCodeDocument.Create(source);

        // Act
        pass.Execute(document, irDocument);

        // Assert
        Assert.Collection(irDocument.Children,
            node => Assert.Same(pageDirective, node),
            node =>
            {
                var csharpCode = Assert.IsType<CSharpCodeIntermediateNode>(node);
                var token = Assert.IsAssignableFrom<IntermediateToken>(Assert.Single(csharpCode.Children));
                Assert.Equal(TokenKind.CSharp, token.Kind);
                Assert.Equal(expectedAttribute, token.Content);
            },
            node => Assert.Same(@namespace, node));
    }

    [Fact]
    public void Execute_EscapesViewPathAndRouteWhenAddingAttributeToPage()
    {
        // Arrange
        var expectedAttribute = "[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@\"/test/\"\"Index.cshtml\", typeof(SomeNamespace.SomeName))]";
        var irDocument = new DocumentIntermediateNode
        {
            DocumentKind = MvcViewDocumentClassifierPass.MvcViewDocumentKind,
            Options = RazorCodeGenerationOptions.CreateDefault(),
        };
        var builder = IntermediateNodeBuilder.Create(irDocument);
        var @namespace = new NamespaceDeclarationIntermediateNode
        {
            Content = "SomeNamespace",
            Annotations =
                {
                    [CommonAnnotations.PrimaryNamespace] = CommonAnnotations.PrimaryNamespace
                },
        };
        builder.Push(@namespace);
        var @class = new ClassDeclarationIntermediateNode
        {
            ClassName = "SomeName",
            Annotations =
                {
                    [CommonAnnotations.PrimaryClass] = CommonAnnotations.PrimaryClass,
                },
        };

        builder.Add(@class);

        var pass = new AssemblyAttributeInjectionPass
        {
            Engine = CreateProjectEngine().Engine,
        };

        var source = TestRazorSourceDocument.Create("test", new RazorSourceDocumentProperties(filePath: null, relativePath: "test\\\"Index.cshtml"));
        var document = RazorCodeDocument.Create(source);

        // Act
        pass.Execute(document, irDocument);

        // Assert
        Assert.Collection(irDocument.Children,
            node =>
            {
                var csharpCode = Assert.IsType<CSharpCodeIntermediateNode>(node);
                var token = Assert.IsAssignableFrom<IntermediateToken>(Assert.Single(csharpCode.Children));
                Assert.Equal(TokenKind.CSharp, token.Kind);
                Assert.Equal(expectedAttribute, token.Content);
            },
            node => Assert.Same(@namespace, node));
    }

    private DocumentIntermediateNode CreateIRDocument(RazorEngine engine, RazorCodeDocument codeDocument)
    {
        for (var i = 0; i < engine.Phases.Count; i++)
        {
            var phase = engine.Phases[i];
            phase.Execute(codeDocument);

            if (phase is IRazorDocumentClassifierPhase)
            {
                break;
            }
        }

        return codeDocument.GetDocumentIntermediateNode();
    }
}
