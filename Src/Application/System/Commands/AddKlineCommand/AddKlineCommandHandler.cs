using AutoMapper;
using BTB.Application.Common.Exceptions;
using BTB.Application.Common.Interfaces;
using BTB.Domain.Common;
using BTB.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Application.System.Commands.AddKlineCommand
{
    public class AddKlineCommandHandler : IRequestHandler<AddKlineCommand>
    {
        private readonly IBTBDbContext _context;
        private readonly IBTBBinanceClient _client;
        private readonly IMapper _mapper;

        public AddKlineCommandHandler(IBTBDbContext context, IBTBBinanceClient client, IMapper mapper)
        {
            _context = context;
            _client = client;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(AddKlineCommand request, CancellationToken cancellationToken)
        {
            if (_client.GetSymbolNames(request.SymbolPair) == null)
            {
                throw new BadRequestException($"Trading pair symbol '{request.SymbolPair}' does not exist.");
            }

            var kline = _mapper.Map<Kline>(request);
            kline.OpenTimestamp = DateTimestampConv.GetTimestamp(DateTime.Now);
            kline.DurationTimestamp = TimestampInterval.FiveMin;
            
            SymbolPair pair = await _client.GetSymbolPairByName(request.SymbolPair);
            kline.SymbolPairId = pair.Id;

            await _context.Klines.AddAsync(kline);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
