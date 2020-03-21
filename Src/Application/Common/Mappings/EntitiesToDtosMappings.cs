using AutoMapper;
using BTB.Application.Alerts.Queries.GetAllAlertsQuery;
using BTB.Application.Authorize.Queries.GetUserInfo;
using BTB.Application.UserProfile.Commands.CreateUserProfileCommand;
using BTB.Application.UserProfile.Queries.GetUserProfileQuery;
using BTB.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BTB.Application.Common.Mappings
{
    public class EntitiesToDtosMappings : Profile
    {
        public EntitiesToDtosMappings()
        {
            CreateMap<UserProfileInfo, UserProfileInfoVm>();
            CreateMap<Alert, AlertVm>();
        }
    }
}
