using Diagnostish.Application.Mappers.Hardware;
using Diagnostish.Application.Mappers.OperatingSystem;
using Diagnostish.Application.Pipelines;
using Diagnostish.Application.Services;
using Diagnostish.Desktop.Controllers;
using Diagnostish.Desktop.Views;
using Diagnostish.Desktop.Views.ConsoleViews;
using Diagnostish.Desktop.Views.UserInterfaces;
using Diagnostish.Domain.Models.Entities.Hardware;
using Diagnostish.Infrastructure.Analyzers.Hardware;
using Diagnostish.Infrastructure.Analyzers.OperatingSystem;
using Diagnostish.Infrastructure.Providers;
using Diagnostish.Infrastructure.Providers.Common.RawModels.Hardware;
using Diagnostish.Infrastructure.Providers.Common.RawModels.OperatingSystem;
using Diagnostish.Infrastructure.Providers.Registry;
using Diagnostish.Infrastructure.Providers.Registry.Common;
using Diagnostish.Infrastructure.Providers.Wmi.Common;
using Diagnostish.Infrastructure.Providers.Wmi.Hardware;
using Diagnostish.Infrastructure.Providers.Wmi.OperatingSystem;
using Diagnostish.Infrastructure.Shared.Registry.Executor;
using Diagnostish.Infrastructure.Shared.Wmi.Executor;

using OsEntity = Diagnostish.Domain.Models.Entities.OperatingSystem.OperatingSystem;

namespace Diagnostish.Desktop.Composition;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        return services
            .AddSingleton<IWmiExecutor, WmiExecutor>()
            .AddSingleton<IRegistryExecutor, RegistryExecutor>()
            .AddSingleton<IUserInterface, ConsoleUserInterface>()
            .AddSingleton<FinalReportComposer>()
            .AddSingleton<FinalReportPrintDispatcher>()
            .AddSingleton<DiagnosticController>();
    }

    public static IServiceCollection AddHardwareComponents(this IServiceCollection services)
    {
        services.AddSingleton<IWmiSource<RawGpuModel>, GpuWmiProvider>();
        services.AddSingleton<IRegistrySource<RawGpuModel>, GpuRegistryProvider>();

        return services
            .AddComponent<HardwareReport, RawCpuModel, Cpu, 
                CpuWmiProvider, CpuAnalyzer, CpuReportMapper>()
            .AddComponent<HardwareReport, RawRamModel, Ram, 
                RamWmiProvider, RamAnalyzer, RamReportMapper>()
            .AddComponent<HardwareReport, RawGpuModel, IReadOnlyList<Gpu>, 
                GpuFallBackProvider, GpuAnalyzer, GpuReportMapper>()
            .AddComponent<HardwareReport, RawStorageDriveModel, IReadOnlyList<StorageDrive>, 
                StorageDriveWmiProvider, StorageDriveAnalyzer, StorageDriveReportMapper>()
            .AddComponent<HardwareReport, RawBiosModel, Bios, 
                BiosWmiProvider, BiosAnalyzer, BiosReportMapper>()
            .AddComponent<HardwareReport, RawBaseBoardModel, BaseBoard, 
                BaseBoardWmiProvider, BaseBoardAnalyzer, BaseBoardReportMapper>();
    }

    public static IServiceCollection AddOperatingSystemComponents(this IServiceCollection services)
    {
        return services.AddComponent<OperatingSystemReport, RawOperatingSystemModel, OsEntity,
            OperatingSystemWmiProvider, OperatingSystemAnalyzer, OperatingSystemReportMapper>();
    }

    public static IServiceCollection AddPrinters(this IServiceCollection services)
    {
        return services
            .AddPrinter<HardwareReport, HardwareConsolePrinter>()
            .AddPrinter<OperatingSystemReport, OperatingSystemConsolePrinter>();
    }

    private static IServiceCollection AddComponent<TReport, TRawData, TData, TProvider, TAnalyzer, TMapper>(
        this IServiceCollection services)
        where TProvider : class, IProvider<TRawData>
        where TAnalyzer : class, IAnalyzer<TRawData, TData>
        where TMapper : class, IReportMapper<TReport, TData>
    {
        services.AddSingleton<IProvider<TRawData>, TProvider>();
        services.AddSingleton<IAnalyzer<TRawData, TData>, TAnalyzer>();
        services.AddSingleton<IReportMapper<TReport, TData>, TMapper>();

        services.AddSingleton(sp => new ComponentPipeline<TReport>(async cancellationToken =>
        {
            var provider = sp.GetRequiredService<IProvider<TRawData>>();
            var analyzer = sp.GetRequiredService<IAnalyzer<TRawData, TData>>();
            var mapper = sp.GetRequiredService<IReportMapper<TReport, TData>>();

            var rawData = await provider.ProvideAsync(cancellationToken);
            var result = analyzer.Analyze(rawData);

            return report => mapper.MapInto(report, result);
        }));

        return services;
    }

    private static IServiceCollection AddPrinter<TReport, TPrinter>(this IServiceCollection services) 
        where TPrinter : class, IReportPrinter<TReport>
    {
        services.AddSingleton<IReportPrinter<TReport>, TPrinter>();
        return services;
    }
}