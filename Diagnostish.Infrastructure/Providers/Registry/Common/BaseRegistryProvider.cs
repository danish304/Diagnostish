using Diagnostish.Infrastructure.Shared.Registry.Executor;
using Microsoft.Win32;

namespace Diagnostish.Infrastructure.Providers.Registry.Common;

public abstract class BaseRegistryProvider<TRawData>(
    IRegistryExecutor executor) : IRegistrySource<TRawData>
{
    public async Task<ProvideResult<IReadOnlyList<TRawData>>> ProvideAsync(
        CancellationToken cancellationToken = default)
    {
        var rawData = new List<TRawData>();
        var warnings = new List<string>();
        var criticalErrors = new List<string>();

        await executor.ExecuteSafeReadAsync(
            RootPath,
            ContextName,
            warnings,
            criticalErrors,
            rootKey =>
        {
            foreach (var subKeyName in rootKey.GetSubKeyNames())
            {
                if (!IsRelevantSubKey(subKeyName))
                {
                    continue;
                }

                using var subKey = rootKey.OpenSubKey(subKeyName);
                if (subKey is null)
                {
                    continue;
                }

                var mappedItem = Map(subKey);
                if (mappedItem is not null)
                {
                    rawData.Add(mappedItem);
                }
            }
        },
        cancellationToken);

        return rawData.Count > 0
            ? ProvideResult<IReadOnlyList<TRawData>>.Ok(rawData, warnings)
            : ProvideResult<IReadOnlyList<TRawData>>.Fail(warnings, criticalErrors);
    }

    protected abstract string RootPath { get; }

    protected abstract string ContextName { get; }

    protected virtual bool IsRelevantSubKey(string subKeyName) => true;

    protected abstract TRawData? Map(RegistryKey subKey);
}