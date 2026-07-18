using Diagnostish.Desktop.Views.Helpers;
using Diagnostish.Domain.Models.Reports;

namespace Diagnostish.Desktop.Views.OperatingSystemInfoPrinters
{
    public class OperatingSystemInfoPrintToConsole : ReportPrinter<OperatingSystemReport>
    {
        protected override void PrintReport(OperatingSystemReport operatingSystemReport)
        {
            ColorPrinter.WriteLineColored("\nКОНФИГУРАЦИЯ ОС:", ConsoleColor.Cyan);

            Console.WriteLine($"\n1) Система: {operatingSystemReport.OperatingSystemName} ({operatingSystemReport.OperatingSystemManufacturer})");
            Console.WriteLine($"   Версия: {operatingSystemReport.OperatingSystemVersion}, установлена: {operatingSystemReport.OperatingSystemInstallDate}");
            Console.WriteLine($"2) Пользователь: {operatingSystemReport.OperatingSystemRegisteredUser}");
            Console.WriteLine($"3) Последнее включение: {operatingSystemReport.OperatingSystemLastBootUpTime}");

            PrintIssues(operatingSystemReport.Errors, operatingSystemReport.CriticalErrors);
        }

    }
}
