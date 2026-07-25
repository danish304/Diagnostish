using Diagnostish.Domain.Common;
using Diagnostish.Domain.Interfaces;
using Diagnostish.Infrastructure.Shared.Wmi.Executor;
using System.Management;

namespace Diagnostish.Infrastructure.Providers.Common;

public abstract class BaseWmiProvider<TRawInfo>(IExecutorWmi executor) : IProvideDiagnosticInfo<TRawInfo>
{
    public ProvideResult<IReadOnlyList<TRawInfo>> ProvideInfo()
    {
        var rawInfo = new List<TRawInfo>();
        var warnings = new List<string>();
        var criticalErrors = new List<string>();

        executor.ExecuteSafeQuery(BuildQuery(), ContextName, warnings, criticalErrors, collection =>
        {
            foreach (var item in collection)
            {
                using (item)
                {
                    var mappedItem = Map(item);
                    rawInfo.Add(mappedItem);
                }
            }
        });

        return rawInfo.Count > 0
            ? ProvideResult<IReadOnlyList<TRawInfo>>.Ok(rawInfo, warnings)
            : ProvideResult<IReadOnlyList<TRawInfo>>.Fail(warnings, criticalErrors);
    }

    protected abstract string BuildQuery();

    protected abstract string ContextName { get; }

    protected abstract TRawInfo Map(ManagementBaseObject item);
}