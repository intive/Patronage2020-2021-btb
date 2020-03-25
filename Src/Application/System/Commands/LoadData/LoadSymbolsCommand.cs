using Binance.Net.Interfaces;
using Binance.Net.Objects;
using BTB.Application.Common.Interfaces;
using BTB.Domain.Entities;
using MediatR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Application.System.Commands.LoadData
{
    public class LoadSymbolsCommand : IRequest
    {
        public class LoadSymbolsCommandHandler : IRequestHandler<LoadSymbolsCommand>
        {
            private readonly IBTBDbContext _context;
            private readonly IBinanceClient _client;

            IEnumerable<BinanceSymbol> _exchangeData;
            private Hashtable _symbolsTemp;
            private Action<Hashtable, string> _tableChecker;

            private List<string> _allowedBuySymbols;

            public LoadSymbolsCommandHandler(IBTBDbContext context, IBinanceClient client)
            {
                _context = context;
                _client = client;

                _symbolsTemp = new Hashtable();

                _tableChecker = (hashtable, value) =>
                {
                    if (!hashtable.ContainsKey(value))
                    {
                        hashtable.Add(value, value);
                    }
                };
            }

            public async Task<Unit> Handle(LoadSymbolsCommand request, CancellationToken cancellationToken)
            {
                _exchangeData = _client.GetExchangeInfo().Data.Symbols;
                SetAllowedSymbols();

                if (!AreSymbolsAdded())
                {
                     await LoadAllSymbolsToDb();
                }

                if (!arePairsAdded())
                {
                    await LoadPairsToDb();
                }

                return Unit.Value;
            }

            private void SetAllowedSymbols()
            {
                _allowedBuySymbols = new List<string>();
                _allowedBuySymbols.Add("BTC");
            }

            private bool AreSymbolsAdded()
            {
                return _context.Symbols.FirstOrDefault() != default(Symbol);
            }

            private bool arePairsAdded()
            {
                return _context.SymbolPairs.FirstOrDefault() != default(SymbolPair);
            }

            private async Task LoadAllSymbolsToDb()
            {             
                FillSymbolHashtable();

                var symbolList = _symbolsTemp.Values;
                Symbol[] symbolArray = new Symbol[symbolList.Count];
                int index = 0;

                foreach (string symb in symbolList)
                {
                    symbolArray[index] = new Symbol()
                    {
                        SymbolName = symb
                    };
                    index++;
                }
                await _context.Symbols.AddRangeAsync(symbolArray);
                _context.SaveChanges();
            }

            private void FillSymbolHashtable()
            {
                string symbolBuy;
                string symbolSell;

                foreach (BinanceSymbol symb in _exchangeData)
                {
                    symbolBuy = symb.BaseAsset;
                    symbolSell = symb.QuoteAsset;

                    _tableChecker(_symbolsTemp, symbolBuy);
                    _tableChecker(_symbolsTemp, symbolSell);
                }
            }

            private async Task LoadPairsToDb()
            {
                List<SymbolPair> symbolPairs = new List<SymbolPair>();
                Symbol symbolBuy;
                Symbol symbolSell;

                foreach (BinanceSymbol symb in _exchangeData)
                {
                    symbolBuy = _context.Symbols.First(x => x.SymbolName == symb.BaseAsset);
                    symbolSell = _context.Symbols.First(x => x.SymbolName == symb.QuoteAsset);

                    if (_allowedBuySymbols.FirstOrDefault(a => string.Equals(a, symbolBuy.SymbolName)) == default)
                        continue;

                    symbolPairs.Add(new SymbolPair()
                    {
                        BuySymbol = symbolBuy,
                        SellSymbol = symbolSell
                    });
                }

                await _context.SymbolPairs.AddRangeAsync(symbolPairs.ToArray());
                _context.SaveChanges();
            }
        }
    }
}
