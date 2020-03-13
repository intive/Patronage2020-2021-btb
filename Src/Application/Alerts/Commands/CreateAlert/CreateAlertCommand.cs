using AutoMapper;
using BTB.Application.Common.Interfaces;
using BTB.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Application.Alerts.Commands.CreateAlert
{
    public class CreateAlertCommand : IRequest
    {
        public string Symbol { get; set; }
        public string Condition { get; set; }
        public string ValueType { get; set; }
        public double Value { get; set; }
        public bool SendEmail { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public class CreateAlertCommandHandler : IRequestHandler<CreateAlertCommand>
        {
            private readonly IBTBDbContext _context;
            private readonly IMapper _mapper;
            private readonly ICurrentUserIdentityService _userIdentity;

            public CreateAlertCommandHandler(IBTBDbContext context, IMapper mapper, ICurrentUserIdentityService userIdentity)
            {
                _context = context;
                _mapper = mapper;
                _userIdentity = userIdentity;
            }

            public async Task<Unit> Handle(CreateAlertCommand request, CancellationToken cancellationToken)
            {
                var alert = _mapper.Map<CreateAlertCommand, Alert>(request);

                var userId = _userIdentity.UserId;
                alert.UserId = userId;

                _context.Alerts.Add(alert);

                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }

    }
}
