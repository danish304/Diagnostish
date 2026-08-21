using Application.Mappers.Hardware;
using Application.Mappers.Network;
using Application.Mappers.OperatingSystem;
using Application.Pipelines;
using Application.Services;
using Desktop.Controllers;
using Desktop.Views.Common.Printers;
using Desktop.Views.ConsoleOutput;
using Desktop.Views.Dispatchers;
using Desktop.Views.FileOutput;
using Domain.Models.Entities.Hardware;
using Domain.Models.Entities.Network;
using Domain.Models.Entities.OperatingSystem;
using Infrastructure.Analyzers.Hardware;
using Infrastructure.Analyzers.Network;
using Infrastructure.Analyzers.OperatingSystem;
using Infrastructure.Providers;
using Infrastructure.Providers.Common.RawModels.Hardware;
using Infrastructure.Providers.Common.RawModels.Network;
using Infrastructure.Providers.Common.RawModels.OperatingSystem;
using Infrastructure.Providers.Registry;
using Infrastructure.Providers.Registry.Common;
using Infrastructure.Providers.Wmi.Common;
using Infrastructure.Providers.Wmi.Hardware;
using Infrastructure.Providers.Wmi.Network;
using Infrastructure.Providers.Wmi.OperatingSystem;
using Infrastructure.Shared.Registry.Executor;
using Infrastructure.Shared.Wmi.Executor;

namespace Desktop.Composition;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        return services
            .AddSingleton<IWmiExecutor, WmiExecutor>()
            .AddSingleton<IRegistryExecutor, RegistryExecutor>()

            .AddSingleton<FinalReportComposer>()
            .AddSingleton<FinalReportPrintDispatcher>()
            .AddSingleton<UserInterfaceDispatcher>()

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
                NetworkAdapterWmiProvider, NetworkAdapterAnalyzer, NetworkAdapterReportMapper>()

            .AddComponent<NetworkReport, IpAddressRawModel, IReadOnlyList<IpAddress>,
                IpAddressWmiProvider, IpAddressAnalyzer, IpAddressReportMapper>()

            .AddComponent<NetworkReport, GatewayRawModel, IReadOnlyList<Gateway>,
                GatewayWmiProvider, GatewayAnalyzer, GatewayReportMapper>()

            .AddComponent<NetworkReport, DnsRawModel, IReadOnlyList<Dns>,
                DnsWmiProvider, DnsAnalyzer, DnsReportMapper>();
    }

    public static IServiceCollection AddConsolePrinters(this IServiceCollection services)
    {
        return services
            .AddSingleton<IReportPrinter<HardwareReport>>(sp =>
                new HardwarePrinter(Console.Out, new ConsoleLineWriter()))

            .AddSingleton<IReportPrinter<OperatingSystemReport>>(sp =>
                new OperatingSystemPrinter(Console.Out, new ConsoleLineWriter()))

            .AddSingleton<IReportPrinter<NetworkReport>>(sp =>
                new NetworkPrinter(Console.Out, new ConsoleLineWriter()));
    }

    public static IServiceCollection AddFilePrinters(this IServiceCollection services)
    {
        services.AddSingleton<CommonReportFile>();

        return services
            .AddSingleton<IReportPrinter<HardwareReport>>(sp =>
                new HardwarePrinter(sp.GetRequiredService<CommonReportFile>().Writer))

            .AddSingleton<IReportPrinter<OperatingSystemReport>>(sp =>
                new OperatingSystemPrinter(sp.GetRequiredService<CommonReportFile>().Writer))

            .AddSingleton<IReportPrinter<NetworkReport>>(sp =>
                new NetworkPrinter(sp.GetRequiredService<CommonReportFile>().Writer));
    }

    public static IServiceCollection AddUserInterfaces(this IServiceCollection services)
    {
        return services
            .AddSingleton<IUserInterface, ConsoleUserInterface>()
            .AddSingleton<IUserInterface, FileUserInterface>();
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
}