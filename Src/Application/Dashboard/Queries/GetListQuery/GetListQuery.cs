using MediatR;
using System.Linq;
using System.Threading;
using BTB.Domain.Entities;
using BTB.Domain.Extensions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using BTB.Domain.Common.Pagination;
using BTB.Application.Common.Exceptions;
using BTB.Application.Common.Interfaces;
using BTB.Domain.ValueObjects;
using BTB.Application.Common.Models;

namespace BTB.Application.Dashboard.Queries.GetListQuery
{
    public class GetListQuery : IRequest<IEnumerable<SimplePriceVO>>
    {
        public PaginationDto Pagination { get; set; }
        public string Name { get; set; }

        public class GetListQueryHandler : IRequestHandler<GetListQuery, IEnumerable<SimplePriceVO>>
        {
            private readonly IBTBBinanceClient _client;
            private readonly IHttpContextAccessor _httpContextAccessor;

            public GetListQueryHandler(IBTBBinanceClient client, IHttpContextAccessor httpContextAccessor)
            {
                _client = client;
                _httpContextAccessor = httpContextAccessor;
            }

            public async Task<IEnumerable<SimplePriceVO>> Handle(GetListQuery request, CancellationToken cancellationToken)
            {
                var result = await _client.Get24HPricesListAsync();

                if (request.Name != null)
                {
                    result = await _client.FilterKlines(request.Name, result.ToList());
                }

                if (result.Any())
                {
                    var prices = await _client.ToSimplePrices(result.ToList());
                    var selectedData = prices.AsQueryable();

                    _httpContextAccessor.HttpContext.InsertPaginationParameterInResponseHeader(selectedData.Count(),
                        (int)request.Pagination.Quantity);

                    var selectedDataList = selectedData.Paginate(request.Pagination).ToList();

                    if (!selectedDataList.Any())
                    {
                        throw new BadRequestException(selectedDataList);
                    }

                    return selectedDataList;
                }

                throw new BadRequestException("Error");
            }
        }
    }
}
