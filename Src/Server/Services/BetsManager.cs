﻿using AutoMapper;
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
using BTB.Common;
using BTB.Application.Bets.Commands.UpdateBetCommand;
using BTB.Application.Common.Exceptions;
using MediatR;
using BTB.Application.Bets.Commands.DeleteBetCommand;
using System.Collections.Generic;

namespace BTB.Server.Services
{
    public class BetsManager : IBetsManager
    {
        private readonly IBTBDbContext _context;
        private readonly IMapper _mapper;
        private readonly IBTBBinanceClient _client;
        private readonly IGamblePointManager _gamblePointsManager;
        private ILogger<GamblePointManager> _logger;
        private readonly IDateTime _dateTime;
        private readonly IBetConditionDetector<BasicConditionDetectorParameters> _betConditionDetector;

        public BetsManager(IBTBDbContext context, IMapper mapper, IBTBBinanceClient client, IGamblePointManager gamblePointsManager,
            ILoggerFactory loggerFactory, IDateTime dateTime, IBetConditionDetector<BasicConditionDetectorParameters> betConditionDetector)
        {
            _context = context;
            _mapper = mapper;
            _client = client;
            _gamblePointsManager = gamblePointsManager;
            _logger = loggerFactory.CreateLogger<GamblePointManager>();
            _dateTime = dateTime;
            _betConditionDetector = betConditionDetector;
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

            var now = _dateTime.Now;
            bet.StartedAt = now;
            long lastFiveMinPeriodTimestamp = DateTimestampConv.GetTimestamp(RoundDown(now, TimeSpan.FromMinutes(5)));
            bet.KlineOpenTimestamp = lastFiveMinPeriodTimestamp + (long)bet.TimeInterval;

            await _context.Bets.AddAsync(bet, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return await SetUsernameForBetValueObject(_mapper.Map<BetVO>(bet));
        }

        public async Task<BetVO> UpdateBetAsync(UpdateBetCommand request, string userId, CancellationToken cancellationToken)
        {
            Bet betToUpdate = await _context.Bets.SingleOrDefaultAsync(b => b.Id == request.Id && b.UserId == userId, cancellationToken);
            if (betToUpdate == null)
            {
                throw new NotFoundException($"Bet '{request.Id}' does not exist or does not belong to user {userId}.");
            }
            if (!betToUpdate.IsEditable)
            {
                throw new BadRequestException($"Bet '{request.Id}' cannot be changed.");
            }

            SymbolPair symbolPair = await _client.GetSymbolPairByName(request.SymbolPair);
            if (symbolPair == null)
            {
                throw new BadRequestException($"Trading pair symbol '{request.SymbolPair}' does not exist.");
            }

            Kline lastPairKline = await GetLastFiveMinuteKlineBySymbolPairIdAsync(symbolPair.Id);
            if (IsBetPriceRangeAboveLimit(request.LowerPriceThreshold, request.UpperPriceThreshold, lastPairKline.ClosePrice))
            {
                throw new PriceRangeAboveLimitException($"Cannot place bet. Price range is above limit. Current price for" +
                    $"{symbolPair.PairName} is {lastPairKline.ClosePrice}.");
            }

            var differenceInPoints = betToUpdate.Points - request.Points;

            if (differenceInPoints > 0)
            {
                await _gamblePointsManager.UnsealGamblePoints(userId, differenceInPoints, cancellationToken);
                await ChangeBetValues(request, betToUpdate, symbolPair, cancellationToken);
            }
            else if (differenceInPoints < 0)
            {
                if (-differenceInPoints > _gamblePointsManager.GetNumberOfFreePoints(userId))
                {
                    throw new BadRequestException($"User (id: {userId}) does not have enough points to place a bet with {request.Points} points.");
                }

                await _gamblePointsManager.SealGamblePoints(userId, -differenceInPoints, cancellationToken);
                await ChangeBetValues(request, betToUpdate, symbolPair, cancellationToken);
            }
            else
            {
                await ChangeBetValues(request, betToUpdate, symbolPair, cancellationToken);
            }

            return await SetUsernameForBetValueObject(_mapper.Map<BetVO>(betToUpdate));
        }

        public async Task<Unit> DeleteBetAsync(DeleteBetCommand request, string userId, CancellationToken cancellationToken)
        {
            Bet betToDelete = await _context.Bets.SingleOrDefaultAsync(b => b.Id == request.Id && b.UserId == userId, cancellationToken);
            if (betToDelete == null)
            {
                throw new NotFoundException($"Bet '{request.Id}' does not exist or does not belong to user {userId}.");
            }

            await _gamblePointsManager.UnsealGamblePoints(userId, betToDelete.Points, cancellationToken);
            _context.Bets.Remove(betToDelete);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }

        public async Task CheckActiveBetsAsync(CancellationToken cancellationToken)
        {
            var now = _dateTime.Now;
            var activeBets = await _context.Bets.Where(bet => bet.IsActive).ToListAsync();
            foreach (var activeBet in activeBets)
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
            _context.Bets.UpdateRange(activeBets);
            await _context.SaveChangesAsync(cancellationToken);
        }

        private async Task ChangeBetValues(UpdateBetCommand request, Bet betToUpdate, SymbolPair symbolPair, CancellationToken cancellationToken)
        {
            betToUpdate.SymbolPair = symbolPair;
            betToUpdate.SymbolPairId = symbolPair.Id;
            betToUpdate.Points = request.Points;
            betToUpdate.LowerPriceThreshold = request.LowerPriceThreshold;
            betToUpdate.UpperPriceThreshold = request.UpperPriceThreshold;

            var now = _dateTime.Now;
            betToUpdate.StartedAt = now;

            long lastFiveMinPeriodTimestamp = DateTimestampConv.GetTimestamp(RoundDown(now, TimeSpan.FromMinutes(5)));
            betToUpdate.KlineOpenTimestamp = lastFiveMinPeriodTimestamp + (long)betToUpdate.TimeInterval;

            await _context.SaveChangesAsync(cancellationToken);
        }

        private async Task HandleBetOutcomeAsync(Bet bet, CancellationToken cancellationToken)
        {
            decimal delta = 0.0m;
            Kline kline = await GetKlineByOpenTimestampAsync(bet.SymbolPairId, bet.KlineOpenTimestamp);
            if (kline == null)
            {
                delta = 0.0m;
                _logger.LogError($"Could not find kline while checking bet. SymbolPairId: {bet.SymbolPairId}, OpenTimestamp: {bet.KlineOpenTimestamp}");
            }
            else
            {
                bool isBetWon = _betConditionDetector.IsConditionMet(bet, new BasicConditionDetectorParameters() { Kline = kline });
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

        public async Task<PaginatedResult<BetVO>> GetAllActiveBetsAsync(PaginationDto pagination, string userId, CancellationToken cancellationToken)
        {
            var bets =
                from bet in _context.Bets
                .Include(bet => bet.SymbolPair).ThenInclude(sp => sp.BuySymbol)
                .Include(bet => bet.SymbolPair).ThenInclude(sp => sp.SellSymbol)
                .Include(bet => bet.User).ThenInclude(user => user.ProfileInfo)
                where bet.IsActive == true
                where userId != null ? bet.UserId == userId : true
                select new BetVO(bet, bet.User.ProfileInfo.Username);

            int betsCount = await bets.CountAsync(cancellationToken);
            var betsList = bets.ToList();

            return new PaginatedResult<BetVO>()
            {
                Result = betsList.Paginate(pagination),
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

        private async Task<BetVO> SetUsernameForBetValueObject(BetVO vo)
        {
            UserProfileInfo info = await _context.UserProfileInfo.SingleOrDefaultAsync(info => info.UserId == vo.UserId);
            vo.Username = info?.Username;
            return vo;
        }

        private DateTime RoundDown(DateTime dateTime, TimeSpan span)
        {
            var delta = dateTime.Ticks % span.Ticks;
            return new DateTime(dateTime.Ticks - delta, dateTime.Kind);
        }

        private bool DidBetCooldownPeriodExpire(Bet bet, DateTime currentTime)
        {
            long betStartedAtAsTimestamp = DateTimestampConv.GetTimestamp(bet.StartedAt);
            long currentTimeAsTimestamp = DateTimestampConv.GetTimestamp(currentTime);

            // I added 300 seconds here to give some five-minute klines a chance to load.
            // Essentially, this delays the bet checking - DidBetCooldownPeriodExpire will return `true` the next time cron does his work.
            if (currentTimeAsTimestamp > betStartedAtAsTimestamp + (long)bet.TimeInterval + 300)
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