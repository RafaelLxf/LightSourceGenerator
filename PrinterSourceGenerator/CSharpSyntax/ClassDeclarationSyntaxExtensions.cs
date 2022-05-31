using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PrinterSourceGenerator.CSharpSyntax
{
    internal static class ClassDeclarationSyntaxExtensions
    {
        public static IEnumerable<MethodDeclarationSyntax> GetMethodDeclarationSyntax(this ClassDeclarationSyntax classDeclaration,
            string methodName)
        {
            var declarations = classDeclaration.Members.OfType<MethodDeclarationSyntax>();

            return declarations.Where(method => method.Identifier.ValueText.Equals(methodName));
        }

        public static IEnumerable<ISymbol> GetMethodSymbols(this Compilation compilation, 
            SyntaxTree syntaxTree, 
            Func<IMethodSymbol, bool> predicate)
        {
            if (!compilation.SyntaxTrees.Contains(syntaxTree))
            {
                return new List<ISymbol>(0);
            }

            var semanticModel = compilation.GetSemanticModel(syntaxTree);
            var root = syntaxTree.GetCompilationUnitRoot();
            var classDeclarations = root.DescendantNodes().OfType<ClassDeclarationSyntax>();
            foreach (var classDeclarationSyntax in classDeclarations)
            {
                var methodDeclarations = classDeclarationSyntax.DescendantNodes().OfType<MethodDeclarationSyntax>();

                foreach (var methodDeclaration in methodDeclarations)
                {
                    var methodSymbol = semanticModel.GetDeclaredSymbol(methodDeclaration);

                    if (predicate(methodSymbol))
                    {

                    }
                }
            }

            return new List<ISymbol>(0);
        }
    }
}
