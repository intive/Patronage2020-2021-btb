﻿using BTB.Application.Common.Models;
using BTB.Domain.Common.Pagination;
using BTB.Domain.ValueObjects;
using MediatR;

namespace BTB.Application.FavoriteSymbolPairs.Queries.GetAllFavoriteSymbolPairs
{
    public class GetAllFavoriteSymbolPairsQuery : IRequest<PaginatedResult<SimplePriceVO>>
    {
        public PaginationDto Pagination { get; set; }
    }
}
