using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW12.Extension
{
    public static class ConsoleHelper
    {
        public static void PrintResult(bool success, string message)
        {
            Console.ForegroundColor = success ? ConsoleColor.Green : ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
            Console.ReadKey();
        }

        public static void PrintColorful(string message, ConsoleColor? foregroundColor = null, ConsoleColor? backgroundColor = null)
        {
            if (foregroundColor.HasValue)
                Console.ForegroundColor = foregroundColor.Value;

            if (backgroundColor.HasValue)
                Console.BackgroundColor = backgroundColor.Value;

            Console.Write(message);
            Console.ResetColor();
        }
    }
}
