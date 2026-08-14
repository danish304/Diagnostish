using Diagnostish.Infrastructure.Providers.Common.RawModels.Hardware;
using Diagnostish.Infrastructure.Providers.Wmi.Common;
using Diagnostish.Infrastructure.Shared.Common.Utils;
using Diagnostish.Infrastructure.Shared.Wmi.Executor;
using System.Management;

namespace Diagnostish.Infrastructure.Providers.Wmi.Hardware;

public class StorageDriveWmiProvider(IWmiExecutor executor)
    : BaseWmiProvider<StorageDriveRawModel>(executor)
{
    private const string MODEL = "Model";
    private const string SIZE = "Size";
    private const string STATUS = "Status";

    protected override string BuildQuery()
        => $"SELECT {MODEL}, {SIZE}, {STATUS} FROM Win32_DiskDrive";

    protected override string ContextName => "о накопителях";

    protected override StorageDriveRawModel Map(ManagementBaseObject item)
    {
        return new StorageDriveRawModel(
            Parser.ToSafeString(item[MODEL]),
            Parser.ToSafeDouble(item[SIZE]),
            Parser.ToSafeString(item[STATUS])
        );
    }
}