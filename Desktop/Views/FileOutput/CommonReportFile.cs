using Serilog;

namespace Desktop.Views.FileOutput;

public class CommonReportFile
{
    private const string REPORT_FILE_NAME = "report.txt";

    public StreamWriter Writer { get; } = StreamWriter.Null;

    public CommonReportFile()
    {
        string reportPath = Path.Combine(AppContext.BaseDirectory, REPORT_FILE_NAME);

        try
        {
            var stream = new FileStream(reportPath, FileMode.Create, FileAccess.Write, FileShare.Read);
            Writer = new StreamWriter(stream)
            {
                AutoFlush = true
            };

            Log.Information("Файл отчета успешно инициализирован: {Path}", reportPath);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Не удалось создать файл отчета {Path}.", reportPath);
        }
    }
}