using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace LightSourceGenerator.GeneratePrinter.CSharpSyntax
{
    /// <summary>
    /// 表达式声明帮助类。
    /// </summary>
    public class ExpressionStatementSyntaxHelper
    {
        /// <summary>
        /// 创建一个没有参数的泛型方法调用语句。
        /// </summary>
        /// <param name="methodName">调用方法名称。</param>
        /// <param name="typeArgumentFullName">类型参数名称。</param>
        /// <param name="leadTrivia">语句缩进。</param>
        /// <returns>返回泛型调用节点。</returns>
        public static ExpressionStatementSyntax CreateGenericMethodInvokeSyntaxNode(string methodName, string typeArgumentFullName, SyntaxTrivia leadTrivia)
        {
            var testClassId = SyntaxFactory.IdentifierName(typeArgumentFullName);
            TypeArgumentListSyntax typeArgumentList = SyntaxFactory.TypeArgumentList();
            typeArgumentList = typeArgumentList.AddArguments(testClassId);

            SyntaxToken addServerToken;

            addServerToken = SyntaxFactory.Identifier(SyntaxFactory.TriviaList(new[] { leadTrivia }), methodName,
                SyntaxFactory.TriviaList());


            GenericNameSyntax genericName = SyntaxFactory.GenericName(addServerToken, typeArgumentList);
            var endOfLine = SyntaxFactory.SyntaxTrivia(SyntaxKind.EndOfLineTrivia, "\n");
            var endOfLineToken = SyntaxFactory.Token(SyntaxFactory.TriviaList(),
                SyntaxKind.SemicolonToken,
                SyntaxFactory.TriviaList(new[] { endOfLine }));
            ExpressionSyntax invocationExpression = SyntaxFactory.InvocationExpression(genericName, SyntaxFactory.ArgumentList());
            ExpressionStatementSyntax expressionStatementSyntax = SyntaxFactory.ExpressionStatement(invocationExpression, endOfLineToken);

            //Console.WriteLine(expressionStatementSyntax.ToString());

            return expressionStatementSyntax;
        }
    }
}
