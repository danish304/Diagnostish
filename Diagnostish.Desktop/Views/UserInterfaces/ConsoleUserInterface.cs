using Diagnostish.Desktop.Views.Helpers;
using Diagnostish.Domain.Interfaces;

namespace Diagnostish.Desktop.Views.UserInterfaces
{
    public class ConsoleUserInterface : IUserInterface
    {
        public void ShowWelcome()
        {
            try
            {
                Console.Title = "Diagnostish";
                Console.Clear();
            }
            catch (IOException) { }

            ColorPrinter.WriteLineColored("ЗАПУСК ДИАГНОСТИКИ . . .", ConsoleColor.Magenta);
        }

        public void WaitForExit()
        {
            ColorPrinter.WriteLineColored("\nСКАНИРОВАНИЕ ЗАВЕРШЕНО!", ConsoleColor.Green);

            if (!Console.IsInputRedirected)
            {
                ColorPrinter.WriteLineColored("Для завершения нажмите любую клавишу . . .", ConsoleColor.DarkGray);
                Console.ReadKey();
            }
        }
    }
}
