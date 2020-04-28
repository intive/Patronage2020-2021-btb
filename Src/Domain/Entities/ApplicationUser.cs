using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace BTB.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public virtual IList<Alert> Alerts { get; set; } = new List<Alert>();
        public virtual UserProfileInfo ProfileInfo { get; set; }
        public virtual IEnumerable<FavoriteSymbolPair> FavoritePairs { get; set; } = new List<FavoriteSymbolPair>();
        public virtual GamblePoint GamblePoints { get; set; }
    }
}
