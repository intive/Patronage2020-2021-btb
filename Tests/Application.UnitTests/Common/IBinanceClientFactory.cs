using Binance.Net.Interfaces;
using Binance.Net.Objects;
using BTB.Domain.Entities;
using CryptoExchange.Net.Objects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UnitTests.Common
{
    public abstract class IBinanceClientFactory
    {
        public static IBinanceClient BinanceClient
        {
            get
            {
                return NewBinanceClient();
            }
        }

        private static IBinanceClient NewBinanceClient()
        {
            Mock<IBinanceClient> _binanceClientMock = new Mock<IBinanceClient>();
            IncludeAllMockMethods(_binanceClientMock);

            return _binanceClientMock.Object;
        }

        private static void IncludeAllMockMethods(Mock<IBinanceClient> _binanceClientMock)
        {
            Type type = typeof(IBinanceClientFactory);
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
                catch (ArgumentOutOfRangeException e)
                {
                }               

            }
        }

        private static void Mock_Get24HPricesListAsync(ref Mock<IBinanceClient> _binanceClientMock)
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

            _binanceClientMock.Setup<Task<WebCallResult<IEnumerable<Binance24HPrice>>>>(
                fun => fun.Get24HPricesListAsync(CancellationToken.None)
            ).Returns(
                Task.Run(() =>
                {
                    return new WebCallResult<IEnumerable<Binance24HPrice>>(HttpStatusCode.OK, null, top10list, null);
                })
            );
        }

        private static void Mock_GetKlinesAsync(ref Mock<IBinanceClient> _binanceClientMock)
        {
            var historyList = new List<BinanceKline>()
                {
                    new BinanceKline { CloseTime = DateTime.Now, Close = 5 },
                    new BinanceKline { CloseTime = DateTime.Now.AddHours(-1), Close = 8 },
                    new BinanceKline { CloseTime = DateTime.Now.AddHours(-2), Close = 21 },
                    new BinanceKline { CloseTime = DateTime.Now.AddHours(-3), Close = 6 },
                };

            _binanceClientMock.Setup<Task<WebCallResult<IEnumerable<BinanceKline>>>>(
               fun => fun.GetKlinesAsync(It.IsAny<string>(), It.IsAny<KlineInterval>(), null, null, null, CancellationToken.None)
           ).Returns(
               Task.Run(() =>
               {
                   return new WebCallResult<IEnumerable<BinanceKline>>(HttpStatusCode.OK, null, historyList, null);
               })
           );
        }

        private static void Mock_GetPriceAsync(ref Mock<IBinanceClient> _binanceClientMock)
        {
            var price = new BinancePrice { Symbol = "BTC", Price = 15 };

            _binanceClientMock.Setup<Task<WebCallResult<BinancePrice>>>(
               fun => fun.GetPriceAsync(It.IsAny<string>(), CancellationToken.None)
           ).Returns(
               Task.Run(() =>
               {
                   return new WebCallResult<BinancePrice>(HttpStatusCode.OK, null, price, null);
               })
           );
        }
    } 
}
