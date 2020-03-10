using AutoMapper;
using BTB.Application.Common.Interfaces;
using BTB.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Application.Alerts.Commands.CreateAlert
{
    public class CreateAlertCommand : IRequest<string>
    {
        public string Symbol { get; set; }
        public string Condition { get; set; }
        public string ValueType { get; set; }
        public double Value { get; set; }
        public bool SendEmail { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public class CreateAlertCommandHandler : IRequestHandler<CreateAlertCommand, string>
        {
            private readonly IBTBDbContext _context;
            private readonly IMapper _mapper;

            public CreateAlertCommandHandler(IBTBDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<string> Handle(CreateAlertCommand request, CancellationToken cancellationToken)
            {
                var alert = _mapper.Map<CreateAlertCommand, Alert>(request);

                _context.Alerts.Add(alert);

                await _context.SaveChangesAsync(cancellationToken);

                return await Task.Run(() => request.Symbol);
            }
        }

    }
}
