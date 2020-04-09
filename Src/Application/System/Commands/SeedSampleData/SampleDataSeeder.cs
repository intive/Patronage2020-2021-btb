using BTB.Application.Common.Interfaces;
using BTB.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Application.System.SeedSampleData
{
    public class SampleDataSeeder
    {
        private readonly IBTBDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public SampleDataSeeder(IBTBDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task SeedAllAsync(CancellationToken cancellationToken)
        {
            if (_userManager.Users.Any())
            {
                return;
            }

            await SeedAspNetUsersAsync(cancellationToken);
        }

        private async Task SeedAspNetUsersAsync(CancellationToken cancellationToken)
        {
            var user = new ApplicationUser
            {
                UserName = "admin"
            };
            await _userManager.CreateAsync(user, "admin");
        }
    }
}