using Infrastructure.Shared.Wmi.Executor;
using System.Management;

namespace Infrastructure.Providers.Wmi.Common;

public abstract class BaseWmiProvider<TRawModel>(
    IWmiExecutor executor) : IWmiSource<TRawModel>
{
    public async Task<ProvideResult<IReadOnlyList<TRawModel>>> ProvideAsync(
        CancellationToken cancellationToken = default)
    {
        var rawData = new List<TRawModel>();
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
            ? ProvideResult<IReadOnlyList<TRawModel>>.Ok(rawData, warnings)
            : ProvideResult<IReadOnlyList<TRawModel>>.Fail(warnings, criticalErrors);
    }

    protected abstract string BuildQuery();

    protected abstract string ContextName { get; }

    protected abstract TRawModel Map(ManagementBaseObject item);
}