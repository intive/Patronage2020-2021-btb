using BTB.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Application.System.Queries.GetSymbolPairsQuery
{
    public class GetSymbolPairsQueryHandler : IRequestHandler<GetSymbolPairsQuery, IEnumerable<string>>
    {
        private readonly IBTBDbContext _context;

        public GetSymbolPairsQueryHandler(IBTBDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<string>> Handle(GetSymbolPairsQuery request, CancellationToken cancellationToken)
        {
            return await _context.SymbolPairs
                .Include(sp => sp.BuySymbol)
                .Include(sp => sp.SellSymbol)
                .Select(sp => sp.PairName)
                .ToListAsync();
        }
    }
}
