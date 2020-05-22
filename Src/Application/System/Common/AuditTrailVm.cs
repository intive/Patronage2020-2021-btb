using AutoMapper;
using BTB.Application.Common.Mappings;
using BTB.Domain.Entities;
using System;

namespace BTB.Application.System.Common
{
    public class AuditTrailVm : IMapFrom<AuditTrail>
    {
        public string Column { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public DateTime Date { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<AuditTrail, AuditTrailVm>();
        }
    }
}