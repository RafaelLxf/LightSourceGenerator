using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LightSourceGenerator.GeneratePrinter.CSharpSyntaxAttributes;

namespace LightSourceGenerator.TestDemo.Printers
{
    [PrinterClass]
    internal class GooglePrinter : IPrinter
    {
        public void Print()
        {
            Console.WriteLine("Google Printer.");
        }
    }
}
