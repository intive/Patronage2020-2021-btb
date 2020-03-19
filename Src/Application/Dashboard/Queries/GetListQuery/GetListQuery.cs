using MediatR;
using System.Linq;
using System.Threading;
using BTB.Domain.Entities;
using BTB.Domain.Extensions;
using Binance.Net.Interfaces;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using BTB.Domain.Common.Pagination;
using BTB.Application.Common.Exceptions;

namespace BTB.Application.Dashboard.Queries.GetListQuery
{
    public class GetListQuery : IRequest<IEnumerable<BinanceSimpleElement>>
    {
        public PaginationDto Pagination { get; set; }
        public string Name { get; set; }

        public class GetListQueryHandler : IRequestHandler<GetListQuery, IEnumerable<BinanceSimpleElement>>
        {
            private readonly IBinanceClient _client;
            private readonly IHttpContextAccessor _httpContextAccessor;

            public GetListQueryHandler(IBinanceClient client, IHttpContextAccessor httpContextAccessor)
            {
                _client = client;
                _httpContextAccessor = httpContextAccessor;
            }

            public async Task<IEnumerable<BinanceSimpleElement>> Handle(GetListQuery request, CancellationToken cancellationToken)
            {
                var result = await _client.Get24HPricesListAsync(cancellationToken);

                if (result.Success)
                {
                    var data = result.Data.AsQueryable();

                    var selectedData = data
                        .Select(d => new BinanceSimpleElement
                        {
                            Symbol = d.Symbol,
                            LastPrice = d.LastPrice,
                            Volume = d.Volume
                        });

                    if (!string.IsNullOrEmpty(request.Name))
                    {
                        selectedData = selectedData
                            .Where(d => d.Symbol.Contains(request.Name.ToUpper()));
                    }

                    _httpContextAccessor.HttpContext.InsertPaginationParameterInResponseHeader(selectedData.Count(),
                        (int)request.Pagination.Quantity);


                    var selectedDataList = selectedData.Paginate(request.Pagination).ToList();

                    if (!selectedDataList.Any())
                    {
                        throw new BadRequestException(selectedDataList);
                    }

                    return selectedDataList;
                }

                throw new BadRequestException(result.Error.Message);
            }
        }
    }
}
