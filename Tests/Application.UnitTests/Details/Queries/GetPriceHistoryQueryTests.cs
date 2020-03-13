using Binance.Net.Objects;
using BTB.Application.Details.Queries.GetPriceHistory;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using static BTB.Application.Details.Queries.GetPriceHistory.GetPriceHistoryQuery;

namespace Application.UnitTests.Details
{
    public class GetPriceHistoryQueryTests : QueryTestsBase
    {
        [Fact]
        public async Task Average_ShouldBeListOfNotNull()
        {
            var handler = new GetPriceHistoryQueryHandler(_binanceClient);

            var result = await handler.Handle(new GetPriceHistoryQuery { Symbol = "ANY", Interval = KlineInterval.OneHour }, CancellationToken.None);
            result.GetEnumerator().MoveNext();

            Assert.IsAssignableFrom<IEnumerable<BinanceSymbolPriceInTimeVm>>(result);
            Assert.NotNull(result.GetEnumerator());
        }
    }
}
