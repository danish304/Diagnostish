using Diagnostish.Domain.Models.Reports.Common;
using System.Diagnostics.CodeAnalysis;

namespace Diagnostish.Application.Mappers.Common;

public static class MapperExtensions
{
    public static bool TryExtractData<TData>(
        this BaseIssuesReport report,
        ProvideResult<TData> result,
        [NotNullWhen(true)] out TData? data)
        where TData : class
    {
        report.AddWarnings(result.Warnings);
        report.AddCriticalErrors(result.CriticalErrors);

        data = result.Data;
        return data is not null;
    }
}