namespace Desktop.Views.Common.Printers;

public class OperatingSystemPrinter(
    TextWriter writer,
    ILineWriter? lineWriter = null)
    : BasePrinter<OperatingSystemReport>(writer, lineWriter)
{
    protected override void PrintReport(OperatingSystemReport report)
    {
        WriteHeader("\nКОНФИГУРАЦИЯ ОС:\n", ConsoleColor.Cyan);

        reportComponentIndex = 1;

        PrintDescriptionAndVersion(report);
        PrintUser(report);
        PrintLastBoot(report);

        PrintIssues(report.Warnings, report.CriticalErrors);
    }

    private void PrintDescriptionAndVersion(OperatingSystemReport report)
    {
        writer.WriteLine(
            $"{reportComponentIndex++}) " +
            $"Система: {report.OperatingSystemName} ({report.OperatingSystemManufacturer})");

        writer.WriteLine(
            $"   Версия: {report.OperatingSystemVersion}, " +
            $"установлена: {report.OperatingSystemInstallDate.FormatDate()}");
    }

    private void PrintUser(OperatingSystemReport report)
    {
        writer.WriteLine(
            $"{reportComponentIndex++}) " +
            $"Пользователь: {report.OperatingSystemRegisteredUser}");
    }

    private void PrintLastBoot(OperatingSystemReport report)
    {
        writer.WriteLine(
            $"{reportComponentIndex++}) " +
            $"Последнее включение: {report.OperatingSystemLastBootUpTime.FormatDate()}");
    }
}