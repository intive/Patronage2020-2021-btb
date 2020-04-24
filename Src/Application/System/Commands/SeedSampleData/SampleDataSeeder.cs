using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using BTB.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;

namespace BTB.Application.System.SeedSampleData
{
    public class SampleDataSeeder
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public SampleDataSeeder(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task SeedAllAsync(CancellationToken cancellationToken)
        {
            await SeedRolesAsync(cancellationToken);

            var adminEmail = _configuration["MainAdmin:email"];
            var user = await _userManager.FindByEmailAsync(adminEmail);
            if (user == null)
            {
                await SeedAdminAsync(cancellationToken);
            }
        }

        private async Task SeedRolesAsync(CancellationToken cancellationToken)
        {
            var roles = _configuration.GetSection("BasicRoles").Get<List<string>>();
            foreach (string role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    var identityRole = new IdentityRole
                    {
                        Name = role
                    };
                    await _roleManager.CreateAsync(identityRole);
                }
            }
        }

        private async Task SeedAdminAsync(CancellationToken cancellationToken)
        {
            var user = new ApplicationUser
            {
                UserName = _configuration["MainAdmin:username"],
                Email = _configuration["MainAdmin:email"]
            };
            await _userManager.CreateAsync(user, _configuration["MainAdmin:password"]);
            await _userManager.AddToRoleAsync(user, "Admin");
        }

    }
}