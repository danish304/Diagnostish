using Diagnostish.Desktop.Views.Helpers;

namespace Diagnostish.Desktop.Views
{
    public interface IReportPrinter<T>
    {
        void Print(T report);
    }

    public abstract class ReportPrinter<TReport> : IReportPrinter<TReport>
    {
        public void Print(TReport report)
        {
            PrintReport(report);
        }

        protected abstract void PrintReport(TReport report);

        protected void PrintIssues(List<string> errors, List<string> criticalErrors)
        {
            if (errors.Count > 0)
            {
                ColorPrinter.WriteLineColored("\n* ПРЕДУПРЕЖДЕНИЯ *", ConsoleColor.DarkYellow);
                foreach (var warn in errors) ColorPrinter.WriteLineColored($"    - {warn}", ConsoleColor.Yellow);
            }

            if (criticalErrors.Count > 0)
            {
                ColorPrinter.WriteLineColored("\n* КРИТИЧЕСКИЕ ОШИБКИ *", ConsoleColor.DarkRed);
                foreach (var warn in criticalErrors) ColorPrinter.WriteLineColored($"    - {warn}", ConsoleColor.Red);
            }
        }
    }
}
