using System.Threading;
using System.Threading.Tasks;

namespace BTB.Application.Common.Interfaces
{
    public interface IEmailKeeper
    {
        Task IncrementEmailSentAsync(CancellationToken cancellationtoken);
        bool CheckIfLimitHasBeenReached();
    }
}
