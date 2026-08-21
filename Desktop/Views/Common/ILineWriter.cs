namespace Desktop.Views.Common;

public interface ILineWriter
{
    void WriteLine(TextWriter writer, string text, ConsoleColor color);
}