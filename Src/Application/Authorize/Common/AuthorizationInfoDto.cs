using AutoMapper;
using BTB.Application.Common.Mappings;
using BTB.Domain.Entities;
using System.Collections.Generic;

namespace BTB.Application.Authorize.Common
{
    public class AuthorizationInfoDto : IMapFrom<AuthorizationInfo>
    {
        public bool IsAuthenticated { get; set; }
        public string UserName { get; set; }
        public Dictionary<string, string> ExposedClaims { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<AuthorizationInfo, AuthorizationInfoDto>();
        }
    }
}