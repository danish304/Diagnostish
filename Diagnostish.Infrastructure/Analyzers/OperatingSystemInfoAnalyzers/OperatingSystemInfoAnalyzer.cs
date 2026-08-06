using Diagnostish.Domain.Models.Entities;
using Diagnostish.Infrastructure.Analyzers.Common;
using Diagnostish.Infrastructure.Providers.OperatingSystemInfoProviders;

namespace Diagnostish.Infrastructure.Analyzers.OperatingSystemInfoAnalyzers;

public class OperatingSystemInfoAnalyzer(Serilog.ILogger logger) : IAnalyzeDiagnosticInfo<RawOperatingSystemInfo, OperatingSystemInfo>
{
    public ProvideResult<OperatingSystemInfo> AnalyzeInfo(ProvideResult<IReadOnlyList<RawOperatingSystemInfo>> providedOperatingSystemData)
    {
        var warnings = new List<string>(providedOperatingSystemData.Warnings);

        if (providedOperatingSystemData.Data is not { Count: > 0 } operatingSystemInfo)
            return ProvideResult<OperatingSystemInfo>.Fail(warnings, providedOperatingSystemData.CriticalErrors);

        var item = operatingSystemInfo[0];

        string caption = item.Caption.GetValueOrWarning(warnings, logger, OperatingSystemAnalyzerMessages.UnknownCaption);
        string manufacturer = item.Manufacturer.GetValueOrWarning(warnings, logger, OperatingSystemAnalyzerMessages.UnknownManufacturer);
        string version = item.Version.GetValueOrWarning(warnings, logger, OperatingSystemAnalyzerMessages.UnknownVersion);

        DateTime installDate = item.InstallDate.GetValueOrWarning(warnings, logger,
                                                                  OperatingSystemAnalyzerMessages.UnknownInstallDate,
                                                                  condition: date => date != DateTime.MinValue && date < DateTime.Now);

        string user = item.User.GetValueOrWarning(warnings, logger, OperatingSystemAnalyzerMessages.UnknownUser);

        DateTime lastBoot = item.LastBoot.GetValueOrWarning(warnings, logger,
                                                            OperatingSystemAnalyzerMessages.UnknownLastBootDate,
                                                            condition: date => date != DateTime.MinValue && date < DateTime.Now);

        return ProvideResult<OperatingSystemInfo>.Ok(new OperatingSystemInfo(caption, manufacturer, version, installDate, user, lastBoot), warnings);
    }
}