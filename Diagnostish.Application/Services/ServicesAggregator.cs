using Diagnostish.Application.Pipelines;
using Diagnostish.Domain.Models.Reports;

namespace Diagnostish.Application.Services;

public class ServicesAggregator
{
    private readonly IEnumerable<ComponentPipeline<HardwareReport>> _hardwarePipelines;
    private readonly IEnumerable<ComponentPipeline<OperatingSystemReport>> _operatingSystemPipeline;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0290:Use primary constructor", Justification = "<Pending>")]
    public ServicesAggregator(IEnumerable<ComponentPipeline<HardwareReport>> hardwarePipelines,
                              IEnumerable<ComponentPipeline<OperatingSystemReport>> operatingSystemPipeline)
    {
        _hardwarePipelines = hardwarePipelines;
        _operatingSystemPipeline = operatingSystemPipeline;
    }

    public FinalReport GetFinalReport()
    {
        var hardwareReport = new HardwareReport();
        foreach (var pipeline in _hardwarePipelines) pipeline.Run(hardwareReport);

        var operatingSystemReport = new OperatingSystemReport();
        foreach (var pipeline in _operatingSystemPipeline) pipeline.Run(operatingSystemReport);

        return new FinalReport 
        { 
            HardwareReport = hardwareReport,
            OperatingSystemReport = operatingSystemReport
        };
    }
}