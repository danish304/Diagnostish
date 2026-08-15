using Domain.Models.Entities.Hardware;
using Infrastructure.Analyzers.Common;
using Infrastructure.Providers.Common.RawModels.Hardware;
using Serilog;

using static Infrastructure.Analyzers.Hardware.Messages.BiosAnalyzerMessages;

namespace Infrastructure.Analyzers.Hardware;

public class BiosAnalyzer(ILogger logger)
    : IAnalyzer<BiosRawModel, Bios>
{
    public ProvideResult<Bios> Analyze(
        ProvideResult<IReadOnlyList<BiosRawModel>> result)
    {
        var warnings = new List<string>(result.Warnings);

        if (result.Data is not [var rawData, ..])
        {
            return ProvideResult<Bios>.Fail(
                warnings,
                result.CriticalErrors);
        }

        string version = rawData.Version
            .GetValueOrWarning(warnings, logger, UNKNOWN_VERSION);

        DateTime releaseDate = rawData.ReleaseDate
            .GetValueOrWarning(
                warnings,
                logger,
                UNKNOWN_RELEASE_DATE,
                condition: date => date != DateTime.MinValue && date < DateTime.Now);

        string manufacturer = rawData.Manufacturer
            .GetValueOrWarning(warnings, logger, UNKNOWN_MANUFACTURER);

        return ProvideResult<Bios>.Ok(
            new Bios(version, releaseDate, manufacturer),
            warnings);
    }
}