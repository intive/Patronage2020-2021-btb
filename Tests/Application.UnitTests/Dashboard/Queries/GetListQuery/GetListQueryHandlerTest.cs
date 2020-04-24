using BTB.Application.Dashboard.Queries.GetListQuery;
using BTB.Domain.Common.Pagination;
using BTB.Domain.ValueObjects;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Application.UnitTests.Dashboard.Queries.GetListQ
{
    public class GetListQueryHandlerTest : QueryTestsBase
    {
        [Fact]
        public async Task Handle_ShouldReturnAllSymbolPairs()
        {
            var userId = "userId";
            var expectedPairs = new List<DashboardPairVO>()
            {
               new DashboardPairVO() { BuySymbolName= "BTC", SellSymbolName = "USDT", ClosePrice = 1, Volume = 1, Id = 1, IsFavorite = true},
               new DashboardPairVO() { BuySymbolName= "BTC", SellSymbolName = "USDT", ClosePrice = 0, Volume = 0, Id = 2, IsFavorite = false}
            };

            var expectedAllRecordsCount = 2;
            var userAccessorMock = GetUserAccessorMock(userId);

            var query = new GetListQuery()
            {
                Pagination = new PaginationDto() { Page = 1, Quantity = PaginationQuantity.Ten }
            };

            var sut = new GetListQueryHandler(_context, _mapper, userAccessorMock.Object);

            var paginatedResult = await sut.Handle(query, CancellationToken.None);
            Assert.Equal(expectedAllRecordsCount, paginatedResult.AllRecordsCount);

            var resultSymbolPairs = paginatedResult.Result.ToList();
            Assert.Equal(resultSymbolPairs.Count(), expectedAllRecordsCount);

            foreach (var pair in expectedPairs)
            {
                var single = resultSymbolPairs.SingleOrDefault(
                    resultPair =>
                    resultPair.BuySymbolName == pair.BuySymbolName &&
                    resultPair.SellSymbolName == pair.SellSymbolName &&
                    resultPair.PairName == pair.PairName &&
                    resultPair.Volume == pair.Volume &&
                    resultPair.ClosePrice == pair.ClosePrice &&
                    resultPair.Id == pair.Id &&
                    resultPair.IsFavorite == pair.IsFavorite
                );

                Assert.NotNull(single);
            }

            userAccessorMock.Verify(x => x.GetCurrentUserId());
        }
    }
}
