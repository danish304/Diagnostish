using Infrastructure.Shared.Registry.Executor;
using Microsoft.Win32;

namespace Infrastructure.Providers.Registry.Common;

public abstract class BaseRegistryProvider<TRawModel>(
    IRegistryExecutor executor) : IRegistrySource<TRawModel>
{
    public async Task<ProvideResult<IReadOnlyList<TRawModel>>> ProvideAsync(
        CancellationToken cancellationToken = default)
    {
        var rawData = new List<TRawModel>();
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
            ? ProvideResult<IReadOnlyList<TRawModel>>.Ok(rawData, warnings)
            : ProvideResult<IReadOnlyList<TRawModel>>.Fail(warnings, criticalErrors);
    }

    protected abstract string RootPath { get; }

    protected abstract string ContextName { get; }

    protected virtual bool IsRelevantSubKey(string subKeyName) => true;

    protected abstract TRawModel? Map(RegistryKey subKey);
}