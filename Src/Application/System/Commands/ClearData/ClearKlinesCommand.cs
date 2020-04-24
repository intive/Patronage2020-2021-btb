using Binance.Net.Objects;
using BTB.Application.Common.Interfaces;
using BTB.Application.Common.Models;
using BTB.Domain.Common;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Application.System.Commands.ClearData
{
    public class ClearKlinesCommand : IRequest<int>
    {
        public TimestampInterval KlineType { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime StopTime { get; set; }

        public class ClearKlinesCommandHandler : IRequestHandler<ClearKlinesCommand, int>
        {
            private readonly IBTBDbContext _context;
            private readonly ILogger _logger;

            public ClearKlinesCommandHandler(IBTBDbContext context, ILogger<ClearKlinesCommandHandler> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<int> Handle(ClearKlinesCommand request, CancellationToken cancellationToken)
            {
                _logger.LogInformation($"All {request.KlineType} klines from {request.StartTime} to {request.StopTime} are going to be removed from database.");
                return await _context.ClearKlinesAsync(request.StartTime, request.StopTime, request.KlineType);
            }
        }
    }
}
