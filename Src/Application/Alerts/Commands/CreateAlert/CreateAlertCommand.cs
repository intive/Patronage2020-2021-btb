using AutoMapper;
using Binance.Net.Interfaces;
using BTB.Application.Alerts.Common;
using BTB.Application.Common.Exceptions;
using BTB.Application.Common.Interfaces;
using BTB.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Application.Alerts.Commands.CreateAlert
{
    public class CreateAlertCommand : IRequest<AlertVm>
    {
        public string SymbolPair { get; set; }
        public string Condition { get; set; }
        public string ValueType { get; set; }
        public double Value { get; set; }
        public bool SendEmail { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }

        public class CreateAlertCommandHandler : IRequestHandler<CreateAlertCommand, AlertVm>
        {
            private readonly IBTBDbContext _context;
            private readonly IMapper _mapper;
            private readonly IBTBBinanceClient _client;
            private readonly ICurrentUserIdentityService _userIdentity;

            public CreateAlertCommandHandler(IBTBDbContext context, IMapper mapper, IBTBBinanceClient client, ICurrentUserIdentityService userIdentity)
            {
                _context = context;
                _mapper = mapper;
                _client = client;
                _userIdentity = userIdentity;
            }

            public async Task<AlertVm> Handle(CreateAlertCommand request, CancellationToken cancellationToken)
            {
                if (_client.GetSymbolNames(request.SymbolPair) == null)
                {
                    throw new BadRequestException($"Trading pair symbol '{request.SymbolPair}' does not exist.");
                }

                var alert = _mapper.Map<Alert>(request);
                alert.UserId = _userIdentity.UserId;

                await _context.Alerts.AddAsync(alert, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                return _mapper.Map<AlertVm>(alert);
            }
        }
    }
}
