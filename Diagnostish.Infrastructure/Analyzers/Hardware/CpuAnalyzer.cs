using Diagnostish.Domain.Models.Entities.Hardware;
using Diagnostish.Infrastructure.Analyzers.Common;
using Diagnostish.Infrastructure.Providers.Common.RawModels.Hardware;
using Serilog;

using static Diagnostish.Infrastructure.Analyzers.Hardware.Messages.CpuAnalyzerMessages;

namespace Diagnostish.Infrastructure.Analyzers.Hardware;

public class CpuAnalyzer(ILogger logger)
    : IAnalyzer<CpuRawModel, Cpu>
{
    public ProvideResult<Cpu> Analyze(
        ProvideResult<IReadOnlyList<CpuRawModel>> result)
    {
        var warnings = new List<string>(result.Warnings);

        if (result.RawData is not [var rawData, ..])
        {
            return ProvideResult<Cpu>.Fail(
                warnings,
                result.CriticalErrors);
        }

        string name = rawData.Name
            .GetValueOrWarning(warnings, logger, UNKNOWN_NAME);

        int countCores = rawData.Cores
            .GetValueOrWarning(warnings, logger, UNKNOWN_COUNT_CORES);

        int speed = rawData.ClockSpeed
            .GetValueOrWarning(warnings, logger, UNKNOWN_SPEED);

        return ProvideResult<Cpu>.Ok(
            new Cpu(name, countCores, speed),
            warnings);
    }
}