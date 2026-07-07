using Diagnostish.Helpers;
using Diagnostish.Models;
using Diagnostish.Views.Interfaces;

namespace Diagnostish.Views.Implementations
{
    public class PrintToConsole : IPrintHW, IPrintOS, IUserInterface
    {
        public void PrintHardware(HWReport rep)
        {
            ConsoleWriter.WriteLineColored("\nКОНФИГУРАЦИЯ ПК:", ConsoleColor.Cyan);

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

            PrintIssues(rep.Errors, rep.CriticalErrors);
        }

        public void PrintOperationSystem(OSReport rep)
        {
            ConsoleWriter.WriteLineColored("\nКОНФИГУРАЦИЯ ОС:", ConsoleColor.Cyan);

            Console.WriteLine($"\nСистема: {rep.Name} ({rep.Manufacturer})");
            Console.WriteLine($"Версия: {rep.Version}, установлена {rep.InstallDate}");
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

            ConsoleWriter.WriteLineColored("ЗАПУСК ДИАГНОСТИКИ . . .", ConsoleColor.Magenta);
        }

        public void WaitForExit()
        {
            ConsoleWriter.WriteLineColored("\nСканирование завершено!", ConsoleColor.Green);

            if (!Console.IsInputRedirected)
            {
                ConsoleWriter.WriteLineColored("Для завершения нажмите любую клавишу . . .", ConsoleColor.DarkGray);
                Console.ReadKey();
            }
        }

        private void PrintIssues(List<string> errors, List<string> criticalErrors)
        {
            if (errors.Count > 0)
            {
                Console.WriteLine("\nПРЕДУПРЕЖДЕНИЯ:");
                foreach (var warn in errors) ConsoleWriter.WriteLineColored($"  - {warn}", ConsoleColor.Yellow);
            }
            
            if (criticalErrors.Count > 0)
            {
                Console.WriteLine("\nКРИТИЧЕСКИЕ ОШИБКИ:");
                foreach (var warn in criticalErrors) ConsoleWriter.WriteLineColored($"  - {warn}", ConsoleColor.Red);
            }
        }
    }
}
