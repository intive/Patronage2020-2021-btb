﻿using BTB.Application.Common.Interfaces;
using BTB.Application.System.Commands.SeedSampleData;
using BTB.Common;
using BTB.Domain.Common;
using BTB.Domain.Entities;
using BTB.Domain.Enums;
using BTB.Infrastructure;
using BTB.Persistence;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UnitTests.Common
{
    public class BTBContextFactory
    {
        public static BTBDbContext Create()
        {
            var options = new DbContextOptionsBuilder<BTBDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var dateTime = DateTimeMockFactory.DateTimeMock;
            var userAccessor = UserAccessorMockFactory.UserAccessorMock;

            var context = new BTBDbContext(options, userAccessor.Object, dateTime.Object);

            context.Database.EnsureCreated();

            context.UserProfileInfo.AddRange
            (
                new UserProfileInfo { Id = 1, UserId = "1", Username = "UserOne", ProfileBio = "", FavouriteTradingPair = ""},
                new UserProfileInfo { Id = 2, UserId = "2", Username = "UserTwo", ProfileBio = "", FavouriteTradingPair = ""},
                new UserProfileInfo { Id = 3, UserId = "3", Username = "UserThree", ProfileBio = "", FavouriteTradingPair = ""}
            );

            context.Klines.AddRange
            (
                new Kline() { Id = 1 , SymbolPairId = 1, OpenTimestamp = 0, DurationTimestamp = TimestampInterval.OneDay, ClosePrice = 1, OpenPrice = 1, Volume = 1 }
            );

            context.Symbols.AddRange
            (
                new Symbol() { Id = 1 , SymbolName = "BTC"},
                new Symbol() { Id = 2 , SymbolName = "USDT" }
            );

            context.SymbolPairs.AddRange
            (
                new SymbolPair() { Id = 1, BuySymbolId = 1, SellSymbolId = 2},
                new SymbolPair() { Id = 2, BuySymbolId = 1, SellSymbolId = 2}          
            );

            context.FavoriteSymbolPairs.AddRange(
                new FavoriteSymbolPair() { ApplicationUserId = "userId", SymbolPairId = 1}
            );

            context.Alerts.AddRange
            (
                new Alert() { Id = 1, UserId = "1", SymbolPairId = 1, Condition = AlertCondition.Crossing, ValueType = AlertValueType.Volume, Value = 1000.0m, SendEmail = true, Email = "alert1@alert1.com" },
                new Alert() { Id = 2, UserId = "1", SymbolPairId = 1, Condition = AlertCondition.Crossing, ValueType = AlertValueType.Price,  Value = 2000.0m, SendEmail = false, Email = null },
                new Alert() { Id = 3, UserId = "2", SymbolPairId = 1, Condition = AlertCondition.Crossing, ValueType = AlertValueType.Volume, Value = 3000.0m, SendEmail = true, Email = "alert3@alert3.com" },
                new Alert() { Id = 4, UserId = "2", SymbolPairId = 1, Condition = AlertCondition.Crossing, ValueType = AlertValueType.Price, Value = 4000.0m, SendEmail = false, Email = null },
                new Alert() { Id = 5, UserId = "3", SymbolPairId = 1, Condition = AlertCondition.Crossing, ValueType = AlertValueType.Volume, Value = 5000.0m, SendEmail = true, Email = "alert5@alert5.com" },
                new Alert() { Id = 6, UserId = "3", SymbolPairId = 1, Condition = AlertCondition.Crossing, ValueType = AlertValueType.Price, Value = 6000.0m, SendEmail = false, Email = null }
            );

            context.EmailTemplates.Add(new EmailTemplate() { Id = 1, Content = "<MESSAGE><br><DOMAIN_URL>"});

            context.AlertMessageTemplates.AddRange(AlertMessageTemplates.GetTemplates());

            context.Bets.AddRange
            (
                new Bet() { Id = 1, UserId = "1", SymbolPairId = 1, Points = 100, LowerPriceThreshold = 7000, UpperPriceThreshold = 8000, StartedAt = new DateTime(2020, 1, 1, 0, 0, 0), RateType = BetRateType.Standard, TimeInterval = BetTimeInterval.TwoDays, KlineOpenTimestamp = 100, IsActive = true },
                new Bet() { Id = 2, UserId = "2", SymbolPairId = 1, Points = 200, LowerPriceThreshold = 7000, UpperPriceThreshold = 8000, StartedAt = new DateTime(2020, 1, 1, 0, 0, 0), RateType = BetRateType.Standard, TimeInterval = BetTimeInterval.TwoDays, KlineOpenTimestamp = 200, IsActive = true },
                new Bet() { Id = 3, UserId = "3", SymbolPairId = 1, Points = 300, LowerPriceThreshold = 7000, UpperPriceThreshold = 8000, StartedAt = new DateTime(2020, 1, 1, 0, 0, 0), RateType = BetRateType.Standard, TimeInterval = BetTimeInterval.TwoDays, KlineOpenTimestamp = 300, IsActive = false }
            );

            context.SaveChanges();

            return context;
        }

        public static void Destroy(BTBDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
