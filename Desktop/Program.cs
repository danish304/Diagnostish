using Desktop.Composition;
using Desktop.Controllers;
using Desktop.Views.ConsoleViews.Common;
using Infrastructure.Shared.Wmi;
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

            var wmiSettings = configuration.GetSection("Wmi").Get<WmiSettings>() ?? new WmiSettings();
            Log.Information(
                "Таймаут WMI-запросов выставлен на {Timeout} сек.",
                wmiSettings.WmiQueryTimeoutSeconds);

            var services = new ServiceCollection()
                .AddSingleton(Log.Logger)
                .Configure<WmiSettings>(configuration.GetSection("Wmi"))
                .AddCoreServices()
                .AddHardwareComponents()
                .AddOperatingSystemComponents()
                .AddNetworkComponents()
                .AddPrinters()
                .AddUserInterfaces();

            using var serviceProvider = services.BuildServiceProvider();

            var controller = serviceProvider.GetRequiredService<DiagnosticController>();
            await controller.StartDiagnosticAsync(cts.Token);

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