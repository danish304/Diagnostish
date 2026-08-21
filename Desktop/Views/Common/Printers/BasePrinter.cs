namespace Desktop.Views.Common.Printers;

public abstract class BasePrinter<TReport>(
    TextWriter writer,
    ILineWriter? lineWriter = null)
    : IReportPrinter<TReport>
{
    private readonly ILineWriter _lineWriter = lineWriter ?? new DefaultLineWriter();
    protected int reportComponentIndex;
    protected readonly TextWriter writer = writer;

    public void Print(TReport report) => PrintReport(report);

    protected abstract void PrintReport(TReport report);

    protected void WriteHeader(string text, ConsoleColor color)
        => _lineWriter.WriteLine(writer, text, color);

    protected void PrintIssues(
        IReadOnlyList<string> warnings,
        IReadOnlyList<string> criticalErrors)
    {
        if (warnings.Count > 0)
        {
            WriteHeader("\n* ПРЕДУПРЕЖДЕНИЯ *", ConsoleColor.DarkYellow);
            foreach (var warning in warnings)
                _lineWriter.WriteLine(writer, $"    - {warning}", ConsoleColor.Yellow);
        }

        if (criticalErrors.Count > 0)
        {
            WriteHeader("\n* КРИТИЧЕСКИЕ ОШИБКИ *", ConsoleColor.DarkRed);
            foreach (var error in criticalErrors)
                _lineWriter.WriteLine(writer, $"    - {error}", ConsoleColor.Red);
        }
    }
}