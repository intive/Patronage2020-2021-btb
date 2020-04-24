using Microsoft.Extensions.DependencyInjection;
using BTB.Domain.Policies;

namespace BTB.Persistence.Configurations.Policy
{
    public static class PoliciesConfiguration
    {
        public static IServiceCollection AddPoliciesConfiguration(this IServiceCollection services)
        {
            services.AddAuthorizationCore(config =>
            {
                config.AddPolicy(Policies.IsAdmin, Policies.IsAdminPolicy());
                config.AddPolicy(Policies.IsUser, Policies.IsUserPolicy());
            });
            return services;
        }
    }
}
