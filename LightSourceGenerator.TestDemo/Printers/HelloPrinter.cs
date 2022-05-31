using System;
using LightSourceGenerator.GeneratePrinter.CSharpSyntaxAttributes;

namespace LightSourceGenerator.TestDemo.Printers
{
    [PrinterClass]
    internal class HelloPrinter : IPrinter
    {
        public void Print()
        {
            Console.WriteLine("Hello!");
        }
    }
}
