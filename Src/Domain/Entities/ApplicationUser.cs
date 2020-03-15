using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BTB.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public IList<Alert> Alerts { get; set; } = new List<Alert>();
        public UserProfileInfo ProfileInfo { get; set; }
    }
}
