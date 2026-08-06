using System.Diagnostics.CodeAnalysis;

namespace Diagnostish.Application.Mappers.Common;

public static class MapperExtensions
{
    public static bool TryExtractData<TData>(this IssuesReport report,
                                             ProvideResult<TData> analysisData,
                                             [NotNullWhen(true)] out TData? data) where TData : class
    {
        report.Warnings.AddRange(analysisData.Warnings);
        report.CriticalErrors.AddRange(analysisData.CriticalErrors);

        data = analysisData.Data;
        return data != null;
    }
}