using AutoMapper;
using Binance.Net.Interfaces;
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

namespace BTB.Application.Alerts.Commands.UpdateAlertCommand
{
    public class UpdateAlertCommand : IRequest
    {
        public int Id { get; set; }
        public string SymbolPair { get; set; }
        public string Condition { get; set; }
        public string ValueType { get; set; }
        public double Value { get; set; }
        public bool SendEmail { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }

        public class UpdateAlertCommandHandler : IRequestHandler<UpdateAlertCommand>
        {
            private readonly IBTBDbContext _context;
            private readonly IMapper _mapper;
            private readonly IBTBBinanceClient _client;
            private readonly ICurrentUserIdentityService _userIdentity;

            public UpdateAlertCommandHandler(IBTBDbContext context, IMapper mapper, IBTBBinanceClient client, ICurrentUserIdentityService userIdentity)
            {
                _context = context;
                _mapper = mapper;
                _client = client;
                _userIdentity = userIdentity;
            }

            public async Task<Unit> Handle(UpdateAlertCommand request, CancellationToken cancellationToken)
            {
                if (_client.GetSymbolNames(request.SymbolPair) == null)
                {
                    throw new BadRequestException($"Trading pair symbol '{request.SymbolPair}' does not exist.");
                }

                Alert dbAlert = await _context.Alerts.SingleOrDefaultAsync(a => a.Id == request.Id && a.UserId == _userIdentity.UserId, cancellationToken);
                if (dbAlert == null)
                {
                    throw new NotFoundException($"User (id: {_userIdentity.UserId}) has no alert with id {request.Id}.");
                }

                _mapper.Map(request, dbAlert);
                await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}
