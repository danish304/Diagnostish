namespace Desktop.Views.Common;

public class DefaultLineWriter : ILineWriter
{
    public void WriteLine(TextWriter writer, string text, ConsoleColor color)
        => writer.WriteLine(text);
}