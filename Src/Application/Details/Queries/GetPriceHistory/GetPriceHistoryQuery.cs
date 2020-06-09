using BTB.Application.Common.Models;
using BTB.Domain.Common.Pagination;
using BTB.Domain.ValueObjects;
using BTB.Domain.Common;
using MediatR;
using Binance.Net.Enums;

namespace BTB.Application.Details.Queries.GetPriceHistory
{
    public class GetPriceHistoryQuery : IRequest<PaginatedResult<KlineVO>>
    {
        public string PairName { get; set; }
        public PaginationDto Pagination { get; set; }
        public KlineInterval KlineType { get; set; }
        public DetailsDataSource DataSource { get; set; }
        public int ExtraAmount { get; set; }
    }
}