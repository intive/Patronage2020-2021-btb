using Binance.Net.Objects;
using BTB.Application.Details.Queries.GetPriceHistory;
using BTB.Domain.ValueObjects;
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
            var handler = new GetPriceHistoryQueryHandler(_btbBinanceClientMock.Object);

            var result = await handler.Handle(new GetPriceHistoryQuery { PairName = "BTCRUB", KlineType = KlineInterval.FiveMinutes }, CancellationToken.None);
            result.GetEnumerator().MoveNext();

            Assert.IsAssignableFrom<IEnumerable<KlineVO>>(result);
            Assert.NotNull(result.GetEnumerator());
        }
    }
}
