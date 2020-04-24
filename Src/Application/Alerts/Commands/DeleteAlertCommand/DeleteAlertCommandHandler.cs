using BTB.Application.Common.Exceptions;
using BTB.Application.Common.Interfaces;
using BTB.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Application.Alerts.Commands.DeleteAlertCommand
{
    public class DeleteAlertCommandHandler : IRequestHandler<DeleteAlertCommand>
    {
        private readonly IBTBDbContext _context;
        private readonly IUserAccessor _userAccessor;

        public DeleteAlertCommandHandler(IBTBDbContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<Unit> Handle(DeleteAlertCommand request, CancellationToken cancellationToken)
        {
            Alert dbAlert = await _context.Alerts.SingleOrDefaultAsync(a => a.Id == request.Id && a.UserId == _userAccessor.GetCurrentUserId(), cancellationToken);
            
            if (dbAlert == null)
            {
                throw new NotFoundException($"User (id: {_userAccessor.GetCurrentUserId()}) has no alert with id {request.Id}.");
            }

            _context.Alerts.Remove(dbAlert);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
