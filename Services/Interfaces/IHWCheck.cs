using Diagnostish.Models;

namespace Diagnostish.Services.Interfaces
{
    public interface IHWCheck
    {
        HWReport CheckPCCFG();
    }
}
