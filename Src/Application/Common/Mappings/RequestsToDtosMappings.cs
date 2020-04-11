using AutoMapper;
using BTB.Application.Indicator.Commands.CalculateRSI;
using BTB.Application.Indicator.Commands.CalculateSMA;
using System.Linq;
using System;

namespace BTB.Application.Common.Mappings
{
    public class RequestsToDtosMappings : Profile
    {
        public RequestsToDtosMappings()
        {
            CreateMap<CalculateRSICommand, CalculateRSIDto>()
                .ForMember(dest => dest.ClosePrices, opt => opt.MapFrom(s => s.ClosePrices.Select(t => Convert.ToDecimal(t)).ToList()));
            CreateMap<CalculateSMACommand, CalculateSMADto>()
                .ForMember(dest => dest.Prices, opt => opt.MapFrom(s => s.Prices.Select(t => Convert.ToDecimal(t)).ToList()));
        }
    }
}
