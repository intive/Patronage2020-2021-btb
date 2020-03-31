using AutoMapper;
using BTB.Application.Common.Exceptions;
using BTB.Application.Common.Interfaces;
using BTB.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Application.Alerts.Commands.DeleteAlertCommand
{
    public class DeleteAlertCommand : IRequest
    {
        public int Id { get; set; }

        public class DeleteAlertCommandHandler : IRequestHandler<DeleteAlertCommand>
        {
            private readonly IBTBDbContext _context;
            private readonly ICurrentUserIdentityService _userIdentity;

            public DeleteAlertCommandHandler(IBTBDbContext context, ICurrentUserIdentityService userIdentity)
            {
                _context = context;
                _userIdentity = userIdentity;
            }

            public async Task<Unit> Handle(DeleteAlertCommand request, CancellationToken cancellationToken)
            {
                Alert dbAlert = await _context.Alerts.SingleOrDefaultAsync(a => a.Id == request.Id && a.UserId == _userIdentity.UserId, cancellationToken);
                if (dbAlert == null)
                {
                    throw new NotFoundException($"User (id: {_userIdentity.UserId}) has no alert with id {request.Id}.");
                }

                _context.Alerts.Remove(dbAlert);
                await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}
