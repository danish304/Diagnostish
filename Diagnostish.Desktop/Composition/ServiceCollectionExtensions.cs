using Diagnostish.Application.Mappers.HardwareMappers;
using Diagnostish.Application.Mappers.OperatingSystemMappers;
using Diagnostish.Application.Pipelines;
using Diagnostish.Application.Services;
using Diagnostish.Desktop.Controllers;
using Diagnostish.Desktop.Views;
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
using Diagnostish.Infrastructure.Providers.HardwareInfoProviders.Registry;
using Diagnostish.Infrastructure.Providers.HardwareInfoProviders.Wmi;
using Diagnostish.Infrastructure.Providers.OperatingSystemInfoProviders;
using Diagnostish.Infrastructure.Shared.Registry.Executor;
using Diagnostish.Infrastructure.Shared.Wmi.Executor;
using Microsoft.Extensions.DependencyInjection;

namespace Diagnostish.Desktop.Composition;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        return services
            .AddSingleton<IExecutorWmi, ExecutorWmi>()
            .AddSingleton<IExecutorRegistry, ExecutorRegistry>()
            .AddSingleton<IUserInterface, ConsoleUserInterface>()
            .AddSingleton<ServicesAggregator>()
            .AddSingleton<PrintersAggregator>()
            .AddSingleton<DiagnosticController>();
    }

    public static IServiceCollection AddHardwareComponents(this IServiceCollection services)
    {
        services.AddSingleton<GpuInfoWmiProvider>();
        services.AddSingleton<GpuInfoRegistryProvider>();

        return services
            .AddComponent<HardwareReport, RawCpuInfo, CpuInfo, 
                          CpuInfoWmiProvider, CpuInfoAnalyzer, CpuReportMapper>()
            .AddComponent<HardwareReport, RawRamInfo, RamInfo, 
                          RamInfoWmiProvider, RamInfoAnalyzer, RamReportMapper>()
            .AddComponent<HardwareReport, RawGpuInfo, IReadOnlyList<GpuInfo>, 
                          GpuInfoProvider, GpuInfoAnalyzer, GpuReportMapper>()
            .AddComponent<HardwareReport, RawStorageDriveInfo, IReadOnlyList<StorageDriveInfo>, 
                          StorageDriveInfoWmiProvider, StorageDriveInfoAnalyzer, StorageDriveReportMapper>()
            .AddComponent<HardwareReport, RawBiosInfo, BiosInfo, 
                          BiosInfoWmiProvider, BiosInfoAnalyzer, BiosReportMapper>()
            .AddComponent<HardwareReport, RawBaseBoardInfo, BaseBoardInfo, 
                          BaseBoardInfoWmiProvider, BaseBoardInfoAnalyzer, BaseBoardReportMapper>();
    }

    public static IServiceCollection AddOperatingSystemComponents(this IServiceCollection services)
    {
        return services
            .AddComponent<OperatingSystemReport, RawOperatingSystemInfo, OperatingSystemInfo,
                                     OperatingSystemInfoWmiProvider, OperatingSystemInfoAnalyzer, OperatingSystemReportMapper>();
    }

    public static IServiceCollection AddPrinters(this IServiceCollection services)
    {
        return services
            .AddPrinter<HardwareReport, HardwareInfoPrintToConsole>()
            .AddPrinter<OperatingSystemReport, OperatingSystemInfoPrintToConsole>();
    }

    private static IServiceCollection AddComponent<TReport, TRaw, TInfo,
                                                  TProvider, TAnalyzer, TMapper>(this IServiceCollection services)

                                                  where TProvider : class, IProvideDiagnosticInfo<TRaw>
                                                  where TAnalyzer : class, IAnalyzeDiagnosticInfo<TRaw, TInfo>
                                                  where TMapper : class, IReportMapper<TReport, TInfo>
    {
        services.AddSingleton<IProvideDiagnosticInfo<TRaw>, TProvider>();
        services.AddSingleton<IAnalyzeDiagnosticInfo<TRaw, TInfo>, TAnalyzer>();
        services.AddSingleton<IReportMapper<TReport, TInfo>, TMapper>();

        services.AddSingleton(sp => new ComponentPipeline<TReport>(async cancellationToken =>
        {
            var provider = sp.GetRequiredService<IProvideDiagnosticInfo<TRaw>>();
            var analyzer = sp.GetRequiredService<IAnalyzeDiagnosticInfo<TRaw, TInfo>>();
            var mapper = sp.GetRequiredService<IReportMapper<TReport, TInfo>>();

            var rawInfo = await provider.ProvideInfoAsync(cancellationToken);
            var analyzedInfo = analyzer.AnalyzeInfo(rawInfo);

            return report => mapper.MapInto(report, analyzedInfo);
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