using BTB.Client.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BTB.Client.Services.Implementations
{
    public class RefreshRequestsService : IRefreshRequestsService
    {
        public event Func<Task> OnRefreshRequested;

        public async Task RequestRefreshAsync()
        {
            await OnRefreshRequested?.Invoke();
        }
    }
}
