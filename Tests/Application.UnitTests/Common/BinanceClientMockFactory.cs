using BTB.Application.Common.Exceptions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reflection;
using System.Threading;
using System.Net;
using System;
using CryptoExchange.Net.Objects;
using Binance.Net.Interfaces;
using Moq;
using Binance.Net.Objects.Spot.MarketData;
using Binance.Net.Enums;

namespace Application.UnitTests.Common
{
    public abstract class BinanceClientMockFactory
    {
        public static Mock<IBinanceClient> ClientMock
        {
            get
            {
                Mock<IBinanceClient> _binanceClientMock = new Mock<IBinanceClient>();
                IncludeAllMockMethods(_binanceClientMock);
                return _binanceClientMock;
            }
        }

        private static void IncludeAllMockMethods(Mock<IBinanceClient> _binanceClientMock)
        {
            Type type = typeof(BinanceClientMockFactory);
            MethodInfo[] methods = type.GetMethods(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);

            foreach (var fun in methods)
            {
                try
                {
                    string name = fun.Name.Substring(0, 5);

                    if (String.Equals(name, "Mock_"))
                    {
                        fun.Invoke(null, new object[1] { _binanceClientMock });
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                }            
            }
        }

        private static void Mock_Get24HPricesListAsync(ref Mock<IBinanceClient> binanceClientMock)
        {
            var top10list = new List<Binance24HPrice>()
            {
                new Binance24HPrice { Symbol = "BTCNGN", LastPrice = 10 },
                new Binance24HPrice { Symbol = "BTCRUB", LastPrice = 9 },
                new Binance24HPrice { Symbol = "BTCTRY", LastPrice = 8 },
                new Binance24HPrice { Symbol = "BTCUSDS", LastPrice = 7 },
                new Binance24HPrice { Symbol = "BTCPAX", LastPrice = 6 },
                new Binance24HPrice { Symbol = "BTCUSDC", LastPrice = 5 },
                new Binance24HPrice { Symbol = "BTCBUSD", LastPrice = 4 },
                new Binance24HPrice { Symbol = "BTCTUSD", LastPrice = 3 },
                new Binance24HPrice { Symbol = "BTCUSDT", LastPrice = 2 },
                new Binance24HPrice { Symbol = "BTCEUR", LastPrice = 1 }
            };

            binanceClientMock.Setup(client => client.Get24HPricesListAsync(CancellationToken.None))
                .Returns(Task.Run(() => new WebCallResult<IEnumerable<Binance24HPrice>>(HttpStatusCode.OK, null, top10list, null)));
        }

        private static void Mock_GetKlinesAsync(ref Mock<IBinanceClient> binanceClientMock)
        {
            var historyList = new List<BinanceKline>()
            {
                new BinanceKline { CloseTime = DateTime.Now, Close = 5 },
                new BinanceKline { CloseTime = DateTime.Now.AddHours(-1), Close = 8 },
                new BinanceKline { CloseTime = DateTime.Now.AddHours(-2), Close = 21 },
                new BinanceKline { CloseTime = DateTime.Now.AddHours(-3), Close = 6 },
            };

            binanceClientMock
                .Setup(client => client.GetKlinesAsync("BTCUSDT", It.IsAny<KlineInterval>(), null, null, null, CancellationToken.None))
                .Returns(Task.Run(() => new WebCallResult<IEnumerable<BinanceKline>>(HttpStatusCode.OK, null, historyList, null)));

            binanceClientMock
                .Setup(client => client.GetKlinesAsync("BLAHBLAH", It.IsAny<KlineInterval>(), null, null, null, CancellationToken.None))
                .Throws(new BadRequestException("Could not get klines from Binance API."));
        }

        private static void Mock_GetPriceAsync(ref Mock<IBinanceClient> binanceClientMock)
        {
            var price = new BinancePrice { Symbol = "BTC", Price = 15 };

            binanceClientMock.Setup(client => client.GetPriceAsync(It.IsAny<string>(), CancellationToken.None))
                .Returns(Task.Run(() => new WebCallResult<BinancePrice>(HttpStatusCode.OK, null, price, null)));
        }
    } 
}
