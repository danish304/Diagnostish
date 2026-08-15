using Desktop.Views.Common;
using Desktop.Views.FileViews.Common;

namespace Desktop.Views.FileViews;

public class OperatingSystemFilePrinter : BaseFilePrinter<OperatingSystemReport>
{
    public OperatingSystemFilePrinter(CommonReportFile reportFile) : base(reportFile)
    {
    }

    protected override void PrintReport(OperatingSystemReport report)
    {
        Writer.WriteLine("\nКОНФИГУРАЦИЯ ОС:");

        Writer.WriteLine(
            $"\n1) Система: {report.OperatingSystemName} ({report.OperatingSystemManufacturer})");

        Writer.WriteLine(
            $"   Версия: {report.OperatingSystemVersion}, " +
            $"установлена: {report.OperatingSystemInstallDate.FormatDate()}");

        Writer.WriteLine(
            $"2) Пользователь: {report.OperatingSystemRegisteredUser}");

        Writer.WriteLine(
            $"3) Последнее включение: {report.OperatingSystemLastBootUpTime.FormatDate()}");

        PrintIssues(report.Warnings, report.CriticalErrors);
    }
}