namespace Desktop.Views.ConsoleOutput;

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
            // Игнорируем
        }

        ColorPrinter.WriteLineColored("ЗАПУСК ДИАГНОСТИКИ . . .", ConsoleColor.Magenta);
    }

    public void ShowCompletion()
    {
        ColorPrinter.WriteLineColored("\nСКАНИРОВАНИЕ ЗАВЕРШЕНО!", ConsoleColor.Green);

        WaitKeyForExit();
    }

    private static void WaitKeyForExit()
    {
        if (Console.IsInputRedirected)
        {
            return;
        }

        ColorPrinter.WriteLineColored(
            "Для завершения нажмите любую клавишу . . .", ConsoleColor.DarkGray);

        Console.ReadKey(intercept: true);
    }
}