using BankingControlAPI.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Serilog;

namespace BankingControlAPI.Extensions
{
    public static class AuthenticationServicesExtensions
    {
        /// <summary>
        /// Add Microsoft Identity Server and set its option up
        /// </summary>
        public static IServiceCollection AddAuthenticationServices(this IServiceCollection services, IConfiguration configuration)
        {
            Log.Logger.Information("Setting Identity Server");

            var jwtSettings = configuration.GetSection("IdentitySettings");

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityConstants.BearerScheme;
                options.DefaultSignInScheme = IdentityConstants.BearerScheme;
                options.DefaultAuthenticateScheme = IdentityConstants.BearerScheme;
            }).AddBearerToken(IdentityConstants.BearerScheme, options =>
            {
                options.ClaimsIssuer = jwtSettings.GetSection("validIssuer").Value;
                options.BearerTokenExpiration = TimeSpan.FromMinutes(10);
                options.RefreshTokenExpiration = TimeSpan.FromDays(15);
            });

            var defaultPolicy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .Build();

            services.AddAuthorizationBuilder()
                .AddDefaultPolicy("defaultPolicy", defaultPolicy);

            services.AddIdentityCore<IdentityUser>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.RequireUniqueEmail = true;

                //SignIn settings
                options.SignIn.RequireConfirmedEmail = false;
            })
            .AddRoles<IdentityRole>()
            .AddUserManager<UserManager<IdentityUser>>()
            .AddSignInManager<SignInManager<IdentityUser>>()
            .AddRoleManager<RoleManager<IdentityRole>>()
            .AddEntityFrameworkStores<BankingDbContext>()
            .AddApiEndpoints();

            return services;
        }
    }
}
