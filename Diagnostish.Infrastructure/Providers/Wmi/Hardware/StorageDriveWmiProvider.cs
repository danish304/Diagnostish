using Diagnostish.Infrastructure.Providers.Common.RawModels.Hardware;
using Diagnostish.Infrastructure.Providers.Wmi.Common;
using Diagnostish.Infrastructure.Shared.Common.Utils;
using Diagnostish.Infrastructure.Shared.Wmi.Executor;
using System.Management;

namespace Diagnostish.Infrastructure.Providers.Wmi.Hardware;

public class StorageDriveWmiProvider(IWmiExecutor executor)
    : BaseWmiProvider<RawStorageDriveModel>(executor)
{
    private const string MODEL = "Model";
    private const string SIZE = "Size";

    protected override string BuildQuery()
        => $"SELECT {MODEL}, {SIZE} FROM Win32_DiskDrive";

    protected override string ContextName => "о накопителях";

    protected override RawStorageDriveModel Map(ManagementBaseObject item)
    {
        return new RawStorageDriveModel(
            Parser.ToSafeString(item[MODEL]),
            Parser.ToSafeDouble(item[SIZE])
        );
    }
}