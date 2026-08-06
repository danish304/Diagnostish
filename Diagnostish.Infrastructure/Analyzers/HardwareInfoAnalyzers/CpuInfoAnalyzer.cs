using Diagnostish.Domain.Models.Entities.Hardware;
using Diagnostish.Infrastructure.Analyzers.Common;
using Diagnostish.Infrastructure.Analyzers.HardwareInfoAnalyzers.Messages;
using Diagnostish.Infrastructure.Providers.HardwareInfoProviders.RawHardwareInfo;

namespace Diagnostish.Infrastructure.Analyzers.HardwareInfoAnalyzers;

public class CpuInfoAnalyzer(Serilog.ILogger logger) : IAnalyzeDiagnosticInfo<RawCpuInfo, CpuInfo>
{
    public ProvideResult<CpuInfo> AnalyzeInfo(ProvideResult<IReadOnlyList<RawCpuInfo>> providedCpuData)
    {
        var warnings = new List<string>(providedCpuData.Warnings);

        if (providedCpuData.Data is not { Count: > 0 } cpuInfo)
            return ProvideResult<CpuInfo>.Fail(warnings, providedCpuData.CriticalErrors);

        var item = cpuInfo[0];

        string name = item.Name.GetValueOrWarning(warnings, logger, CpuAnalyzerMessages.UnknownName);
        int countCores = item.Cores.GetValueOrWarning(warnings, logger, CpuAnalyzerMessages.UnknownCountCores);
        int speed = item.ClockSpeed.GetValueOrWarning(warnings, logger, CpuAnalyzerMessages.UnknownSpeed);

        return ProvideResult<CpuInfo>.Ok(new CpuInfo(name, countCores, speed), warnings);
    }
}