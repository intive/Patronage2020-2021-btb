using AutoMapper;
using BTB.Application.Bets.Commands.CreateBetCommand;
using BTB.Application.Common.Exceptions.BetsManager;
using BTB.Application.Common.Interfaces;
using BTB.Application.Common.Models;
using BTB.Domain.Common;
using BTB.Domain.Common.Pagination;
using BTB.Domain.Entities;
using BTB.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BTB.Domain.Extensions;
using BTB.Application.ConditionDetectors;
using BTB.Application.ConditionDetectors.Between;
using BTB.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace BTB.Server.Services
{
    public class BetsManager : IBetsManager
    {
        private readonly IBTBDbContext _context;
        private readonly IMapper _mapper;
        private readonly IBTBBinanceClient _client;
        private readonly IGamblePointManager _gamblePointsManager;
        private ILogger<GamblePointManager> _logger;
        private readonly IConditionDetector<BasicConditionDetectorParameters> _betweenConditionDetector = new BetweenConditionDetector();

        public BetsManager(IBTBDbContext context, IMapper mapper, IBTBBinanceClient client, IGamblePointManager gamblePointsManager, ILoggerFactory loggerFactory)
        {
            _context = context;
            _mapper = mapper;
            _client = client;
            _gamblePointsManager = gamblePointsManager;
            _logger = loggerFactory.CreateLogger<GamblePointManager>();
        }

        public async Task<BetVO> CreateBetAsync(CreateBetCommand request, string userId, CancellationToken cancellationToken)
        {
            SymbolPair symbolPair = await _client.GetSymbolPairByName(request.SymbolPair);
            Kline lastPairKline = await GetLastFiveMinuteKlineBySymbolPairIdAsync(symbolPair.Id);
            if (IsBetPriceRangeAboveLimit(request.LowerPriceThreshold, request.UpperPriceThreshold, lastPairKline.ClosePrice))
            {
                throw new PriceRangeAboveLimitException($"Cannot place bet. Price range is above limit. Current price for" +
                    $"{symbolPair.PairName} is {lastPairKline.ClosePrice}.");
            }

            var bet = _mapper.Map<Bet>(request);
            bet.UserId = userId;
            bet.SymbolPair = symbolPair;
            bet.SymbolPairId = symbolPair.Id;
            bet.IsActive = true;

            // this line was here for easier testing
            //bet.TimeInterval = (BetTimeInterval)300;

            var now = DateTime.Now;
            bet.CreatedAt = now;
            long lastFiveMinPeriodTimestamp = DateTimestampConv.GetTimestamp(RoundDown(now, TimeSpan.FromMinutes(5)));
            bet.KlineOpenTimestamp = lastFiveMinPeriodTimestamp + (long)bet.TimeInterval;
            
            await _context.Bets.AddAsync(bet, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return _mapper.Map<BetVO>(bet);
        }

        public async Task CheckBetsAsync(CancellationToken cancellationToken)
        {
            var now = DateTime.Now;
            var allActiveBets = await _context.Bets.Where(bet => bet.IsActive).ToListAsync();
            foreach (var activeBet in allActiveBets)
            {
                if (!DidBetCooldownPeriodExpire(activeBet, now))
                {
                    continue;
                }
                else
                {
                    await HandleBetOutcomeAsync(activeBet, cancellationToken);
                    activeBet.IsActive = false;
                }
            }
            _context.Bets.UpdateRange(allActiveBets);
            await _context.SaveChangesAsync(cancellationToken);
        }

        private async Task HandleBetOutcomeAsync(Bet bet, CancellationToken cancellationToken)
        {
            decimal delta;
            Kline kline = await GetKlineByOpenTimestampAsync(bet.SymbolPairId, bet.KlineOpenTimestamp);
            if (kline == null)
            {
                delta = 0;
                _logger.LogError($"Could not find kline while checking bet. SymbolPairId: {bet.SymbolPairId}, OpenTimestamp: {bet.KlineOpenTimestamp}");
            }
            else
            {
                bool isBetWon = _betweenConditionDetector.IsConditionMet(bet, new BasicConditionDetectorParameters() { Kline = kline });
                delta = GetPointsDelta(bet, isBetWon);
            }

            await _gamblePointsManager.UnsealGamblePoints(bet.UserId, bet.Points, cancellationToken);
            await _gamblePointsManager.ChangeFreePointsAmount(bet.UserId, delta, cancellationToken);
        }

        private decimal GetPointsDelta(Bet bet, bool isBetWon)
        {
            decimal betRate = bet.RateType switch
            {
                BetRateType.Standard => 0.1m,
                _ => throw new NotImplementedException("Unknown bet rate type")
            };

            return bet.Points * betRate * (isBetWon ? 1.0m : -1.0m);
        }

        public async Task<PaginatedResult<BetVO>> GetAllActiveBetsAsync(PaginationDto pagination, CancellationToken cancellationToken)
        {
            IQueryable<BetVO> bets =
                from bet in _context.Bets
                .Include(bet => bet.SymbolPair).ThenInclude(sp => sp.BuySymbol)
                .Include(bet => bet.SymbolPair).ThenInclude(sp => sp.SellSymbol)
                select _mapper.Map<BetVO>(bet);

            int betsCount = await bets.CountAsync(cancellationToken);
            var list = bets.ToList();

            return new PaginatedResult<BetVO>()
            {
                Result = bets.Paginate(pagination),
                AllRecordsCount = betsCount,
                RecordsPerPage = (int)pagination.Quantity
            };
        }

        private bool IsBetPriceRangeAboveLimit(decimal lowerThreshold, decimal upperThreshold, decimal klineClosePrice)
        {
            decimal delta = upperThreshold - lowerThreshold;
            decimal rangeLimit = klineClosePrice * 0.1m;
            
            if (delta > rangeLimit)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private DateTime RoundDown(DateTime dateTime, TimeSpan span)
        {
            var delta = dateTime.Ticks % span.Ticks;
            return new DateTime(dateTime.Ticks - delta, dateTime.Kind);
        }

        private bool DidBetCooldownPeriodExpire(Bet bet, DateTime currentTime)
        {
            long betCreatedAtAsTimestamp = DateTimestampConv.GetTimestamp(bet.CreatedAt);
            long currentTimeAsTimestamp = DateTimestampConv.GetTimestamp(currentTime);

            // I added 300 seconds here to give some five-minute klines a chance to load.
            // Essentially, this delays the bet checking - DidBetCooldownPeriodExpire will return `true` the next time cron does his work.
            if (currentTimeAsTimestamp > betCreatedAtAsTimestamp + (long)bet.TimeInterval + 300)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private Task<Kline> GetLastFiveMinuteKlineBySymbolPairIdAsync(int symbolPairId)
        {
            return _context.Klines
                .OrderByDescending(kline => kline.OpenTimestamp)
                .FirstOrDefaultAsync(kline => kline.SymbolPairId == symbolPairId && kline.DurationTimestamp == TimestampInterval.FiveMin);
        }

        private Task<Kline> GetKlineByOpenTimestampAsync(int symbolPairId, long openTimestamp)
        {
            // Changed SingleOrDefault() to FirstOrDefaultAsync() to make sure this does not throw any exception
            // when there are duplicate klines in the db (klines with the same open and duration timestamps).
            return _context.Klines.FirstOrDefaultAsync(kline =>
                kline.SymbolPairId == symbolPairId &&
                kline.OpenTimestamp == openTimestamp &&
                kline.DurationTimestamp == TimestampInterval.FiveMin);
        }
    }
}
