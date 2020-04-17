using Binance.Net.Interfaces;
using Binance.Net.Objects;
using BTB.Application.Common.Exceptions;
using BTB.Application.Common.Interfaces;
using BTB.Domain.Common;
using BTB.Domain.Entities;
using BTB.Domain.Extensions;
using MediatR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

            private IEnumerable<BinanceSymbol> _exchangeData;
            private List<Binance24HPrice> _prices24hList;
            private Hashtable _symbolsTemp;
            private Action<Hashtable, string> _tableChecker;

            private Dictionary<string, int> _allowedSymbols;
            private const int PerSymbolLimit = 7;

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
                var response = _client.GetExchangeInfo();
                ValidateResponse((HttpStatusCode)response.ResponseStatusCode, response.Error);               

                _exchangeData = response.Data.Symbols;
                SetAllowedSymbols();

                if (!AreSymbolsAdded())
                {
                     await LoadAllSymbolsToDb();
                }

                if (!ArePairsAdded())
                {
                    var priceResponse = _client.Get24HPricesList();
                    ValidateResponse((HttpStatusCode)priceResponse.ResponseStatusCode, priceResponse.Error);

                    _prices24hList = priceResponse.Data.ToList();
                    await LoadPairsToDb();                 
                }

                return Unit.Value;
            }

            private void ValidateResponse(HttpStatusCode statusCode, object error)
            {
                if (statusCode != HttpStatusCode.OK)
                {
                    throw new ServiceUnavailableException(error);
                }
            }

            private void SetAllowedSymbols()
            {
                _allowedSymbols = new Dictionary<string, int>();
                
                foreach (var filter in Enum<CurrencyFilter>.GetValues())
                {
                    if (filter != CurrencyFilter.ALL)
                    {
                        _allowedSymbols.Add(filter.ToString(), PerSymbolLimit);
                    }
                }
            }

            private bool AreSymbolsAdded()
            {
                return _context.Symbols.FirstOrDefault() != default(Symbol);
            }

            private bool ArePairsAdded()
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

                    bool CanAddSymbolPair = false;

                    if (IsSymbolAllowed(symbolBuy.SymbolName) || IsSymbolAllowed(symbolSell.SymbolName))
                    {
                        if (!IsSymbolPairEmpty(symbolBuy.SymbolName, symbolSell.SymbolName))
                        {
                            symbolPairs.Add(new SymbolPair()
                            {
                                BuySymbol = symbolBuy,
                                SellSymbol = symbolSell
                            });
                        }
                    }
                }

                await _context.SymbolPairs.AddRangeAsync(symbolPairs.ToArray());
                _context.SaveChanges();
            }

            private bool IsSymbolAllowed(string symbolName)
            {
                bool result = false;

                if (_allowedSymbols.ContainsKey(symbolName))
                {
                    int curAmount = _allowedSymbols[symbolName];
                    if (curAmount > 0)
                    {
                        curAmount--;
                        _allowedSymbols[symbolName] = curAmount;
                        result = true;
                    }
                }

                return result;
            }

            private bool IsSymbolPairEmpty(string buySymbolName, string sellSymbolName)
            {
                string pairName = string.Concat(buySymbolName, sellSymbolName);
                bool isEmpty = true;

                var price = _prices24hList.FirstOrDefault(p => p.Symbol == pairName);

                if (price != default(Binance24HPrice))
                {
                    if (price.LastPrice != decimal.Zero)
                    {
                        isEmpty = false;
                    }
                }

                return isEmpty;
            }
        }
    }
}
