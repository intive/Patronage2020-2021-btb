using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BTB.Application.System.Queries.GetSymbolPairLastKlineQuery
{
    class GetSymbolPairLastKlineQueryValidator : AbstractValidator<GetSymbolPairLastKlineQuery>
    {
        public GetSymbolPairLastKlineQueryValidator()
        {
            RuleFor(request => request.SymbolName)
                .MaximumLength(20)
                .Matches("^$|^([A-Z]{5,20})$")
                .NotEmpty();
        }
    }
}
