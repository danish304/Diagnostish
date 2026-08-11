using Diagnostish.Application.Pipelines;
using Diagnostish.Domain.Models.Reports.Components;

namespace Diagnostish.Application.Services;

public class FinalReportComposer
{
    private readonly IEnumerable<ComponentPipeline<HardwareReport>> _hardwarePipelines;
    private readonly IEnumerable<ComponentPipeline<OperatingSystemReport>> _operatingSystemPipelines;

    public FinalReportComposer(
        IEnumerable<ComponentPipeline<HardwareReport>> hardwarePipelines, 
        IEnumerable<ComponentPipeline<OperatingSystemReport>> operatingSystemPipelines)
    {
        _hardwarePipelines = hardwarePipelines;
        _operatingSystemPipelines = operatingSystemPipelines;
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

        await Task.WhenAll(hardwareTask, operatingSystemTask);

        var hardwareReport = await hardwareTask;
        var operatingSystemReport = await operatingSystemTask;

        return new FinalReport
        {
            HardwareReport = hardwareReport,
            OperatingSystemReport = operatingSystemReport
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