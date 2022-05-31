using System;
using System.IO;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
//using PrinterSourceGenerator.Attributes;

namespace LightSourceGenerator.GeneratePrinter
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //var reader = File.OpenText(@"I:\tmp\LightSourceGenerator\PrinterSourceGenerator\Printers\GoodPrinter.cs");
            //var syntaxTree = CSharpSyntaxTree.ParseText(reader.ReadToEnd());

            //var reader1 = File.OpenText(@"I:\tmp\LightSourceGenerator\PrinterSourceGenerator\Attributes\PrinterClassAttribute.cs");
            //var syntaxTree1 = CSharpSyntaxTree.ParseText(reader1.ReadToEnd());

            //var reader2 = File.OpenText(@"I:\tmp\LightSourceGenerator\PrinterSourceGenerator\Printers\PrinterProvider.g.cs");
            //var syntaxTree2 = CSharpSyntaxTree.ParseText(reader2.ReadToEnd());

            //var metadata = MetadataReference.CreateFromFile(typeof(Attribute).Assembly.Location);
            //var compilation = CSharpCompilation.Create("TestApp2",
            //    new[] { syntaxTree, syntaxTree1, syntaxTree2 },
            //    new[] { metadata });

            //var classDeclarationVisitor = new MarkClassDeclarationVisitor(compilation);
            //var classSymbols = classDeclarationVisitor.GetMarkClassSymbols<PrinterClassAttribute>(syntaxTree);

            //var generator = new RegisterServerCodeGenerator(compilation);
            //var newCode = generator.RegisterServers(syntaxTree2, classSymbols);
            //Console.WriteLine(newCode);
        }
    }
}
