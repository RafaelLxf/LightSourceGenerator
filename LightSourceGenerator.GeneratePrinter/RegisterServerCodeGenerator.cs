using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static LightSourceGenerator.GeneratePrinter.CSharpSyntax.ExpressionStatementSyntaxHelper;

#nullable enable

namespace LightSourceGenerator.GeneratePrinter
{
    /// <summary>
    /// 注册服务代码生成器。
    /// </summary>
    public class RegisterServerCodeGenerator
    {
        public IEnumerable<SyntaxTree> SyntaxTrees { get; }

        public Compilation Compilation { get; }

        public RegisterServerCodeGenerator(Compilation compilation)
        {
            Compilation = compilation;
            SyntaxTrees = compilation.SyntaxTrees;
        }

        public string? RegisterServers(SyntaxTree syntaxTree, IEnumerable<INamedTypeSymbol> typeArgumentSymbols)
        {
            if (!SyntaxTrees.Contains(syntaxTree))
            {
                return null;
            }

            var semanticModel = Compilation.GetSemanticModel(syntaxTree);
            var root = syntaxTree.GetCompilationUnitRoot();
            var methodDeclarationSyntax = root.DescendantNodes().OfType<MethodDeclarationSyntax>().First();
            var methodSymbol = semanticModel.GetDeclaredSymbol(methodDeclarationSyntax);

            if (IsRegisterServersMethod(methodSymbol))
            {
                var typeArgumentFullNames = GetTypeArgumentFullNames(typeArgumentSymbols);
                var newMethod = AddExpressionStatements(methodDeclarationSyntax, typeArgumentFullNames);
                var newRoot = root.ReplaceNode(methodDeclarationSyntax, newMethod);

                return newRoot.SyntaxTree.ToString();
            }

            return null;
        }

        /// <summary>
        /// 获取类型参数名称列表。
        /// </summary>
        /// <param name="typeArgumentSymbols">类型参数符号列表。</param>
        /// <returns>返回类型参数名称列表</returns>
        private List<string> GetTypeArgumentFullNames(IEnumerable<INamedTypeSymbol> typeArgumentSymbols)
        {
            var typeArgumentFullNames = new List<string>();

            foreach (var typeArgumentSymbol in typeArgumentSymbols)
            {
                var typeName = typeArgumentSymbol.ToString();

                if (typeName is not null)
                {
                    typeArgumentFullNames.Add(typeName);
                }
            }

            return typeArgumentFullNames;
        }

        public bool IsRegisterServersMethod(IMethodSymbol? methodSymbol)
        {
            var isRegisterServer = methodSymbol?.Name.Equals("RegisterServers") ?? false;
            var parametersIsEmpty = methodSymbol?.Parameters.IsEmpty ?? false;

            return isRegisterServer && parametersIsEmpty;
        }

        /// <summary>
        /// 添加表达式。
        /// </summary>
        /// <param name="method">需要添加语句的方法。</param>
        /// <param name="typeArgumentFullNames">类型参数名称列表。</param>
        /// <returns>返回添加表达式后的新方法。</returns>
        private MethodDeclarationSyntax AddExpressionStatements(MethodDeclarationSyntax method, List<string> typeArgumentFullNames)
        {
            var body = method.Body;

            if (body is null)
            {
                return method;
            }

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
