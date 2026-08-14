using Desktop.Views.ConsoleViews.Common;

namespace Desktop.Views.UserInterfaces;

public class ConsoleUserInterface : IUserInterface
{
    public void ShowWelcome()
    {
        try
        {
            Console.Title = "Diagnostish";
            Console.Clear();
        }
        catch (IOException)
        {
            // Игнорируем, если терминал не поддерживает изменение заголовка или очистку
        }

        ColorPrinter.WriteLineColored("ЗАПУСК ДИАГНОСТИКИ . . .", ConsoleColor.Magenta);
    }

    public void WaitForExit()
    {
        ColorPrinter.WriteLineColored("\nСКАНИРОВАНИЕ ЗАВЕРШЕНО!", ConsoleColor.Green);

        if (!Console.IsInputRedirected)
        {
            ColorPrinter.WriteLineColored(
                "Для завершения нажмите любую клавишу . . .", ConsoleColor.DarkGray);

            Console.ReadKey(intercept: true);
        }
    }
}