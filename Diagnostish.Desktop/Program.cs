using Diagnostish.Application.Services;
using Diagnostish.Desktop.Controllers;
using Diagnostish.Desktop.Views;
using Diagnostish.Desktop.Views.HardwareInfoPrinters;
using Diagnostish.Desktop.Views.Helpers;
using Diagnostish.Desktop.Views.OperatingSystemInfoPrinters;
using Diagnostish.Desktop.Views.UserInterfaces;
using Diagnostish.Domain.Interfaces;
using Diagnostish.Domain.Models.Entities;
using Diagnostish.Domain.Models.Reports;
using Diagnostish.Infrastructure.HardwareInfoAnalyzers;
using Diagnostish.Infrastructure.HardwareInfoProviders;
using Diagnostish.Infrastructure.OperatingSystemInfoProviders;
using Diagnostish.Infrastructure.WmiHelpers;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

static class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()  
            .WriteTo.File("logs/diagnostish-.txt", 
                          rollingInterval: RollingInterval.Day,
                          outputTemplate: "{Timestamp:HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}------------------------------------{NewLine}") 
            .CreateLogger();

        try
        {
            Log.Information("Приложение Diagnostish запущено.");

            var services = new ServiceCollection();

            services.AddSingleton<IExecutorWmi, ExecutorWmi>();

            services.AddSingleton<IProvideHardwareInfo, BaseboardInfoWmiProvider>();
            services.AddSingleton<IProvideHardwareInfo, BiosInfoWmiProvider>();
            services.AddSingleton<IProvideHardwareInfo, CpuInfoWmiProvider>();
            services.AddSingleton<IProvideHardwareInfo, DrivesInfoWmiProvider>();
            services.AddSingleton<IProvideHardwareInfo, GpuInfoWmiProvider>();
            services.AddSingleton<IProvideHardwareInfo, RamInfoWmiProvider>();
            services.AddSingleton<IProvideOperatingSystemInfo, OperatingSystemInfoWmiProvider>();

            services.AddSingleton<IAnalyzeHardwareInfo<RamInfo, HardwareReport>, RamInfoAnalyzer>();

            services.AddSingleton<ServicesAggregator>();

            services.AddSingleton<IReportPrinter<HardwareReport>, HardwareInfoPrintToConsole>();
            services.AddSingleton<IReportPrinter<OperatingSystemReport>, OperatingSystemInfoPrintToConsole>();
            services.AddSingleton<IUserInterface, ConsoleUserInterface>();

            services.AddSingleton<DiagnosticController>();

            var serviceProvider = services.BuildServiceProvider();

            var controller = serviceProvider.GetRequiredService<DiagnosticController>();
            controller.StartDiagnostic();

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