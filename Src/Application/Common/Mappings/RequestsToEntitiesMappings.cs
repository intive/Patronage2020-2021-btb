﻿using AutoMapper;
using BTB.Application.Alerts.Commands.CreateAlertCommand;
using BTB.Application.Alerts.Commands.UpdateAlertCommand;
using BTB.Application.Bets.Commands.CreateBetCommand;
using BTB.Application.FavoriteSymbolPairs.Commands.CreateFavoriteSymbolPair;
using BTB.Application.System.Commands.AddKlineCommand;
using BTB.Application.UserProfile.Commands.UpdateUserProfileCommand;
using BTB.Domain.Entities;

namespace BTB.Application.Common.Mappings
{
    public class RequestsToEntitiesMappings : Profile
    {
        public RequestsToEntitiesMappings()
        {
            CreateMap<UpdateUserProfileCommand, UserProfileInfo>();
            CreateMap<CreateFavoriteSymbolPairCommand, FavoriteSymbolPair>();

            CreateMap<CreateAlertCommand, Alert>()
                .ForMember(alert => alert.SymbolPair, opt => opt.Ignore())
                .ForMember(alert => alert.WasTriggered, opt => opt.Ignore());

            CreateMap<UpdateAlertCommand, Alert>()
                .ForMember(alert => alert.Id, opt => opt.Ignore())
                .ForMember(alert => alert.SymbolPair, opt => opt.Ignore())
                .ForMember(alert => alert.WasTriggered, opt => opt.Ignore());

            CreateMap<AddKlineCommand, Kline>()
                .ForMember(kline => kline.SymbolPair, opt => opt.Ignore());

            CreateMap<CreateBetCommand, Bet>()
                .ForMember(bet => bet.SymbolPair, opt => opt.Ignore());
        }
    }
}