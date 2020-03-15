using System;
using System.Collections.Generic;
using System.Text;

namespace BTB.Domain.Entities
{
    public class Symbol
    {
        public int Id { get; set; }
        public string SymbolName { get; set; }
        public IEnumerable<Kline> KlinesAsSell { get; set; } = new List<Kline>();
        public IEnumerable<Kline> KlinesAsBuy { get; set; } = new List<Kline>();
    }
}
