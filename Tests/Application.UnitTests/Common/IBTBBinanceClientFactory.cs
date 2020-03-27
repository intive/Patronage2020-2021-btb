using Binance.Net.Objects;
using BTB.Application.Common.Interfaces;
using BTB.Application.Common.Models;
using BTB.Domain.Common;
using BTB.Domain.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UnitTests.Common
{
    public abstract class IBTBBinanceClientFactory
    {
        public static IBTBBinanceClient BTBClient
        {
            get
            {
                return NewBTBClient();
            }
        }

        private static IBTBBinanceClient NewBTBClient()
        {
            var _btbClientMock = new Mock<IBTBBinanceClient>();
            IncludeAllMockMethods(_btbClientMock);

            return _btbClientMock.Object;
        }

        private static void IncludeAllMockMethods(Mock<IBTBBinanceClient> _btbClientMock)
        {
            var type = typeof(IBTBBinanceClientFactory);
            var methods = type.GetMethods(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);

            foreach (var fun in methods)
            {
                try
                {
                    string name = fun.Name.Substring(0, 5);

                    if (String.Equals(name, "Mock_"))
                    {
                        fun.Invoke(null, new object[1] { _btbClientMock });
                    }
                }
                catch (ArgumentOutOfRangeException e)
                {
                }

            }
        }

        private static void Mock_Get24HPricesListAsync(ref Mock<IBTBBinanceClient> _btbClientMock)
        {

            var top10list = new List<SimplePriceVO>()
                {
                    new SimplePriceVO { BuySymbolName = "BTC", SellSymbolName = "NGN", ClosePrice = 10 },
                    new SimplePriceVO { BuySymbolName = "BTC", SellSymbolName = "RUB", ClosePrice = 9 },
                    new SimplePriceVO { BuySymbolName = "BTC", SellSymbolName = "TRY", ClosePrice = 8 },
                    new SimplePriceVO { BuySymbolName = "BTC", SellSymbolName = "USDS", ClosePrice = 7 },
                    new SimplePriceVO { BuySymbolName = "BTC", SellSymbolName = "PAX", ClosePrice = 6 },
                    new SimplePriceVO { BuySymbolName = "BTC", SellSymbolName = "USDC", ClosePrice = 5 },
                    new SimplePriceVO { BuySymbolName = "BTC", SellSymbolName = "BUSD", ClosePrice = 4 },
                    new SimplePriceVO { BuySymbolName = "BTC", SellSymbolName = "TUSD", ClosePrice = 3 },
                    new SimplePriceVO { BuySymbolName = "BTC", SellSymbolName = "USDT", ClosePrice = 2 },
                    new SimplePriceVO { BuySymbolName = "BTC", SellSymbolName = "EUR", ClosePrice = 1 }
                };

            _btbClientMock.Setup<Task<IEnumerable<KlineVO>>>(
                fun => fun.Get24HPricesListAsync()
            ).Returns(
                Task.Run(() =>
                {
                    return (IEnumerable<KlineVO>)top10list;
                })
            );
        }

        private static void Mock_GetKlinesFrom(ref Mock<IBTBBinanceClient> _btbClientMock)
        {

            var top10list = new List<KlineVO>()
                {
                    new KlineVO { BuySymbolName = "BTC", SellSymbolName = "NGN", OpenPrice = 1, ClosePrice = 1, OpenTime = DateTime.UtcNow, CloseTime = DateTime.UtcNow },
                    new KlineVO { BuySymbolName = "BTC", SellSymbolName = "RUB", OpenPrice = 2, ClosePrice = 2, OpenTime = DateTime.UtcNow, CloseTime = DateTime.UtcNow },
                    new KlineVO { BuySymbolName = "BTC", SellSymbolName = "TRY", OpenPrice = 3, ClosePrice = 3, OpenTime = DateTime.UtcNow, CloseTime = DateTime.UtcNow },
                    new KlineVO { BuySymbolName = "BTC", SellSymbolName = "USDS", OpenPrice = 4, ClosePrice = 4, OpenTime = DateTime.UtcNow, CloseTime = DateTime.UtcNow },
                    new KlineVO { BuySymbolName = "BTC", SellSymbolName = "PAX", OpenPrice = 5, ClosePrice = 5, OpenTime = DateTime.UtcNow, CloseTime = DateTime.UtcNow },
                    new KlineVO { BuySymbolName = "BTC", SellSymbolName = "USDC", OpenPrice = 6, ClosePrice = 6, OpenTime = DateTime.UtcNow, CloseTime = DateTime.UtcNow },
                    new KlineVO { BuySymbolName = "BTC", SellSymbolName = "BUSD", OpenPrice = 7, ClosePrice = 7, OpenTime = DateTime.UtcNow, CloseTime = DateTime.UtcNow },
                    new KlineVO { BuySymbolName = "BTC", SellSymbolName = "TUSD", OpenPrice = 8, ClosePrice = 8, OpenTime = DateTime.UtcNow, CloseTime = DateTime.UtcNow },
                    new KlineVO { BuySymbolName = "BTC", SellSymbolName = "USDT", OpenPrice = 9, ClosePrice = 9, OpenTime = DateTime.UtcNow, CloseTime = DateTime.UtcNow },
                    new KlineVO { BuySymbolName = "BTC", SellSymbolName = "EUR", OpenPrice = 10, ClosePrice = 10, OpenTime = DateTime.UtcNow, CloseTime = DateTime.UtcNow }
                };

            _btbClientMock.Setup<Task<List<KlineVO>>>(
                fun => fun.GetKlinesFrom(It.IsAny<TimestampInterval>(), It.IsAny<TimestampInterval>())
            ).Returns(
                Task.Run(() =>
                {
                    return top10list;
                })
            );

            IEnumerable<KlineVO> top10enumerable = top10list;

            _btbClientMock.Setup<Task<IEnumerable<KlineVO>>>(
                fun => fun.GetKlines(It.IsAny<TimestampInterval>(),It.IsAny<int>(), It.IsAny<string>())
            ).Returns(
                Task.Run(() =>
                {
                    return top10enumerable;
                })
            );
        }
    }
}
