using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static LightSourceGenerator.Common.CSharpSyntax.CSharpSyntaxHelper;

#nullable enable

namespace PrinterSourceGenerator.CSharpSyntax
{
    internal class PrinterRegisterServersRewriter
    {
        public SemanticModel SemanticModel { get; }

        public PrinterRegisterServersRewriter(SemanticModel semanticModel)
        {
            SemanticModel = semanticModel;
        }

        public void RegisterServers(IEnumerable<ClassDeclarationSyntax> typeArguments, SyntaxTree tree, SemanticModel model)
        {
            var typeArgumentList = new List<string>();

            foreach (var typeArgument in typeArguments)
            {
                var symbolInfo = model.GetDeclaredSymbol(typeArgument);
                var classFullName = symbolInfo.ToString();

                if (classFullName != null)
                {
                    typeArgumentList.Add(classFullName);
                }
            }

            var root = tree.GetCompilationUnitRoot();
            var methodDeclarationSyntax = root.DescendantNodes().OfType<MethodDeclarationSyntax>().First();
            var newMethod = RegisterServers(methodDeclarationSyntax, typeArgumentList);
            var newRoot = root.ReplaceNode(methodDeclarationSyntax, newMethod);
            Console.WriteLine(newRoot.ToString());
        }

        private MethodDeclarationSyntax RegisterServers(MethodDeclarationSyntax method, List<string> typeArgumentFullNames)
        {
            Console.WriteLine(method.ToString());

            var declaredSymbol = SemanticModel.GetDeclaredSymbol(method);
            var isRegisterServer = declaredSymbol?.Name.Equals("RegisterServers") ?? false;
            var parametersIsEmpty = declaredSymbol?.Parameters.IsEmpty ?? false;
            var isOverride = declaredSymbol?.IsOverride ?? false;

            if (isRegisterServer && parametersIsEmpty)
            {
                return AddServer(method, typeArgumentFullNames);
            }

            return method;
        }

        private MethodDeclarationSyntax AddServer(MethodDeclarationSyntax method, List<string> typeArgumentFullNames)
        {
            var body = method.Body;

            if (body is null)
            {
                return method;
            }

            //// 清空所有语句。
            //var bodyStatements = body.Statements;
            //while (bodyStatements.Any())
            //{
            //    bodyStatements.RemoveAt(0);
            //}

            // 重新添加语句。
            foreach (var typeArgumentFullName in typeArgumentFullNames)
            {
                var expression = CreateGenericMethodInvokeSyntaxNode("AddServer", typeArgumentFullName, DefaultWhitespaceTrivia);
                body = body.AddStatements(expression);
            }

            method = method.WithBody(body);
            //Console.WriteLine(method.ToString());

            return method;
        }

        private SyntaxTrivia DefaultWhitespaceTrivia => SyntaxFactory.SyntaxTrivia(SyntaxKind.WhitespaceTrivia, "            ");
    }
}
