﻿using AutoMapper;
using BTB.Application.Alerts.Common;
using BTB.Application.System.Common;
using BTB.Application.UserProfile.Common;
using BTB.Domain.Common;
using BTB.Domain.Entities;
using BTB.Domain.ValueObjects;
using System.Linq;

namespace BTB.Application.Common.Mappings
{
    public class EntitiesToDtosMappings : Profile
    {
        public EntitiesToDtosMappings()
        {
            CreateMap<UserProfileInfo, UserProfileInfoVm>();
            CreateMap<Alert, AlertVm>();
            CreateMap<AuditTrail, AuditTrailVm>();
            CreateMap<SymbolPair, SimplePriceVO>()
                .ForMember(s => s.BuySymbolName, opts => opts.MapFrom(src => src.BuySymbol.SymbolName))
                .ForMember(s => s.SellSymbolName, opts => opts.MapFrom(src => src.SellSymbol.SymbolName))
                .ForMember(s => s.ClosePrice, opts => opts.MapFrom(src => src.Klines.Where(k => k.DurationTimestamp == TimestampInterval.OneDay).OrderBy(k => k.DurationTimestamp).LastOrDefault().ClosePrice))
                .ForMember(s => s.Volume, opts => opts.MapFrom(src => src.Klines.Where(k => k.DurationTimestamp == TimestampInterval.OneDay).OrderBy(k => k.DurationTimestamp).LastOrDefault().Volume));      
        }
    }
}
