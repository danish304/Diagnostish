using Infrastructure.Providers.Common.RawModels.Hardware;
using Infrastructure.Providers.Wmi.Common;
using Infrastructure.Shared.Common.Utils;
using Infrastructure.Shared.Wmi.Executor;
using System.Management;

namespace Infrastructure.Providers.Wmi.Hardware;

public class CpuWmiProvider(IWmiExecutor executor)
    : BaseWmiProvider<CpuRawModel>(executor)
{
    private const string CPU_NAME = "Name";
    private const string COUNT_CORES = "NumberOfCores";
    private const string SPEED = "CurrentClockSpeed";

    protected override string BuildQuery() =>
        $"SELECT {CPU_NAME}, {COUNT_CORES}, {SPEED} FROM Win32_Processor";

    protected override string ContextName => "о процессоре";

    protected override CpuRawModel Map(ManagementBaseObject item)
    {
        return new CpuRawModel(
            Parser.ToSafeString(item[CPU_NAME]),
            Parser.ToSafeInt(item[COUNT_CORES]),
            Parser.ToSafeInt(item[SPEED])
        );
    }
}