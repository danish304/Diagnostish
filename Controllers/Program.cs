using Diagnostish.Models;
using Diagnostish.Services;
using Diagnostish.View;

static class Program
{
    static void Main(string[] args)
    {
        Console.Title = "Diagnostish";
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("Starting . . .");
        System.Threading.Thread.Sleep(2000);
        Console.ForegroundColor= ConsoleColor.Yellow;
        Console.WriteLine("Scanning configurations, please wait . . .");
        System.Threading.Thread.Sleep(2000);

        PrintToConsole printToConsole = new PrintToConsole();

        HWCheck check_HW = new HWCheck();
        HWReport report_HW = check_HW.CheckPCCFG();
        printToConsole.PrintHardware(report_HW);

        OSCheck check_OS = new OSCheck();
        OSReport report_OS = check_OS.CheckOSCFG();
        printToConsole.PrintOperationSystem(report_OS);
        
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\nDone! Press any key to continue . . .");
        Console.ReadKey();
    }
}