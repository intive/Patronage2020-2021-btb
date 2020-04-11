using MediatR;
using System.Collections.Generic;

namespace BTB.Application.Indicator.Commands.CalculateSMA
{
    public class CalculateSMACommand : IRequest<SMAVm>
    {
        public int? TimePeriod { get; set; }
        public List<string> Prices { get; set; }
    }
}
