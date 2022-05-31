using System;
using LightSourceGenerator.GeneratePrinter.CSharpSyntaxAttributes;

namespace LightSourceGenerator.TestDemo.Printers
{
    [PrinterClass]
    internal class GoodPrinter : IPrinter
    {
        public void Print()
        {
            Console.WriteLine("Good!");
        }
    }
}
