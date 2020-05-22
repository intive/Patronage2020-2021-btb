using AutoMapper;
using BTB.Application.Common.Mappings;
using BTB.Domain.Common.Indicator;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BTB.Application.Indicator.Commands.CalculateRSI
{
    public class CalculateRSIDto : IMapFrom<CalculateRSICommand>
    {
        public List<decimal> ClosePrices { get; set; }
        public RSITimeframe Timeframe { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CalculateRSICommand, CalculateRSIDto>()
                .ForMember(dest => dest.ClosePrices, opt => opt.MapFrom(s => s.ClosePrices.Select(t => Convert.ToDecimal(t)).ToList()));
        }
    }
}