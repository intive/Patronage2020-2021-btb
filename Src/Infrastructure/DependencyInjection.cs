using BTB.Common;
using BTB.Application.Common.Interfaces;
using BTB.Infrastructure.Stock.Indicator;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BTB.Infrastructure.Identity;

namespace BTB.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
        {
            services.AddTransient<IDateTime, MachineDateTime>();
            services.AddTransient<IIndicator, Indicator>();
            services.AddScoped<IJwtGenerator, JwtGenerator>();
            return services;
        }
    }
}
