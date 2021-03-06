﻿using AutoMapper;
using BTB.Application.Common.Exceptions;
using BTB.Application.Common.Interfaces;
using BTB.Common;
using BTB.Domain.Common;
using BTB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
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
        private ILogger _logger;
        private readonly IDateTime _dateTime;

        public AddKlineCommandHandler(IBTBDbContext context, IBTBBinanceClient client, IMapper mapper, ILogger<AddKlineCommandHandler> logger, IDateTime dateTime)
        {
            _context = context;
            _client = client;
            _mapper = mapper;
            _logger = logger;
            _dateTime = dateTime;
        }

        public async Task<Unit> Handle(AddKlineCommand request, CancellationToken cancellationToken)
        {
            if (_client.GetSymbolNames(request.SymbolPair) == null)
            {
                var e = new BadRequestException($"Trading pair symbol '{request.SymbolPair}' does not exist.");
                _logger.LogError(e, "Wrong request argument.");
                throw e;
            }

            var secondLastKline = _mapper.Map<Kline>(request);
            var lastKline = _mapper.Map<Kline>(request);
            long now = DateTimestampConv.GetTimestamp(_dateTime.Now);
            secondLastKline.OpenTimestamp = now;
            lastKline.OpenTimestamp = now + 1;

            secondLastKline.DurationTimestamp = TimestampInterval.FiveMin;
            lastKline.DurationTimestamp = TimestampInterval.FiveMin;
            
            SymbolPair pair = await _client.GetSymbolPairByName(request.SymbolPair);
            secondLastKline.SymbolPairId = pair.Id;
            lastKline.SymbolPairId = pair.Id;

            await _context.Klines.AddAsync(secondLastKline);
            await _context.Klines.AddAsync(lastKline);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
