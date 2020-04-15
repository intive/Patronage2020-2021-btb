using AutoMapper;
using BTB.Application.Alerts.Commands.CreateAlertCommand;
using BTB.Application.Alerts.Commands.UpdateAlertCommand;
using BTB.Application.FavoriteSymbolPairs.Commands.CreateFavoriteSymbolPair;
using BTB.Application.System.Commands.AddKlineCommand;
using BTB.Application.UserProfile.Commands.CreateUserProfileCommand;
using BTB.Application.UserProfile.Commands.UpdateUserProfileCommand;
using BTB.Domain.Common;
using BTB.Domain.Entities;
using BTB.Domain.Enums;
using System;

namespace BTB.Application.Common.Mappings
{
    public class RequestsToEntitiesMappings : Profile
    {
        public RequestsToEntitiesMappings()
        {
            CreateMap<CreateUserProfileCommand, UserProfileInfo>();
            CreateMap<UpdateUserProfileCommand, UserProfileInfo>();
            CreateMap<CreateFavoriteSymbolPairCommand, FavoriteSymbolPair>();

            CreateMap<CreateAlertCommand, Alert>()
                .ForMember(alert => alert.SymbolPair, opt => opt.Ignore());

            CreateMap<UpdateAlertCommand, Alert>()
                .ForMember(alert => alert.Id, opt => opt.Ignore())
                .ForMember(alert => alert.SymbolPair, opt => opt.Ignore());

            CreateMap<AddKlineCommand, Kline>()
                .ForMember(kline => kline.SymbolPair, opt => opt.Ignore());
        }
    }
}
