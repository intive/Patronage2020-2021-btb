using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BTB.Persistence.Configurations.Policy;
using BTB.Application.Common.Interfaces;
using BTB.Domain.Entities;
using System.Text;
using System;

namespace BTB.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IBTBDbContext, BTBDbContext>();

            services.AddDbContext<BTBDbContext>(options =>
            {                
                options.UseSqlServer(configuration.GetConnectionString("Default"));
            });

            services.AddIdentityCore<ApplicationUser>()
                .AddRoles<IdentityRole>()
                .AddSignInManager<SignInManager<ApplicationUser>>()
                .AddEntityFrameworkStores<BTBDbContext>()
                .AddDefaultTokenProviders();
            services.AddPoliciesConfiguration();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["jwt:key"])),
                    ClockSkew = TimeSpan.Zero
                });

            services.Configure<DataProtectionTokenProviderOptions>(opt => opt.TokenLifespan = TimeSpan.FromHours(2));

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 4;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.RequireUniqueEmail = true;
            });

            return services;
        }
    }
}