using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BTB.Domain.Entities
{
    public class Symbol
    {
        public int Id { get; set; }
        public string SymbolName { get; set; }

        public virtual IEnumerable<SymbolPair> BuySymbolPairs { get; set; } = new List<SymbolPair>();

        public virtual IEnumerable<SymbolPair> SellSymbolPairs { get; set; } = new List<SymbolPair>();
    }
}
