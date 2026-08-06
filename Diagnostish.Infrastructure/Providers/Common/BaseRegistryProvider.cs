using Diagnostish.Domain.Common;
using Diagnostish.Domain.Interfaces;
using Diagnostish.Infrastructure.Shared.Registry.Executor;
using Microsoft.Win32;

namespace Diagnostish.Infrastructure.Providers.Common;

public abstract class BaseRegistryProvider<TRawInfo>(IExecutorRegistry executor) : IProvideDiagnosticInfo<TRawInfo>
{
    public async Task<ProvideResult<IReadOnlyList<TRawInfo>>> ProvideInfoAsync(CancellationToken cancellationToken = default)
    {
        var rawInfo = new List<TRawInfo>();
        var warnings = new List<string>();
        var criticalErrors = new List<string>();

        await executor.ExecuteSafeReadAsync(RootPath, ContextName, warnings, criticalErrors, rootKey =>
        {
            foreach (var subKeyName in rootKey.GetSubKeyNames())
            {
                if (!IsRelevantSubKey(subKeyName)) continue;

                using var subKey = rootKey.OpenSubKey(subKeyName);
                if (subKey is null) continue;

                var mappedItem = Map(subKey);
                if (mappedItem is not null) rawInfo.Add(mappedItem);
            }
        }, cancellationToken);

        return rawInfo.Count > 0
            ? ProvideResult<IReadOnlyList<TRawInfo>>.Ok(rawInfo, warnings)
            : ProvideResult<IReadOnlyList<TRawInfo>>.Fail(warnings, criticalErrors);
    }

    protected abstract string RootPath { get; }

    protected abstract string ContextName { get; }

    protected virtual bool IsRelevantSubKey(string subKeyName) => true;

    protected abstract TRawInfo? Map(RegistryKey subKey);
}