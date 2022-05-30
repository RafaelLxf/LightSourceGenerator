using System;
using PrinterSourceGenerator.Attributes;

namespace PrinterSourceGenerator.Printers
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
