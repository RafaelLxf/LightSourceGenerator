using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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
    }
}
