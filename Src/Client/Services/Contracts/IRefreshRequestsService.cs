using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BTB.Client.Services.Contracts
{
    public interface IRefreshRequestsService
    {
        event Func<Task> OnRefreshRequested;
        Task RequestRefreshAsync();
    }
}
