using Diagnostish.Models;

namespace Diagnostish.Services
{
    public interface IHWCheck
    {
        HWReport CheckPCCFG();
    }
}
