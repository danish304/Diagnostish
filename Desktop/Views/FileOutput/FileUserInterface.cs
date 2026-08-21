namespace Desktop.Views.FileOutput;

public class FileUserInterface : IUserInterface
{
    private readonly StreamWriter _writer;

    public FileUserInterface(CommonReportFile reportFile)
    {
        _writer = reportFile.Writer;
    }

    public void ShowWelcome()
    {
        _writer.WriteLine($"ЗАПУСК ДИАГНОСТИКИ ({DateTime.Now:yyyy-MM-dd HH:mm:ss})");
    }

    public void ShowCompletion()
    {
        _writer.WriteLine($"\nСКАНИРОВАНИЕ ЗАВЕРШЕНО! ({DateTime.Now:yyyy-MM-dd HH:mm:ss})");
    }
}