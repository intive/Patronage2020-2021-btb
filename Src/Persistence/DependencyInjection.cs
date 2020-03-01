using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace BTB.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            /*
                services.AddScoped<IDbContext, DbContext>();

                services.AddDbContext<DbContext>(options =>
                {
                    options.UseSqlServer(ConnectionString);
                });
            */

            return services;
        }
    }
}