using Diagnostish.Desktop.Views.Common;
using Diagnostish.Desktop.Views.ConsoleViews.Common;

namespace Diagnostish.Desktop.Views.ConsoleViews;

public class OperatingSystemConsolePrinter : BaseConsolePrinter<OperatingSystemReport>
{
    protected override void PrintReport(OperatingSystemReport report)
    {
        ColorPrinter.WriteLineColored(
            "\nКОНФИГУРАЦИЯ ОС:", 
            ConsoleColor.Cyan);

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