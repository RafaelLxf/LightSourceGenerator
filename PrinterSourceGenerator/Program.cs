using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LightSourceGenerator.Common.CSharpSyntax;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using PrinterSourceGenerator.Attributes;
using PrinterSourceGenerator.CSharpSyntax;

namespace Printer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var reader = File.OpenText(@"I:\tmp\LightSourceGenerator\PrinterSourceGenerator\Printers\GoodPrinter.cs");
            var syntaxTree = CSharpSyntaxTree.ParseText(reader.ReadToEnd());

            var reader1 = File.OpenText(@"I:\tmp\LightSourceGenerator\PrinterSourceGenerator\Attributes\PrinterClassAttribute.cs");
            var syntaxTree1 = CSharpSyntaxTree.ParseText(reader1.ReadToEnd());

            var reader2 = File.OpenText(@"I:\tmp\LightSourceGenerator\PrinterSourceGenerator\Printers\PrinterProvider.g.cs");
            var syntaxTree2 = CSharpSyntaxTree.ParseText(reader2.ReadToEnd());

            var metadata = MetadataReference.CreateFromFile(typeof(Attribute).Assembly.Location);
            var compilation = CSharpCompilation.Create("TestApp2",
                new[] { syntaxTree, syntaxTree1, syntaxTree2 },
                new[] { metadata });

            var root = syntaxTree.GetCompilationUnitRoot();
            IEnumerable<ClassDeclarationSyntax> classDeclarations = root.DescendantNodes().OfType<ClassDeclarationSyntax>();
            var typeDec = classDeclarations.First();
            var semanticModel = compilation.GetSemanticModel(syntaxTree);

            var markClassSyntaxVisitor = new MarkClassSyntaxVisitor(semanticModel);
            var classDeclarationSyntaxes = markClassSyntaxVisitor.GetMarkClasses<PrinterClassAttribute>();
            var declarationSyntax = classDeclarationSyntaxes.First();

            var syntaxes = declarationSyntax.GetMethodDeclarationSyntax("Print").First();

            var semanticModel1 = compilation.GetSemanticModel(syntaxTree2);
            var semanticModel2 = compilation.GetSemanticModel(syntaxTree);
            var printerRegisterServersRewriter = new PrinterRegisterServersRewriter(semanticModel1);
            printerRegisterServersRewriter.RegisterServers(classDeclarationSyntaxes, syntaxTree2, semanticModel2);

            Console.WriteLine("Hello World!");
        }
    }
}
