using BTB.Application.Details.Queries.GetPriceHistory;
using BTB.Application.Common.Exceptions;
using BTB.Domain.Common.Pagination;
using BTB.Domain.ValueObjects;
using BTB.Domain.Common;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Binance.Net.Objects;
using Xunit;
using System.Linq;

namespace Application.UnitTests.Details
{
    public class GetPriceHistoryQueryTests : QueryTestsBase
    {
        [Fact]
        public async Task Handle_WithDatabaseAsSource_ShouldReturnListOfNotNull()
        {
            const int AssumedSize = 1;

            var handler = new GetPriceHistoryQueryHandler(_context, _mapper, _binanceClientMock.Object);

            var result = await handler.Handle(new GetPriceHistoryQuery
            {
                PairName = "BTCUSDT",
                KlineType = KlineInterval.OneDay,
                Pagination = new PaginationDto
                {
                    Page = 1,
                    Quantity = PaginationQuantity.Ten
                },
                DataSource = DetailsDataSource.Database
            },
                CancellationToken.None);

            Assert.NotNull(result);
            Assert.NotNull(result.Result);
            Assert.Equal((int)PaginationQuantity.Ten, result.RecordsPerPage);
            Assert.Equal(AssumedSize, result.AllRecordsCount);
            Assert.IsAssignableFrom<IEnumerable<KlineVO>>(result.Result);
            Assert.Single(result.Result);
        }

        [Fact]
        public async Task Handle_WithDatabaseAsSource_WhenGivenSymbolDoesNotExist_ShouldCatchBadRequestException()
        {
            var handler = new GetPriceHistoryQueryHandler(_context, _mapper, _binanceClientMock.Object);
            var request = new GetPriceHistoryQuery
            {
                PairName = "BLAHBLAH",
                KlineType = KlineInterval.OneDay,
                Pagination = new PaginationDto
                {
                    Page = 1,
                    Quantity = PaginationQuantity.Ten
                },
                DataSource = DetailsDataSource.Database
            };

            await Assert.ThrowsAsync<BadRequestException>(async () => await handler.Handle(request, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_WithAPIAsSource_ShouldReturnListOfNotNull()
        {
            const int AssumedSize = 4;

            var handler = new GetPriceHistoryQueryHandler(_context, _mapper, _binanceClientMock.Object);

            var result = await handler.Handle(new GetPriceHistoryQuery
            {
                PairName = "BTCUSDT",
                KlineType = KlineInterval.FiveMinutes,
                Pagination = new PaginationDto
                {
                    Page = 1,
                    Quantity = PaginationQuantity.Ten
                },
                DataSource = DetailsDataSource.API
            },
                CancellationToken.None);

            Assert.NotNull(result);
            Assert.NotNull(result.Result);
            Assert.Equal((int)PaginationQuantity.Ten, result.RecordsPerPage);
            Assert.Equal(AssumedSize, result.AllRecordsCount);
            Assert.IsAssignableFrom<IEnumerable<KlineVO>>(result.Result);
            Assert.Equal(AssumedSize, result.Result.Count());
        }

        [Fact]
        public async Task Handle_WithAPIAsSource_WhenGivenSymbolDoesNotExist_ShouldCatchBadRequestException()
        {
            var handler = new GetPriceHistoryQueryHandler(_context, _mapper, _binanceClientMock.Object);
            var request = new GetPriceHistoryQuery
            {
                PairName = "BLAHBLAH",
                KlineType = KlineInterval.OneDay,
                Pagination = new PaginationDto
                {
                    Page = 1,
                    Quantity = PaginationQuantity.Ten
                },
                DataSource = DetailsDataSource.Database
            };

            await Assert.ThrowsAsync<BadRequestException>(async () => await handler.Handle(request, CancellationToken.None));
        }
    }
}
