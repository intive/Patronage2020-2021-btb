using AutoMapper;
using BTB.Application.Common.Exceptions;
using BTB.Application.Common.Interfaces;
using BTB.Domain.Common;
using BTB.Domain.Entities;
using BTB.Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Application.Bets.Commands.CreateBetCommand
{
    class CreateBetCommandHandler : IRequestHandler<CreateBetCommand, BetVO>
    {
        private readonly IBTBDbContext _context;
        private readonly IMapper _mapper;
        private readonly IBTBBinanceClient _client;
        private readonly IUserAccessor _userAccessor;
        private readonly IGamblePointManager _gamblePointsManager;

        public CreateBetCommandHandler(IBTBDbContext context, IMapper mapper, IBTBBinanceClient client, IUserAccessor userAccessor,
            IGamblePointManager gamblePointsManager)
        {
            _context = context;
            _mapper = mapper;
            _client = client;
            _userAccessor = userAccessor;
            _gamblePointsManager = gamblePointsManager;
        }

        public async Task<BetVO> Handle(CreateBetCommand request, CancellationToken cancellationToken)
        {
            SymbolPairVO names = _client.GetSymbolNames(request.SymbolPair);
            if (names == null)
            {
                throw new BadRequestException($"Trading pair symbol '{request.SymbolPair}' does not exist.");
            }

            string userId = _userAccessor.GetCurrentUserId();
            if (request.Points > _gamblePointsManager.GetNumberOfFreePoints(userId))
            {
                throw new BadRequestException($"User (id: {userId}) does not have enough points to place a bet with {request.Points} points.");
            }

            SymbolPair symbolPair = await _client.GetSymbolPairByName(request.SymbolPair);
            Kline lastPairKline = await GetLastKlineBySymbolPairIdAsync(symbolPair.Id);
            if (IsBetPriceRangeAboveLimit(request, lastPairKline))
            {
                throw new BadRequestException($"Cannot place bet. Price range is above limit. Current price for {symbolPair.PairName} is {lastPairKline.ClosePrice}.");
            }

            var bet = _mapper.Map<Bet>(request);
            bet.UserId = userId;
            bet.SymbolPair = symbolPair;
            bet.SymbolPairId = symbolPair.Id;
            bet.IsActive = true;

            var now = DateTime.Now;
            bet.CreatedAt = now;
            long lastFiveMinPeriodTimestamp = DateTimestampConv.GetTimestamp(RoundDown(now, TimeSpan.FromMinutes(5)));
            bet.KlineOpenTimestamp = lastFiveMinPeriodTimestamp + (long)bet.TimeInterval;

            await _context.Bets.AddAsync(bet, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            await _gamblePointsManager.SealGamblePoints(userId, bet.Points, cancellationToken);
            return _mapper.Map<BetVO>(bet);
        }

        private DateTime RoundDown(DateTime dateTime, TimeSpan span)
        {
            var delta = dateTime.Ticks % span.Ticks;
            return new DateTime(dateTime.Ticks - delta, dateTime.Kind);
        }

        private Task<Kline> GetLastKlineBySymbolPairIdAsync(int symbolPairId)
        {
            return _context.Klines
                .OrderByDescending(kline => kline.OpenTimestamp)
                .FirstOrDefaultAsync(kline => kline.SymbolPairId == symbolPairId && kline.DurationTimestamp == TimestampInterval.FiveMin);
        }

        private bool IsBetPriceRangeAboveLimit(CreateBetCommand request, Kline lastPairKline)
        {
            decimal delta = request.UpperPriceThreshold - request.LowerPriceThreshold;
            decimal rangeLimit = lastPairKline.ClosePrice * 0.1m;
            return delta > rangeLimit;
        }
    }
}
