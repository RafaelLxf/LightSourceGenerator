using System.IO;

namespace LightSourceGenerator.GeneratePrinter.Logs
{
    internal class FileLog
    {
        public static void LogInfo(string message)
        {
            string path = @"I:\tmp\LightSourceGenerator\LightSourceGenerator.TestDemo\temp\log.txt";
            string format = "Message: \n" + message + "\n\n";
            File.AppendAllText(path, format);
        }
    }
}
