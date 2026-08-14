namespace Desktop.Views.ConsoleViews.Common;

public abstract class BaseConsolePrinter<TReport> : IReportPrinter<TReport>
{
    public void Print(TReport report) => PrintReport(report);

    protected abstract void PrintReport(TReport report);

    protected void PrintIssues(
        IReadOnlyList<string> warnings,
        IReadOnlyList<string> criticalErrors)
    {
        if (warnings.Count > 0)
        {
            ColorPrinter.WriteLineColored("\n* ПРЕДУПРЕЖДЕНИЯ *", ConsoleColor.DarkYellow);

            foreach (var warning in warnings)
            {
                ColorPrinter.WriteLineColored($"    - {warning}", ConsoleColor.Yellow);
            }
        }

        if (criticalErrors.Count > 0)
        {
            ColorPrinter.WriteLineColored("\n* КРИТИЧЕСКИЕ ОШИБКИ *", ConsoleColor.DarkRed);

            foreach (var error in criticalErrors)
            {
                ColorPrinter.WriteLineColored($"    - {error}", ConsoleColor.Red);
            }
        }
    }
}