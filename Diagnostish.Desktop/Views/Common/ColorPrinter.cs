namespace Diagnostish.Desktop.Views.Common;

public static class ColorPrinter
{
    public static void WriteLineColored(string text, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(text);
        Console.ResetColor();
    }
}