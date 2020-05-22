using AutoMapper;
using BTB.Application.Common.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BTB.Application.Indicator.Commands.CalculateSMA
{
    public class CalculateSMADto : IMapFrom<CalculateSMACommand>
    {
        public int TimePeriod { get; set; }
        public List<decimal> Prices { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CalculateSMACommand, CalculateSMADto>()
                .ForMember(dest => dest.Prices, opt => opt.MapFrom(s => s.Prices.Select(t => Convert.ToDecimal(t)).ToList()));
        }
    }
}