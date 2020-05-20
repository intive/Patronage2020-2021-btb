using AutoMapper;
using BTB.Application.Common.Exceptions;
using BTB.Application.Common.Interfaces;
using BTB.Domain.Entities;
using BTB.Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Application.Alerts.Commands.CreateAlertCommand
{
    public class CreateAlertCommandHandler : IRequestHandler<CreateAlertCommand, AlertVO>
    {
        private readonly IBTBDbContext _context;
        private readonly IMapper _mapper;
        private readonly IBTBBinanceClient _client;
        private readonly IUserAccessor _userAccessor;

        public CreateAlertCommandHandler(IBTBDbContext context, IMapper mapper, IBTBBinanceClient client, IUserAccessor userAccessor)
        {
            _context = context;
            _mapper = mapper;
            _client = client;
            _userAccessor = userAccessor;
        }

        public async Task<AlertVO> Handle(CreateAlertCommand request, CancellationToken cancellationToken)
        {
            SymbolPairVO names = _client.GetSymbolNames(request.SymbolPair);
            if (names == null)
            {
                throw new BadRequestException($"Trading pair symbol '{request.SymbolPair}' does not exist.");
            }

            var alert = _mapper.Map<Alert>(request);
            alert.UserId = _userAccessor.GetCurrentUserId();
            SymbolPair symbolPair = await _client.GetSymbolPairByName(request.SymbolPair);
            alert.SymbolPairId = symbolPair.Id;
            alert.WasTriggered = false;

            AlertMessageTemplate template = await _context.AlertMessageTemplates.SingleOrDefaultAsync(t => t.Type == alert.Condition);
            alert.MessageTemplateId = template.Id;

            await _context.Alerts.AddAsync(alert, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return _mapper.Map<AlertVO>(alert);
        }
    }
}
