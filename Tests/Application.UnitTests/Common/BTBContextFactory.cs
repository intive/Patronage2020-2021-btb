using BTB.Application.Common.Interfaces;
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
                new Alert() { Id = 1, UserId = "1", SymbolPairId = 1, Condition = AlertCondition.Crossing, ValueType = AlertValueType.Volume, Value = 1000.0m, SendEmail = true, Email = "alert1@alert1.com", Message = "alert id: 1, user id: 1" },
                new Alert() { Id = 2, UserId = "1", SymbolPairId = 1, Condition = AlertCondition.Crossing, ValueType = AlertValueType.Price,  Value = 2000.0m, SendEmail = false, Email = null, Message = null },
                new Alert() { Id = 3, UserId = "2", SymbolPairId = 1, Condition = AlertCondition.Crossing, ValueType = AlertValueType.Volume, Value = 3000.0m, SendEmail = true, Email = "alert3@alert3.com", Message = "alert id: 3, user id: 2" },
                new Alert() { Id = 4, UserId = "2", SymbolPairId = 1, Condition = AlertCondition.Crossing, ValueType = AlertValueType.Price, Value = 4000.0m, SendEmail = false, Email = null, Message = null },
                new Alert() { Id = 5, UserId = "3", SymbolPairId = 1, Condition = AlertCondition.Crossing, ValueType = AlertValueType.Volume, Value = 5000.0m, SendEmail = true, Email = "alert5@alert5.com", Message = "alert id: 5, user id: 3" },
                new Alert() { Id = 6, UserId = "3", SymbolPairId = 1, Condition = AlertCondition.Crossing, ValueType = AlertValueType.Price, Value = 6000.0m, SendEmail = false, Email = null, Message = null }
            );

            context.EmailTemplates.Add(new EmailTemplate() { Id = 1, Header = "<EmailHeader />", Footer = "<EmailFooter />" });

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
