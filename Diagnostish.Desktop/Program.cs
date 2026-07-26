using Diagnostish.Application.Mappers.HardwareMappers;
using Diagnostish.Application.Mappers.OperatingSystemMappers;
using Diagnostish.Application.Services;
using Diagnostish.Desktop.Composition;
using Diagnostish.Desktop.Controllers;
using Diagnostish.Desktop.Views;
using Diagnostish.Desktop.Views.Common;
using Diagnostish.Desktop.Views.HardwareInfoPrinters;
using Diagnostish.Desktop.Views.OperatingSystemInfoPrinters;
using Diagnostish.Desktop.Views.UserInterfaces;
using Diagnostish.Domain.Interfaces;
using Diagnostish.Domain.Models.Entities;
using Diagnostish.Domain.Models.Entities.Hardware;
using Diagnostish.Domain.Models.Reports;
using Diagnostish.Infrastructure.Analyzers.HardwareInfoAnalyzers;
using Diagnostish.Infrastructure.Analyzers.OperatingSystemInfoAnalyzers;
using Diagnostish.Infrastructure.Providers.HardwareInfoProviders;
using Diagnostish.Infrastructure.Providers.HardwareInfoProviders.RawHardwareInfo;
using Diagnostish.Infrastructure.Providers.OperatingSystemInfoProviders;
using Diagnostish.Infrastructure.Shared.Wmi;
using Diagnostish.Infrastructure.Shared.Wmi.Executor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

static class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Log.Logger = LoggerConfigurator.Create();

        try
        {
            Log.Information("Приложение Diagnostish запущено.");

            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
                .Build();

            Log.Information("Конфигурация: Таймаут WMI-запросов выставлен на {Timeout} сек.", configuration.GetValue<int>("Wmi:WmiQueryTimeoutSeconds"));

            var services = new ServiceCollection()
                .AddSingleton(Log.Logger)
                .AddSingleton<IConfiguration>(configuration)
                    .Configure<WmiSettings>(configuration.GetSection("Wmi"))

                .AddSingleton<IExecutorWmi, ExecutorWmi>()

                .AddComponent<HardwareReport, 
                              RawBaseBoardInfo, BaseBoardInfo, 
                              BaseBoardInfoWmiProvider, BaseBoardInfoAnalyzer, BaseBoardReportMapper>()
                .AddComponent<HardwareReport,
                              RawBiosInfo, BiosInfo, 
                              BiosInfoWmiProvider, BiosInfoAnalyzer, BiosReportMapper>()
                .AddComponent<HardwareReport, 
                              RawCpuInfo, CpuInfo, 
                              CpuInfoWmiProvider, CpuInfoAnalyzer, CpuReportMapper>()
                .AddComponent<HardwareReport, 
                              RawGpuInfo, IReadOnlyList<GpuInfo>, 
                              GpuInfoWmiProvider, GpuInfoAnalyzer, GpuReportMapper>()
                .AddComponent<HardwareReport, 
                              RawRamInfo, RamInfo, 
                              RamInfoWmiProvider, RamInfoAnalyzer, RamReportMapper>()
                .AddComponent<HardwareReport, 
                              RawStorageDriveInfo, IReadOnlyList<StorageDriveInfo>, 
                              StorageDriveInfoWmiProvider, StorageDriveInfoAnalyzer, StorageDriveReportMapper>()
                .AddComponent<OperatingSystemReport, 
                              RawOperatingSystemInfo, OperatingSystemInfo, 
                              OperatingSystemInfoWmiProvider, OperatingSystemInfoAnalyzer, OperatingSystemReportMapper>()

                .AddPrinter<HardwareReport, HardwareInfoPrintToConsole>()
                .AddPrinter<OperatingSystemReport, OperatingSystemInfoPrintToConsole>()

                .AddSingleton<IUserInterface, ConsoleUserInterface>()
                .AddSingleton<ServicesAggregator>()
                .AddSingleton<PrintersAggregator>()
                .AddSingleton<DiagnosticController>();

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