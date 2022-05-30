using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using CSharpExtensions = Microsoft.CodeAnalysis.CSharp.CSharpExtensions;

#nullable enable

namespace LightSourceGenerator.Common.CSharpSyntax
{
    /// <summary>
    /// 标记属性类型语法访问类型。
    /// </summary>
    public class MarkClassSyntaxVisitor
    {
        public MarkClassSyntaxVisitor(SemanticModel model)
        {
            Model = model;
        }

        /// <summary>
        /// 语义模型。
        /// </summary>
        public SemanticModel Model { get; }

        /// <summary>
        /// 获取标记类型。
        /// </summary>
        /// <param name="attributeFullName">标记的特性名称。</param>
        /// <returns>返回被标记的类型集合。</returns>
        public IEnumerable<ClassDeclarationSyntax> GetMarkClasses(string attributeFullName)
        {
            if (string.IsNullOrEmpty(attributeFullName) || string.IsNullOrWhiteSpace(attributeFullName))
            {
                throw new ArgumentException($"Argument {attributeFullName} must be a valid value.");
            }

            var root = CSharpExtensions.GetCompilationUnitRoot(Model.SyntaxTree);
            var classDeclarations = root.DescendantNodes().OfType<ClassDeclarationSyntax>();
            var declarationList = new List<ClassDeclarationSyntax>();

            foreach (var classDeclarationSyntax in classDeclarations)
            {
                if (IsMarkClass(classDeclarationSyntax, attributeFullName))
                {
                    declarationList.Add(classDeclarationSyntax);
                }
            }

            return declarationList;
        }

        /// <summary>
        /// 获取标记类型。
        /// </summary>
        /// <typeparam name="TAttributeType">标记的特性类型。</typeparam>
        /// <returns>返回被标记的类型集合。</returns>
        public IEnumerable<ClassDeclarationSyntax> GetMarkClasses<TAttributeType>() where TAttributeType : Attribute
        {
            var fullName = typeof(TAttributeType).FullName;

            if (fullName is null)
            {
                return new List<ClassDeclarationSyntax>(0);
            }

            return GetMarkClasses(fullName);
        }

        /// <summary>
        /// 判断一个类型声明是否是标记了特性。
        /// </summary>
        /// <param name="classDeclaration">类型声明语法节点。</param>
        /// <param name="attributeFullName">属性名称。</param>
        /// <returns>如果类型被标记返回 true，否则返回 false。</returns>
        private bool IsMarkClass(ClassDeclarationSyntax classDeclaration, string attributeFullName)
        {
            ISymbol? declaredSymbol = Model.GetDeclaredSymbol(classDeclaration);

            if (declaredSymbol is null)
            {
                return false;
            }

            var attributeLists = classDeclaration.AttributeLists;
            var attributes = declaredSymbol.GetAttributes();

            return attributes.Any(item => item.ToString()?.Equals(attributeFullName) ?? false);
        }
    }
}
