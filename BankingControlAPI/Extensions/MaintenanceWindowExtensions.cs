using BankingControlAPI.Domain.Enums;
using BankingControlAPI.Domain.System;
using BankingControlAPI.Middlewares;

namespace BankingControlAPI.Extensions
{
    public static class MaintenanceWindowExtensions
    {
        public static IServiceCollection AddMaintenance(this IServiceCollection services, MaintenanceWindow window)
        {
            services.AddSingleton(window);
            return services;
        }

        public static IServiceCollection AddMaintenance(this IServiceCollection services, MaintenanceStatus status, int retryAfterInSeconds = 3600)
        {
            services.AddMaintenance(new MaintenanceWindow(status)
            {
                RetryAfterInSeconds = retryAfterInSeconds
            });

            return services;
        }

        public static IApplicationBuilder UseMaintenance(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MaintenanceMiddleware>();
        }
    }
}
