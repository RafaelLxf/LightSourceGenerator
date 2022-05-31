using LightSourceGenerator.GeneratePrinter.CSharpSyntax;
using LightSourceGenerator.GeneratePrinter.CSharpSyntaxAttributes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Generic;
using System.Linq;
using static LightSourceGenerator.GeneratePrinter.Logs.FileLog;

namespace LightSourceGenerator.GeneratePrinter
{
    [Generator]
    public class SourceGenerator : ISourceGenerator
    {
        private string _newSource = @"using LightSourceGenerator.GeneratePrinter.CSharpSyntaxAttributes;

#nullable enable

namespace LightSourceGenerator.TestDemo.Printers
{
    [GenerateClass]
    internal partial class PrinterProvider
    {
        /// <summary>
        /// 由框架自动重写。
        /// </summary>
        partial void RegisterServers()
        {
        }
    }
}
";
        public void Initialize(GeneratorInitializationContext context)
        {
        }

        public void Execute(GeneratorExecutionContext context)
        {
            var newSource = GetRegisterServerSource(context.Compilation);
            context.AddSource("PrinterProvider.g.cs", newSource);
        }

        /// <summary>
        /// 获取注册服务代码。
        /// </summary>
        /// <param name="compilation"></param>
        /// <returns></returns>
        private string GetRegisterServerSource(Compilation compilation)
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(_newSource);
            var newCompilation = compilation.AddSyntaxTrees(syntaxTree);
            var codeGenerator = new RegisterServerCodeGenerator(newCompilation);
            var symbols = GetNamedTypeSymbols(newCompilation);

            var newString = codeGenerator.RegisterServers(syntaxTree, symbols);

            LogInfo("newClass: " + newString);

            return newString;
        }

        /// <summary>
        /// 获取被标记的类型。
        /// </summary>
        /// <param name="compilation"></param>
        /// <returns></returns>
        private IEnumerable<INamedTypeSymbol> GetNamedTypeSymbols(Compilation compilation)
        {
            var syntaxTrees = compilation.SyntaxTrees;
            var classDeclarationVisitor = new MarkClassDeclarationVisitor(compilation);
            var symbols = new List<INamedTypeSymbol>();

            foreach (var syntaxTree in syntaxTrees)
            {
                var classSymbols = classDeclarationVisitor.GetMarkClassSymbols<PrinterClassAttribute>(syntaxTree);

                if (classSymbols.Any())
                {
                    symbols.AddRange(classSymbols.ToList());
                }
            }

            return symbols;
        }
    }
}
