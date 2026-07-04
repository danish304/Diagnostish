using Diagnostish.Models;

namespace Diagnostish.Views
{
    public class PrintToConsole : IPrintHW, IPrintOS, IUserInterface
    {
        private void PrintErrors(List<string> warnings)
        {
            if (warnings.Count > 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Предупреждения:");
                foreach (var warn in warnings) Console.WriteLine($"  - {warn}");
            }
            Console.ResetColor();
        }

        public void PrintHardware(HWReport rep)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\nКОНФИГУРАЦИЯ ПК:");
            Console.ResetColor();

            Console.WriteLine($"\nПроцессор: {rep.ProcessorName} ({rep.CoresCount} ядер), частота - {rep.CurrentClockSpeed} MGz");
            Console.WriteLine($"ОЗУ: {rep.RAMSize} GB, {rep.RAMSpeed} MGz");
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
        }

        public void Preview()
        {
            Console.Title = "Diagnostish";
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("ЗАПУСК ДИАГНОСТИКИ . . .");
            Console.ResetColor();
        }

        public void WaitForExit()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\nДля завершения нажмите любую клавишу . . .");
            Console.ResetColor();
            Console.ReadKey();
        }
    }
}
