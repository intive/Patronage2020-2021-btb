using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BTB.Application.System.Queries.GetSymbolPairsQuery
{
    public class GetSymbolPairsQuery : IRequest<IEnumerable<string>>
    {
    }
}
