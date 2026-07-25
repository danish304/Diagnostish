using Diagnostish.Domain.Common;
using Diagnostish.Domain.Interfaces;
using Diagnostish.Domain.Models.Entities.Hardware;
using Diagnostish.Infrastructure.Analyzers.Common;
using Diagnostish.Infrastructure.Analyzers.HardwareInfoAnalyzers.Messages;
using Diagnostish.Infrastructure.Providers.HardwareInfoProviders.RawHardwareInfo;

namespace Diagnostish.Infrastructure.Analyzers.HardwareInfoAnalyzers;

public class BiosInfoAnalyzer(Serilog.ILogger logger) : IAnalyzeDiagnosticInfo<RawBiosInfo, BiosInfo>
{
    public ProvideResult<BiosInfo> AnalyzeInfo(ProvideResult<IReadOnlyList<RawBiosInfo>> providedBiosData)
    {
        var warnings = new List<string>(providedBiosData.Warnings);

        if (providedBiosData.Data is not { Count: > 0 } biosInfo) 
            return ProvideResult<BiosInfo>.Fail(warnings, providedBiosData.CriticalErrors);

        var item = biosInfo[0];

        string version = item.Version.GetValueOrWarning(warnings, logger, BiosAnalyzerMessages.UnknownVersion);

        DateTime releaseDate = item.ReleaseDate.GetValueOrWarning(warnings, logger,
                                                                  BiosAnalyzerMessages.UnknownReleaseDate,
                                                                  condition: date => date != DateTime.MinValue && date < DateTime.Now);

        string manufacturer = item.Manufacturer.GetValueOrWarning(warnings, logger, BiosAnalyzerMessages.UnknownManufacturer);

        return ProvideResult<BiosInfo>.Ok(new BiosInfo(version, releaseDate, manufacturer), warnings);
    }
}