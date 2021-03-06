﻿using BTB.Application.ConditionDetectors;
using BTB.Application.ConditionDetectors.Crossing;
using BTB.Domain.Common;
using BTB.Domain.Entities;
using BTB.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Application.UnitTests.ConditionDetectors
{
    public class CrossingUpConditionDetectorTests
    {
        [Theory]
        [InlineData(AlertValueType.Price, 5, 1, 10, 0)]
        [InlineData(AlertValueType.Price, 5, 1, 5, 0)]
        [InlineData(AlertValueType.Price, 5, 5, 10, 0)]
        [InlineData(AlertValueType.Volume, 5, 1, 1, 10)]
        [InlineData(AlertValueType.Volume, 5, 1, 1, 5)]
        public void IsConditionMet_ShouldReturnTrue_WhenCrossingUpOccurs(AlertValueType valueType,
            decimal threshold, decimal openPrice, decimal closePrice, decimal volume)
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
                Condition = AlertCondition.CrossingUp,
                ValueType = valueType,
                Value = threshold
            };

            var sut = new CrossingUpConditionDetector();
            bool result = sut.IsConditionMet(alert, new BasicConditionDetectorParameters() { Kline = kline });
            Assert.True(result);
        }

        [Theory]
        [InlineData(AlertValueType.Price, 5, 1, 4, 0)]
        [InlineData(AlertValueType.Price, 5, 6, 10, 0)]
        [InlineData(AlertValueType.Volume, 5, 1, 1, 4)]
        [InlineData(AlertValueType.Volume, -5, 1, 1, -4)]

        [InlineData(AlertValueType.Price, 5, 10, 1, 0)]
        [InlineData(AlertValueType.Price, 5, 5, 1, 0)]
        [InlineData(AlertValueType.Price, 5, 10, 5, 0)]
        [InlineData(AlertValueType.Volume, -5, 1, 1, -10)]
        [InlineData(AlertValueType.Volume, -5, 1, 1, -5)]
        public void IsConditionMet_ShouldReturnFalse_WhenCrossingUpDoesNotOccur(AlertValueType valueType,
            decimal threshold, decimal openPrice, decimal closePrice, decimal volume)
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
                Condition = AlertCondition.CrossingUp,
                ValueType = valueType,
                Value = threshold
            };

            var sut = new CrossingUpConditionDetector();
            bool result = sut.IsConditionMet(alert, new BasicConditionDetectorParameters() { Kline = kline });
            Assert.False(result);
        }
    }
}
