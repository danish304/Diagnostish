using Desktop.Views.Common;
using Desktop.Views.ConsoleViews.Common;

namespace Desktop.Views.ConsoleViews;

public class OperatingSystemConsolePrinter : BaseConsolePrinter<OperatingSystemReport>
{
    protected override void PrintReport(OperatingSystemReport report)
    {
        ColorPrinter.WriteLineColored("\nКОНФИГУРАЦИЯ ОС:", ConsoleColor.Cyan);

        Console.WriteLine(
            $"\n1) Система: {report.OperatingSystemName} ({report.OperatingSystemManufacturer})");

        Console.WriteLine(
            $"   Версия: {report.OperatingSystemVersion}, " +
            $"установлена: {report.OperatingSystemInstallDate.FormatDate()}");

        Console.WriteLine(
            $"2) Пользователь: {report.OperatingSystemRegisteredUser}");

        Console.WriteLine(
            $"3) Последнее включение: {report.OperatingSystemLastBootUpTime.FormatDate()}");

        PrintIssues(report.Warnings, report.CriticalErrors);
    }
}