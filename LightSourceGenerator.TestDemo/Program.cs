using LightSourceGenerator.TestDemo.Printers;
using System;

namespace LightSourceGenerator.TestDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var printerProvider = new PrinterProvider();
            Console.WriteLine("ServerCount: " + printerProvider.ServerCount);
        }
    }
}
