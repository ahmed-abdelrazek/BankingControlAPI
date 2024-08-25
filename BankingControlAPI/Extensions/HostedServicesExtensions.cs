using BankingControlAPI.HostedServices;
using Serilog;

namespace BankingControlAPI.Extensions
{
    public static class HostedServicesExtensions
    {
        /// <summary>
        /// Add the database Context using Sql Server with logs
        /// </summary>
        public static IServiceCollection AddHostedServices(this IServiceCollection services)
        {
            Log.Logger.Information("Setting Hosted Services");

            services.AddHostedService<DbMigrationHostedService>();
            services.AddHostedService<QueuedHostedService>();

            return services;
        }
    }
}
