using AutoMapper;
using BTB.Application.Common.Exceptions;
using BTB.Application.Common.Interfaces;
using BTB.Domain.Common;
using BTB.Domain.Entities;
using BTB.Domain.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Application.System.Queries.GetSymbolPairLastKlineQuery
{
    public class GetSymbolPairLastKlineQueryHandler : IRequestHandler<GetSymbolPairLastKlineQuery, KlineVO>
    {
        private readonly IBTBBinanceClient _client;
        private readonly IMapper _mapper;

        public GetSymbolPairLastKlineQueryHandler(IBTBBinanceClient client, IMapper mapper)
        {
            _client = client;
            _mapper = mapper;
        }

        public async Task<KlineVO> Handle(GetSymbolPairLastKlineQuery request, CancellationToken cancellationToken)
        {
            SymbolPair symbolPair = await _client.GetSymbolPairByName(request.SymbolName);
            if (symbolPair == null)
            {
                throw new BadRequestException($"Trading pair symbol '{request.SymbolName}' does not exist.");
            }

            var kline = _mapper.Map<KlineVO>(await _client.GetLastKlineBySymboPair(symbolPair, TimestampInterval.FiveMin));
            return kline;
        }
    }
}
