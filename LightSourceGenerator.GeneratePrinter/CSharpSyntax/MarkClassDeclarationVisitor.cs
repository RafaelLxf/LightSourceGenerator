using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static LightSourceGenerator.GeneratePrinter.Logs.FileLog;

#nullable enable

namespace LightSourceGenerator.GeneratePrinter.CSharpSyntax
{
    public class MarkClassDeclarationVisitor
    {
        public Compilation Compilation { get; }

        public IEnumerable<SyntaxTree> SyntaxTrees { get; }

        public MarkClassDeclarationVisitor(Compilation compilation)
        {
            Compilation = compilation;
            SyntaxTrees = Compilation.SyntaxTrees;
        }

        public IEnumerable<INamedTypeSymbol> GetMarkClassSymbols(SyntaxTree syntaxTree, string attributeFullName)
        {
            if (!SyntaxTrees.Contains(syntaxTree))
            {
                return new List<INamedTypeSymbol>(0);
            }

            var root = syntaxTree.GetCompilationUnitRoot();
            var classDeclarations = root.DescendantNodes().OfType<ClassDeclarationSyntax>();
            var semanticModel = Compilation.GetSemanticModel(syntaxTree);
            var symbolList = new List<INamedTypeSymbol>();

            foreach (var classDeclaration in classDeclarations)
            {
                var declaredSymbol = semanticModel.GetDeclaredSymbol(classDeclaration);

                if (IsMarkClassSymbol(declaredSymbol, attributeFullName))
                {
                    symbolList.Add(declaredSymbol!);
                }
            }

            //LogInfo("symbolList:" + symbolList.Count);

            return symbolList;
        }

        public IEnumerable<ClassDeclarationSyntax> GetMarkClassSyntaxNodes(SyntaxTree syntaxTree, string attributeFullName)
        {
            if (!SyntaxTrees.Contains(syntaxTree))
            {
                return new List<ClassDeclarationSyntax>(0);
            }

            var root = syntaxTree.GetCompilationUnitRoot();
            var classDeclarations = root.DescendantNodes().OfType<ClassDeclarationSyntax>();
            var semanticModel = Compilation.GetSemanticModel(syntaxTree);
            var nodeList = new List<ClassDeclarationSyntax>();

            foreach (var classDeclaration in classDeclarations)
            {
                var declaredSymbol = semanticModel.GetDeclaredSymbol(classDeclaration);

                if (IsMarkClassSymbol(declaredSymbol, attributeFullName))
                {
                    nodeList.Add(classDeclaration);
                }
            }

            return nodeList;
        }

        public IEnumerable<INamedTypeSymbol> GetMarkClassSymbols<TAttribute>(SyntaxTree syntaxTree) where TAttribute : Attribute
        {
            var fullName = typeof(TAttribute).FullName;

            //LogInfo("ClassName:" + fullName);

            if (fullName is null || fullName.Equals(typeof(Attribute).FullName))
            {
                return new List<INamedTypeSymbol>(0);
            }
           
            return GetMarkClassSymbols(syntaxTree, fullName);
        }

        public IEnumerable<ClassDeclarationSyntax> GetMarkClassSyntaxNodes<TAttribute>(SyntaxTree syntaxTree) where TAttribute : Attribute
        {
            var fullName = typeof(TAttribute).FullName;
            
            if (fullName is null || fullName.Equals(typeof(Attribute).FullName))
            {
                return new List<ClassDeclarationSyntax>(0);
            }

            return GetMarkClassSyntaxNodes(syntaxTree, fullName);
        }

        /// <summary>
        /// 判断一个类型声明是否是标记了特性。
        /// </summary>
        /// <param name="declaredSymbol"></param>
        /// <param name="attributeFullName">属性名称。</param>
        /// <returns>如果类型被标记返回 true，否则返回 false。</returns>
        private bool IsMarkClassSymbol(INamedTypeSymbol? declaredSymbol, string attributeFullName)
        {
            if (declaredSymbol is null)
            {
                return false;
            }

            var attributes = declaredSymbol.GetAttributes();

            return attributes.Any(item => item.ToString()?.Equals(attributeFullName) ?? false);
        }
    }
}
