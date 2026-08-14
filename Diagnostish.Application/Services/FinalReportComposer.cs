using Diagnostish.Application.Pipelines;
using Diagnostish.Domain.Models.Reports.Components;

namespace Diagnostish.Application.Services;

public class FinalReportComposer
{
    private readonly IEnumerable<ComponentPipeline<HardwareReport>> _hardwarePipelines;
    private readonly IEnumerable<ComponentPipeline<OperatingSystemReport>> _operatingSystemPipelines;
    private readonly IEnumerable<ComponentPipeline<NetworkReport>> _networkPipelines;

    public FinalReportComposer(
        IEnumerable<ComponentPipeline<HardwareReport>> hardwarePipelines,
        IEnumerable<ComponentPipeline<OperatingSystemReport>> operatingSystemPipelines,
        IEnumerable<ComponentPipeline<NetworkReport>> networkPipelines)
    {
        _hardwarePipelines = hardwarePipelines;
        _operatingSystemPipelines = operatingSystemPipelines;
        _networkPipelines = networkPipelines;
    }

    public async Task<FinalReport> GetFinalReportAsync(
        CancellationToken cancellationToken = default)
    {
        var hardwareTask = CollectReportAsync(
            _hardwarePipelines,
            new HardwareReport(),
            cancellationToken);

        var operatingSystemTask = CollectReportAsync(
            _operatingSystemPipelines,
            new OperatingSystemReport(),
            cancellationToken);

        var networkTask = CollectReportAsync(
            _networkPipelines,
            new NetworkReport(),
            cancellationToken);

        await Task.WhenAll(hardwareTask, operatingSystemTask, networkTask);

        var hardwareReport = await hardwareTask;
        var operatingSystemReport = await operatingSystemTask;
        var networkReport = await networkTask;

        return new FinalReport
        {
            HardwareReport = hardwareReport,
            OperatingSystemReport = operatingSystemReport,
            NetworkReport = networkReport
        };
    }

    private static async Task<TReport> CollectReportAsync<TReport>(
        IEnumerable<ComponentPipeline<TReport>> pipelines,
        TReport report,
        CancellationToken cancellationToken)
    {
        var mapActions = await Task.WhenAll(
            pipelines.Select(p => p.CollectAndAnalyze(cancellationToken))
        );

        foreach (var applyMap in mapActions)
        {
            applyMap(report);
        }

        return report;
    }
}