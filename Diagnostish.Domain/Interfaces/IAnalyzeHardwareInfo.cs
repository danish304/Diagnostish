namespace Diagnostish.Domain.Interfaces
{
    public interface IAnalyzeHardwareInfo<TInfo, TReport>
    {
        void AnalyzeInfo(IEnumerable<TInfo> ifno, TReport report);
    }
}
