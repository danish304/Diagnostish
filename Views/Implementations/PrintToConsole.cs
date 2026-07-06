using Diagnostish.Models;
using Diagnostish.Views.Interfaces;

namespace Diagnostish.Views.Implementations
{
    public class PrintToConsole : IPrintHW, IPrintOS, IUserInterface
    {
        public void PrintHardware(HWReport rep)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\nКОНФИГУРАЦИЯ ПК:");
            Console.ResetColor();

            Console.WriteLine($"\nПроцессор: {rep.ProcessorName} ({rep.CoresCount} ядер), частота - {rep.CurrentClockSpeed} MHz");
            Console.WriteLine($"ОЗУ: {rep.RAMSize} GB, {rep.RAMSpeed} MHz");
            Console.WriteLine("Видеокарты:");
            foreach (var gpu in rep.VideoCards)
            {
                Console.WriteLine($" - {gpu}");
            }
            Console.WriteLine("Накопители:");
            foreach (var drives in rep.Drives)
            {
                Console.WriteLine($" - {drives}");
            }

            PrintErrors(rep.Errors);
            PrintCriticalErrors(rep.CriticalErrors);
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

            PrintErrors(rep.Errors);
            PrintCriticalErrors(rep.CriticalErrors);
        }

        public void ShowWelcome()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            try
            {
                Console.Title = "Diagnostish";
                Console.Clear();
            }
            catch (IOException) { }

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("ЗАПУСК ДИАГНОСТИКИ . . .");
            Console.ResetColor();
        }

        public void WaitForExit()
        {
            if (Console.IsInputRedirected)
            {
                return;
            }

            try
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\nДля завершения нажмите любую клавишу . . .");
                Console.ResetColor();
                Console.ReadKey();
            }
            catch (InvalidOperationException) { }
        }

        private void PrintErrors(List<string> warnings)
        {
            if (warnings.Count > 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nПредупреждения:");
                foreach (var warn in warnings) Console.WriteLine($"  - {warn}");
            }
            Console.ResetColor();
        }

        private void PrintCriticalErrors(List<string> errors)
        {
            if (errors.Count > 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nКритические ошибки:");
                foreach (var warn in errors) Console.WriteLine($"  - {warn}");
            }
            Console.ResetColor();
        }
    }
}
