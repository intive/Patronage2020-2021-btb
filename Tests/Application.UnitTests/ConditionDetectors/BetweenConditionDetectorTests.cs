using BTB.Application.ConditionDetectors;
using BTB.Application.ConditionDetectors.Between;
using BTB.Domain.Common;
using BTB.Domain.Entities;
using BTB.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Application.UnitTests.ConditionDetectors
{
    public class BetweenConditionDetectorTests
    {
        [Theory]
        [InlineData(AlertValueType.Price, 4, 5, 1, 4, 0)]
        [InlineData(AlertValueType.Price, 4, 5, 1, 4.5, 0)]
        [InlineData(AlertValueType.Price, 4, 5, 1, 5, 0)]
        [InlineData(AlertValueType.Volume, 4, 5, 1, 1, 4)]
        [InlineData(AlertValueType.Volume, 4, 5, 1, 1, 4.5)]
        [InlineData(AlertValueType.Volume, 4, 5, 1, 1, 5)]
        public void IsConditionMet_AlertOverload_ShouldReturnTrue_WhenConditionIsMet(AlertValueType valueType,
            decimal lowerThreshold, decimal upperThreshold, decimal openPrice, decimal closePrice, decimal volume)
        {
            var kline = new Kline()
            {
                OpenTimestamp = 1,
                SymbolPairId = 1,
                DurationTimestamp = TimestampInterval.FiveMin,
                OpenPrice = openPrice,
                ClosePrice = closePrice,
                Volume = volume
            };

            var alert = new Alert()
            {
                Condition = AlertCondition.Between,
                ValueType = valueType,
                Value = lowerThreshold,
                AdditionalValue = upperThreshold
            };

            var sut = new BetweenConditionDetector();
            Assert.True(sut.IsConditionMet(alert, new BasicConditionDetectorParameters() { Kline = kline }));
        }

        [Theory]
        [InlineData(4, 5, 1, 4)]
        [InlineData(4, 5, 1, 4.5)]
        [InlineData(4, 5, 1, 5)]
        public void IsConditionMet_BetOverload_ShouldReturnTrue_WhenConditionIsMet(decimal lowerThreshold,
            decimal upperThreshold, decimal openPrice, decimal closePrice)
        {
            var kline = new Kline()
            {
                OpenTimestamp = 1,
                SymbolPairId = 1,
                DurationTimestamp = TimestampInterval.FiveMin,
                OpenPrice = openPrice,
                ClosePrice = closePrice
            };

            var bet = new Bet()
            {
                LowerPriceThreshold = lowerThreshold,
                UpperPriceThreshold = upperThreshold
            };

            var sut = new BetweenConditionDetector();
            Assert.True(sut.IsConditionMet(bet, new BasicConditionDetectorParameters() { Kline = kline }));
        }

        [Theory]
        [InlineData(AlertValueType.Price, 4, 5, 1, 3, 0)]
        [InlineData(AlertValueType.Price, 4, 5, 1, 6, 0)]
        [InlineData(AlertValueType.Price, 4, 5, 6, 3, 0)]
        [InlineData(AlertValueType.Volume, 4, 5, 1, 1, 3)]
        [InlineData(AlertValueType.Volume, 4, 5, 1, 1, 6)]
        [InlineData(AlertValueType.Volume, -5, -4, 1, 1, -6)]
        public void IsConditionMet_AlertOverload_ShouldReturnFalse_WhenConditionIsNotMet(AlertValueType valueType,
            decimal lowerThreshold, decimal upperThreshold, decimal openPrice, decimal closePrice, decimal volume)
        {
            var kline = new Kline()
            {
                OpenTimestamp = 1,
                SymbolPairId = 1,
                DurationTimestamp = TimestampInterval.FiveMin,
                OpenPrice = openPrice,
                ClosePrice = closePrice,
                Volume = volume
            };

            var alert = new Alert()
            {
                Condition = AlertCondition.Between,
                ValueType = valueType,
                Value = lowerThreshold,
                AdditionalValue = upperThreshold
            };

            var sut = new BetweenConditionDetector();
            Assert.False(sut.IsConditionMet(alert, new BasicConditionDetectorParameters() { Kline = kline }));
        }

        [Theory]
        [InlineData(4, 5, 1, 3)]
        [InlineData(4, 5, 1, 6)]
        [InlineData(4, 5, 6, 3)]
        public void IsConditionMet_BetOverload_ShouldReturnFalse_WhenConditionIsNotMet(decimal lowerThreshold,
            decimal upperThreshold, decimal openPrice, decimal closePrice)
        {
            var kline = new Kline()
            {
                OpenTimestamp = 1,
                SymbolPairId = 1,
                DurationTimestamp = TimestampInterval.FiveMin,
                OpenPrice = openPrice,
                ClosePrice = closePrice
            };

            var bet = new Bet()
            {
                LowerPriceThreshold = lowerThreshold,
                UpperPriceThreshold = upperThreshold
            };

            var sut = new BetweenConditionDetector();
            Assert.False(sut.IsConditionMet(bet, new BasicConditionDetectorParameters() { Kline = kline }));
        }
    }
}
