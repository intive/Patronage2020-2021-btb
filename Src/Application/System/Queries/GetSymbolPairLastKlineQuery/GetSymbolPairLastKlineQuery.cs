using BTB.Domain.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BTB.Application.System.Queries.GetSymbolPairLastKlineQuery
{
    public class GetSymbolPairLastKlineQuery : IRequest<KlineVO>
    {
        public string SymbolName { get; set; }
    }
}
