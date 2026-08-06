using Diagnostish.Desktop.Views.Common;

namespace Diagnostish.Desktop.Views.OperatingSystemInfoPrinters;

public class OperatingSystemInfoPrintToConsole : BaseReportPrinter<OperatingSystemReport>
{
    protected override void PrintReport(OperatingSystemReport operatingSystemReport)
    {
        ColorPrinter.WriteLineColored("\nКОНФИГУРАЦИЯ ОС:", ConsoleColor.Cyan);

        Console.WriteLine($"\n1) Система: {operatingSystemReport.OperatingSystemName} ({operatingSystemReport.OperatingSystemManufacturer})");
        Console.WriteLine($"   Версия: {operatingSystemReport.OperatingSystemVersion}, установлена: {FormattingData.FormatDate(operatingSystemReport.OperatingSystemInstallDate)}");

        Console.WriteLine($"2) Пользователь: {operatingSystemReport.OperatingSystemRegisteredUser}");

        Console.WriteLine($"3) Последнее включение: {FormattingData.FormatDate(operatingSystemReport.OperatingSystemLastBootUpTime)}");

        PrintIssues(operatingSystemReport.Warnings, operatingSystemReport.CriticalErrors);
    }
}