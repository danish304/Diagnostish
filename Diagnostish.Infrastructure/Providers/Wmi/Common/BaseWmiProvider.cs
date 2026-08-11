using Diagnostish.Infrastructure.Shared.Wmi.Executor;
using System.Management;

namespace Diagnostish.Infrastructure.Providers.Wmi.Common;

public abstract class BaseWmiProvider<TRawData>(
    IWmiExecutor executor) : IWmiSource<TRawData>
{
    public async Task<ProvideResult<IReadOnlyList<TRawData>>> ProvideAsync(
        CancellationToken cancellationToken = default)
    {
        var rawData = new List<TRawData>();
        var warnings = new List<string>();
        var criticalErrors = new List<string>();

        await executor.ExecuteSafeQueryAsync(
            BuildQuery(),
            ContextName,
            warnings,
            criticalErrors,
            collection =>
        {
            foreach (var item in collection)
            {
                using (item)
                {
                    var mappedItem = Map(item);
                    rawData.Add(mappedItem);
                }
            }
        },
        cancellationToken);

        return rawData.Count > 0
            ? ProvideResult<IReadOnlyList<TRawData>>.Ok(rawData, warnings)
            : ProvideResult<IReadOnlyList<TRawData>>.Fail(warnings, criticalErrors);
    }

    protected abstract string BuildQuery();

    protected abstract string ContextName { get; }

    protected abstract TRawData Map(ManagementBaseObject item);
}