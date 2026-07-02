using System;
using Diagnostish.Models;
using Diagnostish.Services;

static class Program
{
    static void Main(string[] args)
    {
        Console.Title = "Diagnostish";
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Program \"DIAGNOSTISH\" is starting...");
        Console.ResetColor();

        HWCheck check = new HWCheck();
        HWReport report = check.CheckSystemCFG();

        PrintSystemCFG(report);
    }

    // Вывод полученной конфигурации системы на экран
    static void PrintSystemCFG(HWReport rep)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\nSYSTEM CONFIGURATIONS:");
        Console.ResetColor();

        Console.WriteLine($"\nProcessor: {rep.ProcessorName}");
        Console.WriteLine($"Cores count: {rep.CoresCount}");
        Console.WriteLine($"RAM: {rep.GBRAM} GB");

        Console.WriteLine("Videocards:");
        foreach (var gpu in rep.VideoCards)
        {
            Console.WriteLine($"  - {gpu}");
        }
        
        Console.ReadKey();
    }
}