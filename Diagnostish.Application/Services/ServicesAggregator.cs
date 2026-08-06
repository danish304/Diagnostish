using Diagnostish.Application.Pipelines;

namespace Diagnostish.Application.Services;

public class ServicesAggregator
{
    private readonly IEnumerable<ComponentPipeline<HardwareReport>> _hardwarePipelines;
    private readonly IEnumerable<ComponentPipeline<OperatingSystemReport>> _operatingSystemPipelines;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0290:Use primary constructor", Justification = "<Pending>")]
    public ServicesAggregator(IEnumerable<ComponentPipeline<HardwareReport>> hardwarePipelines,
                              IEnumerable<ComponentPipeline<OperatingSystemReport>> operatingSystemPipelines)
    {
        _hardwarePipelines = hardwarePipelines;
        _operatingSystemPipelines = operatingSystemPipelines;
    }

    public event Action<string>? ComponentsCollected;

    public async Task<FinalReport> GetFinalReportAsync(CancellationToken cancellationToken = default)
    {
        var hardwareReportTask = CollectReportAsync(_hardwarePipelines, new HardwareReport(), cancellationToken);
        var operatingSystemReportTask = CollectReportAsync(_operatingSystemPipelines, new OperatingSystemReport(), cancellationToken);

        await Task.WhenAll(hardwareReportTask, operatingSystemReportTask);

        var hardwareReport = await hardwareReportTask;
        var operatingSystemReport = await operatingSystemReportTask;

        return new FinalReport
        {
            HardwareReport = hardwareReport,
            OperatingSystemReport = operatingSystemReport
        };
    }

    private static async Task<TReport> CollectReportAsync<TReport>(IEnumerable<ComponentPipeline<TReport>> pipelines, 
                                                                   TReport report, 
                                                                   CancellationToken cancellationToken)
    {
        var mapActions = await Task.WhenAll(pipelines.Select(p => p.CollectAndAnalyze(cancellationToken)));

        foreach (var applyMap in mapActions) applyMap(report);

        return report;
    }
}