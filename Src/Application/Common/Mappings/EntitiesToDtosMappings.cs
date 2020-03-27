using AutoMapper;
using BTB.Application.Alerts.Common;
using BTB.Application.UserProfile.Common;
using BTB.Domain.Entities;

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
