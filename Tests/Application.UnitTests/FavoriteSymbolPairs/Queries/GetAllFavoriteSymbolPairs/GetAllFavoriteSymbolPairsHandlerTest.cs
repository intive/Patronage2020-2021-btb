using BTB.Application.FavoriteSymbolPairs.Queries.GetAllFavoriteSymbolPairs;
using BTB.Domain.Common.Pagination;
using BTB.Domain.ValueObjects;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Application.UnitTests.FavoriteSymbolPairs.Queries.GetAllFavoriteSymbolPairs
{
    public class GetAllFavoriteSymbolPairsHandlerTest : QueryTestsBase
    {
        [Fact]
        public async Task Handle_ShouldReturnAllFavoriteSymbolPairsBelongingToUser()
        {
            var userId = "userId";
            var expectedFavoritePairs = new List<DashboardPairVO>()
            {
               new DashboardPairVO() { BuySymbolName= "BTC", SellSymbolName = "USDT", ClosePrice = 1, Volume = 1, Id = 1, IsFavorite = true}
            };

            var expectedAllRecordsCount = 1;
            var userAccessorMock = GetUserAccessorMock(userId);

            var sut = new GetAllFavoriteSymbolPairsHandler(_context, _mapper, userAccessorMock.Object);
            var query = new GetAllFavoriteSymbolPairsQuery()
            {
                Pagination = new PaginationDto() { Page = 1, Quantity = PaginationQuantity.Ten }
            };

            var paginatedResult = await sut.Handle(query, CancellationToken.None);
            Assert.Equal(expectedAllRecordsCount, paginatedResult.AllRecordsCount);

            var resultFavoriteSymbolPairs = paginatedResult.Result.ToList();
            Assert.Equal(resultFavoriteSymbolPairs.Count(), expectedAllRecordsCount);

            foreach (var favoritePair in expectedFavoritePairs)
            {
                var single = resultFavoriteSymbolPairs.SingleOrDefault(
                    resultPair =>
                    resultPair.BuySymbolName == favoritePair.BuySymbolName &&
                    resultPair.SellSymbolName == favoritePair.SellSymbolName &&
                    resultPair.PairName == favoritePair.PairName &&
                    resultPair.Volume == favoritePair.Volume &&
                    resultPair.ClosePrice == favoritePair.ClosePrice &&
                    resultPair.Id == favoritePair.Id &&
                    resultPair.IsFavorite == favoritePair.IsFavorite
                );

                Assert.NotNull(single);
            }

            userAccessorMock.Verify(x => x.GetCurrentUserId());
        }
    }
}
