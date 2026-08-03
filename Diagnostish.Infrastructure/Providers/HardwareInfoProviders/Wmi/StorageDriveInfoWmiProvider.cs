using Diagnostish.Infrastructure.Providers.Common;
using Diagnostish.Infrastructure.Providers.HardwareInfoProviders.RawHardwareInfo;
using Diagnostish.Infrastructure.Shared.Utils;
using Diagnostish.Infrastructure.Shared.Wmi.Executor;
using System.Management;

namespace Diagnostish.Infrastructure.Providers.HardwareInfoProviders.Wmi;

public class StorageDriveInfoWmiProvider(IExecutorWmi executor) : BaseWmiProvider<RawStorageDriveInfo>(executor)
{
    private const string MODEL = "Model";
    private const string SIZE = "Size";

    protected override string BuildQuery() => $"SELECT {MODEL}, {SIZE} FROM Win32_DiskDrive";

    protected override string ContextName => "о накопителях";

    protected override RawStorageDriveInfo Map(ManagementBaseObject item)
    {
        return new RawStorageDriveInfo(Parser.ToSafeString(item[MODEL]),
                                       Parser.ToSafeDouble(item[SIZE]));
    }
}