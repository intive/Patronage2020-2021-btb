using AutoMapper;
using BTB.Application.Alerts.Commands.CreateAlert;
using BTB.Application.Alerts.Commands.UpdateAlertCommand;
using BTB.Application.UserProfile.Commands.CreateUserProfileCommand;
using BTB.Application.UserProfile.Commands.UpdateUserProfileCommand;
using BTB.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BTB.Application.Common.Mappings
{
    public class RequestsToEntitiesMappings : Profile
    {
        public RequestsToEntitiesMappings()
        {
            CreateMap<CreateUserProfileCommand, UserProfileInfo>();
            CreateMap<UpdateUserProfileCommand, UserProfileInfo>();
            CreateMap<CreateAlertCommand, Alert>();

            CreateMap<UpdateAlertCommand, Alert>()
                .ForMember(aler => aler.Id, opt => opt.Ignore());
        }
    }
}
