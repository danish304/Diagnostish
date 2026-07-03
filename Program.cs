using Diagnostish.Models;
using Diagnostish.Services;

static class Program
{
    static void Main(string[] args)
    {
        Console.Title = "Diagnostish";
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Program \"DIAGNOSTISH\" by Danish is starting...");
        Console.ResetColor();

        HWCheck check_HW = new HWCheck();
        HWReport report_HW = check_HW.CheckPCCFG();
        PrintPCCFG(report_HW);
        Continue();

        OSCheck check_OS = new OSCheck();
        OSReport report_OS = check_OS.CheckOSCFG();
        PrintOSCFG(report_OS);
        Continue();

        StartupCheck check_Startup = new StartupCheck();
        List<StartupReport> report_Startup = check_Startup.GetStartupPrograms();
        PringStartup(report_Startup);
        Continue();
    }

    // Вывод сообщения о продолжении
    static void Continue()
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("\nPress any key to continue . . .");
        Console.ResetColor();
        Console.ReadKey();
    }

    // Вывод полученной конфигурации ПК
    static void PrintPCCFG(HWReport rep)
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine($"\n{rep.CheckTime}");

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\nPC CONFIGURATIONS:");
        Console.ResetColor();

        Console.WriteLine($"\nProcessor: {rep.ProcessorName} ({rep.CoresCount} cores), clock speed - {rep.CurrentClockSpeed} MGz");
        Console.WriteLine($"RAM: {rep.RAMSize} GB, {rep.RAMSpeed} MGz");
        Console.WriteLine("Videocards:");
        foreach (var gpu in rep.VideoCards)
        {
            Console.WriteLine($"  - {gpu}");
        }
        Console.WriteLine("Drives:");
        foreach (var drives in rep.Drives)
        {
            Console.WriteLine($"  - {drives}");
        }
    }

    // Вывод полученной конфигурации системы
    static void PrintOSCFG(OSReport rep)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\nOS CONFIGURATIONS:");
        Console.ResetColor();

        Console.WriteLine($"\nSystem: {rep.Name} ({rep.Manufacturer})");
        Console.WriteLine($"Version: {rep.Version}, installed {rep.InstallDate}");
        Console.WriteLine($"User: {rep.RegisteredUser}");
        Console.WriteLine($"Last boot: {rep.LastBootUpTime}");
    }

    // Вывод полученной информации об автозапуске
    static void PringStartup(List<StartupReport> rep)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\nSTARTUP CONFIGURATIONS:");
        Console.ResetColor();
        Console.WriteLine();

        foreach (var app in rep)
        {
            Console.WriteLine($"  - {app.Name}, {app.Command}");
        }
    }
}