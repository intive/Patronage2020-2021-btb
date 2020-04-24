using BTB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Application.Logs.Commands
{
    public class LoadLogsToDBCommandHandler : IRequestHandler<LoadLogsToDBCommand>
    {
        private readonly IBTBDbContext _context;
        private readonly ILogger _logger;

        public LoadLogsToDBCommandHandler(IBTBDbContext context, ILogger<LoadLogsToDBCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Unit> Handle(LoadLogsToDBCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _context.Logs.AddRangeAsync(request.LogList.ToArray());
                _context.SaveChanges();
            }
            catch (NullReferenceException e)
            {
                _logger.LogError(e, "List of logs is null!");
            }

            return Unit.Value;
        }
    }
}
