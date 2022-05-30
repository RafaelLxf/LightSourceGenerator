using System;
using PrinterSourceGenerator.Attributes;

namespace PrinterSourceGenerator.Printers
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
