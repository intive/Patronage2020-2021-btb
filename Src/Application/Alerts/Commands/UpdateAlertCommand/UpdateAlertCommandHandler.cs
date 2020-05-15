using AutoMapper;
using BTB.Application.Common.Exceptions;
using BTB.Application.Common.Interfaces;
using BTB.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Application.Alerts.Commands.UpdateAlertCommand
{
    public class UpdateAlertCommandHandler : IRequestHandler<UpdateAlertCommand>
    {
        private readonly IBTBDbContext _context;
        private readonly IMapper _mapper;
        private readonly IBTBBinanceClient _client;
        private readonly IUserAccessor _userAccessor;

        public UpdateAlertCommandHandler(IBTBDbContext context, IMapper mapper, IBTBBinanceClient client, IUserAccessor userAccessor)
        {
            _context = context;
            _mapper = mapper;
            _client = client;
            _userAccessor = userAccessor;
        }

        public async Task<Unit> Handle(UpdateAlertCommand request, CancellationToken cancellationToken)
        {
            if (_client.GetSymbolNames(request.SymbolPair) == null)
            {
                throw new BadRequestException($"Trading pair symbol '{request.SymbolPair}' does not exist.");
            }

            Alert dbAlert = await _context.Alerts.SingleOrDefaultAsync(a => a.Id == request.Id && a.UserId == _userAccessor.GetCurrentUserId(), cancellationToken);
            
            if (dbAlert == null)
            {
                throw new NotFoundException($"User (id: {_userAccessor.GetCurrentUserId()}) has no alert with id {request.Id}.");
            }

            _mapper.Map(request, dbAlert);
            SymbolPair symbolPair = await _client.GetSymbolPairByName(request.SymbolPair);
            dbAlert.SymbolPairId = symbolPair.Id;

            AlertMessageTemplate template = await _context.AlertMessageTemplates.SingleOrDefaultAsync(t => t.Type == dbAlert.Condition);
            dbAlert.MessageTemplateId = template.Id;

            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
