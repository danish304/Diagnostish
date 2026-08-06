using Diagnostish.Desktop.Composition;
using Diagnostish.Desktop.Controllers;
using Diagnostish.Desktop.Views.Common;
using Diagnostish.Infrastructure.Shared.Wmi;
using Microsoft.Extensions.Configuration;
using Serilog;

static class Program
{
    static async Task Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Log.Logger = LoggerConfigurator.Create();
        var configuration = ConfigurationConfigurator.Create();

        using var cts = new CancellationTokenSource();
        Console.CancelKeyPress += (_, e) =>
        {
            e.Cancel = true;
            Log.Warning("Сканирование остановлено пользователем (Ctrl+C).");
            cts.Cancel();
        };

        try
        {
            Log.Information("Приложение Diagnostish запущено.");
            Log.Information("Таймаут WMI-запросов выставлен на {Timeout} сек.", 
                             configuration.GetValue("Wmi:WmiQueryTimeoutSeconds", 
                                                     new WmiSettings().WmiQueryTimeoutSeconds));

            var serviceProvider = new ServiceCollection()
                .AddSingleton(Log.Logger)
                .AddSingleton(configuration).Configure<WmiSettings>(configuration.GetSection("Wmi"))
                .AddCoreServices()
                .AddHardwareComponents()
                .AddOperatingSystemComponents()
                .AddPrinters()
                .BuildServiceProvider();

            await serviceProvider.GetRequiredService<DiagnosticController>().StartDiagnosticAsync(cts.Token);

            Log.Information("Приложение Diagnostish завершило свою работу.");
        }
        catch (Exception ex)
        {
            ColorPrinter.WriteLineColored("\nПРОИЗОШЛА КРИТИЧЕСКАЯ ОШИБКА!", ConsoleColor.Red);
            Log.Fatal(ex, "Необработанное исключение. Приложение аварийно завершило свою работу.");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}