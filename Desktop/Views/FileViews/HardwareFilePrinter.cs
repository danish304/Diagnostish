using Desktop.Views.Common;
using Desktop.Views.FileViews.Common;

namespace Desktop.Views.FileViews;

public class HardwareFilePrinter : BaseFilePrinter<HardwareReport>
{
    public HardwareFilePrinter(CommonReportFile reportFile) : base(reportFile)
    {
    }

    protected override void PrintReport(HardwareReport report)
    {
        Writer.WriteLine("\nКОНФИГУРАЦИЯ ПК:");

        Writer.WriteLine(
            $"\n1) Процессор: {report.CpuName} ({report.CpuCores} ядер), " +
            $"частота - {report.CpuClockSpeed} MHz");

        Writer.WriteLine(
            $"2) ОЗУ: {report.RamType} {report.RamCapacity} GB, " +
            $"{report.RamSpeed} MHz");

        Writer.Write("3) Видеокарты: ");
        if (report.VideoCards.Count > 0)
        {
            Writer.WriteLine();
            foreach (var gpu in report.VideoCards)
            {
                Writer.WriteLine($"    - {gpu.Name} ({gpu.AdapterRam} GB)");
            }
        }
        else
        {
            Writer.WriteLine("Данные не получены.");
        }

        Writer.Write("4) Накопители: ");
        if (report.StorageDrives.Count > 0)
        {
            Writer.WriteLine();
            foreach (var storageDrive in report.StorageDrives)
            {
                Writer.WriteLine(
                    $"    - {storageDrive.Model} ({storageDrive.Size} GB), " +
                    $"cтатус: {storageDrive.Status}");
            }
        }
        else
        {
            Writer.WriteLine("Данные не получены.");
        }

        Writer.WriteLine(
            $"5) Материнская плата: {report.BaseBoardModel} ({report.BaseBoardManufacturer}), " +
            $"версия: {report.BaseBoardVersion}, " +
            $"cтатус: {report.BaseBoardStatus}");

        Writer.WriteLine(
            $"6) BIOS: {report.BiosVersion} ({report.BiosManufacturer}), " +
            $"дата релиза - {report.BiosReleaseDate.FormatDate()}");

        PrintIssues(report.Warnings, report.CriticalErrors);
    }
}