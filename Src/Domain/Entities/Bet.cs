﻿using BTB.Domain.Common;
using BTB.Domain.Enums;
using System;

namespace BTB.Domain.Entities
{
    public class Bet : AuditableEntity
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public int SymbolPairId { get; set; }
        public virtual SymbolPair SymbolPair { get; set; }
        public decimal Points { get; set; }
        public decimal LowerPriceThreshold { get; set; }
        public decimal UpperPriceThreshold { get; set; }
        public DateTime StartedAt { get; set; }
        public BetRateType RateType { get; set; }
        public BetTimeInterval TimeInterval { get; set; }
        public long KlineOpenTimestamp { get; set; }
        public bool IsActive { get; set; }
        public bool IsEditable { get => DateTimestampConv.GetTimestamp(StartedAt) + (long)TimeInterval/2 > DateTimestampConv.GetTimestamp(DateTime.Now); }
    }
}
