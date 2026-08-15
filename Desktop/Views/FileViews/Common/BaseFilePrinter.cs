namespace Desktop.Views.FileViews.Common;

public abstract class BaseFilePrinter<TReport> : IReportPrinter<TReport>
{
    protected readonly StreamWriter Writer;

    protected BaseFilePrinter(CommonReportFile reportFile)
    {
        Writer = reportFile.Writer;
    }

    public void Print(TReport report) => PrintReport(report);

    protected abstract void PrintReport(TReport report);

    protected void PrintIssues(
        IReadOnlyList<string> warnings,
        IReadOnlyList<string> criticalErrors)
    {
        if (warnings.Count > 0)
        {
            Writer.WriteLine($"\n[ПРЕДУПРЕЖДЕНИЯ]:");

            foreach (var warning in warnings)
            {
                Writer.WriteLine($"    - {warning}");
            }
        }

        if (criticalErrors.Count > 0)
        {
            Writer.WriteLine($"\n[КРИТИЧЕСКИЕ ОШИБКИ]:");

            foreach (var error in criticalErrors)
            {
                Writer.WriteLine($"    - {error}");
            }
        }
    }
}