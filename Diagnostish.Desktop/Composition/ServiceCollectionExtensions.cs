using Diagnostish.Application.Pipelines;
using Diagnostish.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Diagnostish.Desktop.Composition;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddComponent<TReport,
                                                  TRaw, TInfo,
                                                  TProvider, TAnalyzer, TMapper>(this IServiceCollection services)

                                                  where TProvider : class, IProvideDiagnosticInfo<TRaw>
                                                  where TAnalyzer : class, IAnalyzeDiagnosticInfo<TRaw, TInfo>
                                                  where TMapper : class, IReportMapper<TReport, TInfo>
    {
        services.AddSingleton<IProvideDiagnosticInfo<TRaw>, TProvider>();
        services.AddSingleton<IAnalyzeDiagnosticInfo<TRaw, TInfo>, TAnalyzer>();
        services.AddSingleton<IReportMapper<TReport, TInfo>, TMapper>();

        services.AddSingleton(sp => new ComponentPipeline<TReport>(report =>
        {
            var provider = sp.GetRequiredService<IProvideDiagnosticInfo<TRaw>>();
            var analyzer = sp.GetRequiredService<IAnalyzeDiagnosticInfo<TRaw, TInfo>>();
            var mapper = sp.GetRequiredService<IReportMapper<TReport, TInfo>>();

            mapper.MapInto(report, analyzer.AnalyzeInfo(provider.ProvideInfo()));
        }));

        return services;
    }

    public static IServiceCollection AddPrinter<TReport, 
                                                TPrinter>(this IServiceCollection services) 
        
                                                where TPrinter : class, IReportPrinter<TReport>
    {
        services.AddSingleton<IReportPrinter<TReport>, TPrinter>();
        return services;
    }
}