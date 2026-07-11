using Diagnostish.Models;
using Diagnostish.Views.Interfaces;

namespace Diagnostish.Views.Implementations
{
    public class PrintToConsole : IPrintHW, IPrintOS, IUserInterface
    {
        public void PrintHardware(HWReport rep)
        {
            WriteLineColored("\nКОНФИГУРАЦИЯ ПК:", ConsoleColor.Cyan);
            Console.WriteLine($"\n1) Процессор: {rep.ProcessorName} ({rep.CoresCount} ядер), частота - {rep.CurrentClockSpeed} MHz");
            Console.WriteLine($"2) ОЗУ: {rep.RAMType} {rep.RAMSize} GB, {rep.RAMSpeed} MHz");

            Console.WriteLine("3) Видеокарты:");
            foreach (var (videoCard, size) in rep.VideoCards.Zip(rep.AdaptersRAM))
            {
                Console.WriteLine($"    - {videoCard} ({size} GB)");
            }

            Console.WriteLine("4) Накопители:");
            foreach (var (drive, size) in rep.ModelsDrives.Zip(rep.DrivesSize))
            {
                Console.WriteLine($"    - {drive} ({size} GB)");
            }

            Console.WriteLine($"5) Материнская плата: {rep.BaseBoardModel} ({rep.BaseBoardManufacturer}), версия {rep.BaseBoardVersion}");
            Console.WriteLine($"   Статус платы: {rep.BaseBoardStatus}");

            Console.WriteLine($"6) BIOS: {rep.BIOSVersion} ({rep.BIOSManufacturer}), дата релиза - {rep.BIOSReleaseDate}");

            PrintIssues(rep.Errors, rep.CriticalErrors);
        }

        public void PrintOperationSystem(OSReport rep)
        {
            WriteLineColored("\nКОНФИГУРАЦИЯ ОС:", ConsoleColor.Cyan);

            Console.WriteLine($"\nСистема: {rep.Name} ({rep.Manufacturer})");
            Console.WriteLine($"Версия: {rep.Version}, установлена: {rep.InstallDate}");
            Console.WriteLine($"Пользователь: {rep.RegisteredUser}");
            Console.WriteLine($"Последнее включение: {rep.LastBootUpTime}");

            PrintIssues(rep.Errors, rep.CriticalErrors);
        }

        public void ShowWelcome()
        {
            try
            {
                Console.Title = "Diagnostish";
                Console.Clear();
            }
            catch (IOException) { }

            WriteLineColored("ЗАПУСК ДИАГНОСТИКИ . . .", ConsoleColor.Magenta);
        }

        public void WaitForExit()
        {
            WriteLineColored("\nСканирование завершено!", ConsoleColor.Green);

            if (!Console.IsInputRedirected)
            {
                WriteLineColored("Для завершения нажмите любую клавишу . . .", ConsoleColor.DarkGray);
                Console.ReadKey();
            }
        }

        private static void PrintIssues(List<string> errors, List<string> criticalErrors)
        {
            if (errors.Count > 0)
            {
                Console.WriteLine("\nПРЕДУПРЕЖДЕНИЯ:");
                foreach (var warn in errors) WriteLineColored($"  - {warn}", ConsoleColor.Yellow);
            }
            
            if (criticalErrors.Count > 0)
            {
                Console.WriteLine("\nКРИТИЧЕСКИЕ ОШИБКИ:");
                foreach (var warn in criticalErrors) WriteLineColored($"  - {warn}", ConsoleColor.Red);
            }
        }

        private static void WriteLineColored(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }
    }
}
