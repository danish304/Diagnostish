namespace Desktop.Views.Common.Printers;

public class HardwarePrinter(
    TextWriter writer,
    ILineWriter? lineWriter = null)
    : BasePrinter<HardwareReport>(writer, lineWriter)
{
    protected override void PrintReport(HardwareReport report)
    {
        WriteHeader("\nКОНФИГУРАЦИЯ ПК:\n", ConsoleColor.Cyan);

        reportComponentIndex = 1;

        PrintCpu(report);
        PrintGpu(report);
        PrintStorageDrives(report);
        PrintRam(report);
        PrintBaseBoard(report);
        PrintBios(report);

        PrintIssues(report.Warnings, report.CriticalErrors);
    }

    private void PrintCpu(HardwareReport report)
    {
        writer.WriteLine(
            $"{reportComponentIndex++}) Процессор: {report.CpuName} ({report.CpuCores} ядер), " +
            $"частота - {report.CpuClockSpeed} MHz");
    }

    private void PrintGpu(HardwareReport report)
    {
        writer.Write($"{reportComponentIndex++}) Видеокарты: ");

        if (report.VideoCards.Count > 0)
        {
            writer.WriteLine();
            foreach (var gpu in report.VideoCards)
            {
                writer.WriteLine($"    - {gpu.Name} ({gpu.AdapterRam} GB)");
            }
        }
        else
        {
            writer.WriteLine("Данные не получены.");
        }
    }

    private void PrintStorageDrives(HardwareReport report)
    {
        writer.Write($"{reportComponentIndex++}) Накопители: ");
        if (report.StorageDrives.Count > 0)
        {
            writer.WriteLine();
            foreach (var storageDrive in report.StorageDrives)
            {
                writer.WriteLine(
                    $"    - {storageDrive.Model} ({storageDrive.Size} GB), " +
                    $"cтатус: {storageDrive.Status}");
            }
        }
        else
        {
            writer.WriteLine("Данные не получены.");
        }
    }

    private void PrintRam(HardwareReport report)
    {
        writer.WriteLine(
            $"{reportComponentIndex++}) ОЗУ: {report.RamType} {report.RamCapacity} GB, " +
            $"{report.RamSpeed} MHz");
    }

    private void PrintBaseBoard(HardwareReport report)
    {
        writer.WriteLine(
            $"{reportComponentIndex++}) " +
            $"Материнская плата: {report.BaseBoardModel} ({report.BaseBoardManufacturer}), " +
            $"версия: {report.BaseBoardVersion}, " +
            $"cтатус: {report.BaseBoardStatus}");
    }

    private void PrintBios(HardwareReport report)
    {
        writer.WriteLine(
            $"{reportComponentIndex++}) " +
            $"BIOS: {report.BiosVersion} ({report.BiosManufacturer}), " +
            $"дата релиза - {report.BiosReleaseDate.FormatDate()}");
    }
}