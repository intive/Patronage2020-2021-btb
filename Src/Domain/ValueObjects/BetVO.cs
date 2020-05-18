using BTB.Domain.Entities;
using BTB.Domain.Enums;
using System;

namespace BTB.Domain.ValueObjects
{
    public class BetVO
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public string SymbolPair { get; set; }
        public decimal Points { get; set; }
        public decimal LowerPriceThreshold { get; set; }
        public decimal UpperPriceThreshold { get; set; }
        public DateTime StartedAt { get; set; }
        public BetRateType RateType { get; set; }
        public BetTimeInterval TimeInterval { get; set; }
        public long KlineOpenTimestamp { get; set; }
        public bool IsActive { get; set; }
        public bool IsEditable { get; set; }

        public BetVO()
        {
        }

        public BetVO(Bet bet, string username = null)
        {
            Id = bet.Id;
            UserId = bet.UserId;
            Username = username;
            SymbolPair = bet.SymbolPair.PairName;
            Points = bet.Points;
            LowerPriceThreshold = bet.LowerPriceThreshold;
            UpperPriceThreshold = bet.UpperPriceThreshold;
            StartedAt = bet.StartedAt;
            RateType = bet.RateType;
            TimeInterval = bet.TimeInterval;
            KlineOpenTimestamp = bet.KlineOpenTimestamp;
            IsActive = bet.IsActive;
            IsEditable = bet.IsEditable;
        }
    }
}
