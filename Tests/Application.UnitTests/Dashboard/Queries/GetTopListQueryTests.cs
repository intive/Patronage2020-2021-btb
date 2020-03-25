using BTB.Application.Dashboard.Queries.GetTopListQuery;
using BTB.Domain.Entities;
using BTB.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using static BTB.Application.Dashboard.Queries.GetTopListQuery.GetTopListQuery;

namespace Application.UnitTests.Dashboard.Queries
{
    public class GetTopListQueryTests : QueryTestsBase
    {
        [Fact]
        public async Task Average_ShouldBeListOfNotNull ()
        {
            var handler = new GetTopListQueryHandler(_binanceClient);

            var result = await handler.Handle(new GetTopListQuery(), CancellationToken.None);
            result.GetEnumerator().MoveNext();

            Assert.IsAssignableFrom<IEnumerable<SimplePriceVO>>(result);
            Assert.NotNull(result.GetEnumerator());
        }
    }
}
