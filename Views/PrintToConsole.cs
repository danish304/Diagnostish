using Diagnostish.Models;

namespace Diagnostish.Views
{
    public class PrintToConsole : IPrintHW, IPrintOS
    {
        public void PrintHardware(HWReport rep)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\nКОНФИГУРАЦИЯ ПК:");
            Console.ResetColor();

            Console.WriteLine($"\nПроцессор: {rep.ProcessorName} ({rep.CoresCount} ядер), частота - {rep.CurrentClockSpeed} MGz");
            Console.WriteLine($"ОЗУ: {rep.RAMSize} GB, {rep.RAMSpeed} MGz");
            Console.WriteLine("Videocards:");
            foreach (var gpu in rep.VideoCards)
            {
                Console.WriteLine($" - {gpu}");
            }
            Console.WriteLine("Накопители:");
            foreach (var drives in rep.Drives)
            {
                Console.WriteLine($" - {drives}");
            }

            // Вывод некритичных ошибок
            if (rep.Errors.Count > 0)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Предупреждения при сканировании конфигурации ПК:");
                foreach (var error in rep.Errors)
                {
                    Console.WriteLine($"  - {error}");
                }
                Console.ResetColor();
            }
        }

        public void PrintOperationSystem(OSReport rep)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\nКОНФИГУРАЦИЯ ОС:");
            Console.ResetColor();

            Console.WriteLine($"\nСистема: {rep.Name} ({rep.Manufacturer})");
            Console.WriteLine($"Версия: {rep.Version}, установлена {rep.InstallDate}");
            Console.WriteLine($"Пользователь: {rep.RegisteredUser}");
            Console.WriteLine($"Последнее включение: {rep.LastBootUpTime}");

            // Вывод некритичных ошибок
            if (rep.Errors.Count > 0)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Предупреждения при сканировании конфигурации ОС:");
                foreach (var error in rep.Errors)
                {
                    Console.WriteLine($"  - {error}");
                }
                Console.ResetColor();
            }
        }
    }
}
