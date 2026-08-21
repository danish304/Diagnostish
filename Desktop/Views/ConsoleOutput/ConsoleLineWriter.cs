using Desktop.Views.Common;

namespace Desktop.Views.ConsoleOutput;

public class ConsoleLineWriter : ILineWriter
{
    public void WriteLine(TextWriter writer, string text, ConsoleColor color)
        => ColorPrinter.WriteLineColored(text, color);
}