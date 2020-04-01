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
            var expectedFavoritePairs = new List<SimplePriceVO>()
            {
               new SimplePriceVO() { BuySymbolName= "BTC", SellSymbolName = "USDT", ClosePrice = 1, Volume = 1}
            };

            var expectedAllRecordsCount = 1;
            var userIdentityMock = GetUserIdentityMock(userId);

            var sut = new GetAllFavoriteSymbolPairsHandler(_context, _mapper, userIdentityMock.Object);
            var command = new GetAllFavoriteSymbolPairsQuery()
            {
                Pagination = new PaginationDto() { Page = 1, Quantity = PaginationQuantity.Ten },
            };

            var paginatedResult = await sut.Handle(command, CancellationToken.None);
            Assert.Equal(expectedAllRecordsCount, paginatedResult.AllRecorsCount);

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
                    resultPair.ClosePrice == favoritePair.ClosePrice
                );

                Assert.NotNull(single);
            }

            userIdentityMock.VerifyGet(x => x.UserId);
        }
    }
}
