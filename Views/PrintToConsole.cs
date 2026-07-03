using Diagnostish.Models;

namespace Diagnostish.View
{
    public class PrintToConsole : IPrintHW, IPrintOS
    {
        public void PrintHardware(HWReport rep)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\nPC CONFIGURATIONS:");
            Console.ResetColor();

            Console.WriteLine($"\nProcessor: {rep.ProcessorName} ({rep.CoresCount} cores), clock speed - {rep.CurrentClockSpeed} MGz");
            Console.WriteLine($"RAM: {rep.RAMSize} GB, {rep.RAMSpeed} MGz");
            Console.WriteLine("Videocards:");
            foreach (var gpu in rep.VideoCards)
            {
                Console.WriteLine($" - {gpu}");
            }
            Console.WriteLine("Drives:");
            foreach (var drives in rep.Drives)
            {
                Console.WriteLine($" - {drives}");
            }
        }

        public void PrintOperationSystem(OSReport rep)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\nOS CONFIGURATIONS:");
            Console.ResetColor();

            Console.WriteLine($"\nSystem: {rep.Name} ({rep.Manufacturer})");
            Console.WriteLine($"Version: {rep.Version}, installed {rep.InstallDate}");
            Console.WriteLine($"User: {rep.RegisteredUser}");
            Console.WriteLine($"Last boot: {rep.LastBootUpTime}");
        }
    }
}
