using BTB.Application.Common.Interfaces;
using BTB.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using MediatR;
using BTB.Domain.Enums;

namespace BTB.Application.System.Commands.SeedSampleData
{
    public class SeedSampleDataCommandHandler : IRequestHandler<SeedSampleDataCommand>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IBTBDbContext _context;
        private readonly IGamblePointManager _gamblePointManager;

        private const decimal NumberOfPointsToAddToNewUser = 1000;

        public SeedSampleDataCommandHandler(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, IBTBDbContext context, IGamblePointManager gamblePointManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _context = context;
            _gamblePointManager = gamblePointManager;
        }

        public async Task<Unit> Handle(SeedSampleDataCommand request, CancellationToken cancellationToken)
        {
            await SeedRolesAsync(cancellationToken); // this line has to be before the one with seeding admin
            bool doesAdminExist = await DoesAdminExistAsync(cancellationToken);
            if (false == doesAdminExist)
            {
                await SeedAdminAsync(cancellationToken);
            }
            await SeedTemplateAsync(cancellationToken);

            return Unit.Value;
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

        public async Task<bool> DoesAdminExistAsync(CancellationToken cancellationToken)
        {
            bool doesAdminExist;

            var userByEmail = await _userManager.FindByEmailAsync(_configuration["MainAdmin:email"]);
            var userByUsername = await _userManager.FindByNameAsync(_configuration["MainAdmin:username"]);
            if (userByEmail == null && userByUsername == null)
            {
                doesAdminExist = false;
            }
            else if (userByEmail == null && userByUsername != null)
            {
                await _userManager.DeleteAsync(userByUsername);
                doesAdminExist = false;
            }
            else if (userByEmail != null && userByUsername == null)
            {
                await _userManager.DeleteAsync(userByEmail);
                doesAdminExist = false;
            }
            else
            {
                if (userByEmail == userByUsername)
                {
                    var roles = await _userManager.GetRolesAsync(userByEmail);
                    if (roles.Contains("Admin"))
                    {
                        doesAdminExist = true;
                    }
                    else
                    {
                        await _userManager.DeleteAsync(userByUsername);
                        doesAdminExist = false;
                    }
                }
                else
                {
                    await _userManager.DeleteAsync(userByUsername);
                    await _userManager.DeleteAsync(userByEmail);
                    doesAdminExist = false;
                }
            }

            return doesAdminExist;
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
            await _gamblePointManager.InitGamblePoints(user.UserName, NumberOfPointsToAddToNewUser, cancellationToken);
            
            var profileInfo = new UserProfileInfo()
            {
                UserId = (await _userManager.FindByNameAsync(user.UserName)).Id,
                Username = "admin"
            };
            await _context.UserProfileInfo.AddAsync(profileInfo, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task SeedTemplateAsync(CancellationToken cancellationToken)
        {
            if (!await _context.EmailTemplates.AnyAsync())
            {
                var template = new EmailTemplate
                {
                    Content = MailTemplate.GetTemplate()
                };
                _context.EmailTemplates.Add(template);
                await _context.SaveChangesAsync(cancellationToken);
            }

            if (!await _context.AlertMessageTemplates.AnyAsync())
            {
                _context.AlertMessageTemplates.AddRange(AlertMessageTemplates.GetTemplates());
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
