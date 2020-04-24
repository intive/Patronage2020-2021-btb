using AutoMapper;
using Binance.Net.Objects;
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

            CreateMap<Alert, AlertVO>()
                .ForMember(vo => vo.SymbolPair, opt => opt.MapFrom(alert => alert.SymbolPair.PairName))
                .ForMember(vo => vo.Condition, opt => opt.MapFrom(alert => alert.Condition.ToString()))
                .ForMember(vo => vo.ValueType, opt => opt.MapFrom(alert => alert.ValueType.ToString()));

            CreateMap<AuditTrail, AuditTrailVm>();

            CreateMap<SymbolPair, DashboardPairVO>()
                .ForMember(s => s.Id, opts => opts.MapFrom(src => src.Id))
                .ForMember(s => s.BuySymbolName, opts => opts.MapFrom(src => src.BuySymbol.SymbolName))
                .ForMember(s => s.SellSymbolName, opts => opts.MapFrom(src => src.SellSymbol.SymbolName))
                .ForMember(s => s.ClosePrice, opts => opts.MapFrom(src => src.Klines.Where(k => k.DurationTimestamp == TimestampInterval.OneDay).OrderBy(k => k.DurationTimestamp).LastOrDefault().ClosePrice))
                .ForMember(s => s.Volume, opts => opts.MapFrom(src => src.Klines.Where(k => k.DurationTimestamp == TimestampInterval.OneDay).OrderBy(k => k.DurationTimestamp).LastOrDefault().Volume));

            CreateMap<Kline, KlineVO>()
                .ForMember(k => k.OpenTime, opts => opts.MapFrom(src => DateTimestampConv.GetDateTime(src.OpenTimestamp)))
                .ForMember(k => k.CloseTime, opts => opts.MapFrom(src => DateTimestampConv.GetDateTime(src.OpenTimestamp + (long)src.DurationTimestamp)));

            CreateMap<BinanceKline, KlineVO>()
                .ForMember(k => k.ClosePrice, opts => opts.MapFrom(src => src.Close))
                .ForMember(k => k.OpenPrice, opts => opts.MapFrom(src => src.Open))
                .ForMember(k => k.HighestPrice, opts => opts.MapFrom(src => src.High))
                .ForMember(k => k.LowestPrice, opts => opts.MapFrom(src => src.Low));

            CreateMap<ApplicationUser, ApplicationUserVO>()
                .ForMember(u => u.UserId, opts => opts.MapFrom(src => src.Id))
                .ForMember(u => u.UserName, opts => opts.MapFrom(src => src.UserName))
                .ForMember(u => u.Email, opts => opts.MapFrom(src => src.Email));
        }
    }
}
