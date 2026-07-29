using Diagnostish.Application.Pipelines;
using Diagnostish.Domain.Models.Reports;

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

    public async Task<FinalReport> GetFinalReportAsync(CancellationToken cancellationToken = default)
    {
        var hardwareReportTask = CollectReportAsync(_hardwarePipelines, new HardwareReport(), cancellationToken);
        var operatingSystemReportTask = CollectReportAsync(_operatingSystemPipelines, new OperatingSystemReport(), cancellationToken);

        await Task.WhenAll(hardwareReportTask, operatingSystemReportTask);

        return new FinalReport
        {
            HardwareReport = hardwareReportTask.Result,
            OperatingSystemReport = operatingSystemReportTask.Result
        };
    }

    private static async Task<TReport> CollectReportAsync<TReport>(IEnumerable<ComponentPipeline<TReport>> pipelines, 
                                                                   TReport report, 
                                                                   CancellationToken cancellationToken) where TReport : new()
    {
        var mapActions = await Task.WhenAll(pipelines.Select(p => p.CollectAndAnalyze(cancellationToken)));

        foreach (var applyMap in mapActions) applyMap(report);

        return report;
    }
}