using Diagnostish.Application.Mappers.Hardware;
using Diagnostish.Application.Mappers.Network;
using Diagnostish.Application.Mappers.OperatingSystem;
using Diagnostish.Application.Pipelines;
using Diagnostish.Application.Services;
using Diagnostish.Desktop.Controllers;
using Diagnostish.Desktop.Views;
using Diagnostish.Desktop.Views.ConsoleViews;
using Diagnostish.Desktop.Views.UserInterfaces;
using Diagnostish.Domain.Models.Entities.Hardware;
using Diagnostish.Domain.Models.Entities.Network;
using Diagnostish.Domain.Models.Entities.OperatingSystem;
using Diagnostish.Infrastructure.Analyzers.Hardware;
using Diagnostish.Infrastructure.Analyzers.Network;
using Diagnostish.Infrastructure.Analyzers.OperatingSystem;
using Diagnostish.Infrastructure.Providers;
using Diagnostish.Infrastructure.Providers.Common.RawModels.Hardware;
using Diagnostish.Infrastructure.Providers.Common.RawModels.Network;
using Diagnostish.Infrastructure.Providers.Common.RawModels.OperatingSystem;
using Diagnostish.Infrastructure.Providers.Registry;
using Diagnostish.Infrastructure.Providers.Registry.Common;
using Diagnostish.Infrastructure.Providers.Wmi.Common;
using Diagnostish.Infrastructure.Providers.Wmi.Hardware;
using Diagnostish.Infrastructure.Providers.Wmi.Network;
using Diagnostish.Infrastructure.Providers.Wmi.OperatingSystem;
using Diagnostish.Infrastructure.Shared.Registry.Executor;
using Diagnostish.Infrastructure.Shared.Wmi.Executor;

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
        services.AddSingleton<IWmiSource<GpuRawModel>, GpuWmiProvider>();
        services.AddSingleton<IRegistrySource<GpuRawModel>, GpuRegistryProvider>();

        return services
            .AddComponent<HardwareReport, CpuRawModel, Cpu,
                CpuWmiProvider, CpuAnalyzer, CpuReportMapper>()

            .AddComponent<HardwareReport, RamRawModel, Ram,
                RamWmiProvider, RamAnalyzer, RamReportMapper>()

            .AddComponent<HardwareReport, GpuRawModel, IReadOnlyList<Gpu>,
                GpuFallBackProvider, GpuAnalyzer, GpuReportMapper>()

            .AddComponent<HardwareReport, StorageDriveRawModel, IReadOnlyList<StorageDrive>,
                StorageDriveWmiProvider, StorageDriveAnalyzer, StorageDriveReportMapper>()

            .AddComponent<HardwareReport, BiosRawModel, Bios,
                BiosWmiProvider, BiosAnalyzer, BiosReportMapper>()

            .AddComponent<HardwareReport, BaseBoardRawModel, BaseBoard,
                BaseBoardWmiProvider, BaseBoardAnalyzer, BaseBoardReportMapper>();
    }

    public static IServiceCollection AddOperatingSystemComponents(this IServiceCollection services)
    {
        return services
            .AddComponent<OperatingSystemReport, OperatingSystemRawModel, OperSystem,
                OperatingSystemWmiProvider, OperatingSystemAnalyzer, OperatingSystemReportMapper>();
    }

    public static IServiceCollection AddNetworkComponents(this IServiceCollection services)
    {
        return services
            .AddComponent<NetworkReport, NetworkAdapterRawModel, IReadOnlyList<NetworkAdapter>,
                NetworkAdatperWmiProvider, NetworkAdapterAnalyzer, NetworkAdapterReportMapper>()

            .AddComponent<NetworkReport, IpAddressRawModel, IReadOnlyList<IpAddress>,
                IpAddressWmiProvider, IpAddressAnalyzer, IpAddressReportMapper>()

            .AddComponent<NetworkReport, GatewayRawModel, IReadOnlyList<Gateway>,
                GatewayWmiProvider, GatewayAnalyzer, GatewayReportMapper>()

            .AddComponent<NetworkReport, DnsRawModel, IReadOnlyList<Dns>,
                DnsWmiProvider, DnsAnalyzer, DnsReportMapper>();

    }

    public static IServiceCollection AddPrinters(this IServiceCollection services)
    {
        return services
            .AddPrinter<HardwareReport, HardwareConsolePrinter>()
            .AddPrinter<OperatingSystemReport, OperatingSystemConsolePrinter>()
            .AddPrinter<NetworkReport, NetworkConsolePrinter>();
    }

    private static IServiceCollection AddComponent<TReport, TRawModel, TData, TProvider, TAnalyzer, TMapper>(
        this IServiceCollection services)
        where TProvider : class, IProvider<TRawModel>
        where TAnalyzer : class, IAnalyzer<TRawModel, TData>
        where TMapper : class, IReportMapper<TReport, TData>
    {
        services.AddSingleton<IProvider<TRawModel>, TProvider>();
        services.AddSingleton<IAnalyzer<TRawModel, TData>, TAnalyzer>();
        services.AddSingleton<IReportMapper<TReport, TData>, TMapper>();

        services.AddSingleton(sp => new ComponentPipeline<TReport>(async cancellationToken =>
        {
            var provider = sp.GetRequiredService<IProvider<TRawModel>>();
            var analyzer = sp.GetRequiredService<IAnalyzer<TRawModel, TData>>();
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