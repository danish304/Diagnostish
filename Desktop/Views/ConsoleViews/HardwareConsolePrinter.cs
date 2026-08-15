using Desktop.Views.Common;
using Desktop.Views.ConsoleViews.Common;

namespace Desktop.Views.ConsoleViews;

public class HardwareConsolePrinter : BaseConsolePrinter<HardwareReport>
{
    protected override void PrintReport(HardwareReport report)
    {
        ColorPrinter.WriteLineColored("\nКОНФИГУРАЦИЯ ПК:", ConsoleColor.Cyan);

        Console.WriteLine(
            $"\n1) Процессор: {report.CpuName} ({report.CpuCores} ядер), " +
            $"частота - {report.CpuClockSpeed} MHz");

        Console.WriteLine(
            $"2) ОЗУ: {report.RamType} {report.RamCapacity} GB, " +
            $"{report.RamSpeed} MHz");

        Console.Write("3) Видеокарты: ");
        if (report.VideoCards.Count > 0)
        {
            Console.WriteLine();
            foreach (var gpu in report.VideoCards)
            {
                Console.WriteLine($"    - {gpu.Name} ({gpu.AdapterRam} GB)");
            }
        }
        else
        {
            Console.WriteLine("Данные не получены.");
        }

        Console.Write("4) Накопители: ");
        if (report.StorageDrives.Count > 0)
        {
            Console.WriteLine();
            foreach (var storageDrive in report.StorageDrives)
            {
                Console.WriteLine(
                    $"    - {storageDrive.Model} ({storageDrive.Size} GB), " +
                    $"cтатус: {storageDrive.Status}");
            }
        }
        else
        {
            Console.WriteLine("Данные не получены.");
        }

        Console.WriteLine(
            $"5) Материнская плата: {report.BaseBoardModel} ({report.BaseBoardManufacturer}), " +
            $"версия: {report.BaseBoardVersion}, " +
            $"cтатус: {report.BaseBoardStatus}");

        Console.WriteLine(
            $"6) BIOS: {report.BiosVersion} ({report.BiosManufacturer}), " +
            $"дата релиза - {report.BiosReleaseDate.FormatDate()}");

        PrintIssues(report.Warnings, report.CriticalErrors);
    }
}