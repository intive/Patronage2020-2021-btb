using BTB.Application.System.SeedSampleData;
using BTB.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Threading;
using MediatR;

namespace BTB.Application.System.Commands.SeedSampleData
{
    public class SeedSampleDataCommand : IRequest
    {
    }

    public class SeedSampleDataCommandHandler : IRequestHandler<SeedSampleDataCommand>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public SeedSampleDataCommandHandler(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task<Unit> Handle(SeedSampleDataCommand request, CancellationToken cancellationToken)
        {
            var seeder = new SampleDataSeeder(_userManager, _roleManager, _configuration);

            await seeder.SeedAllAsync(cancellationToken);

            return Unit.Value;
        }
    }
}