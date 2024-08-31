using BankingControlAPI.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace BankingControlAPI.Extensions
{
    public static class ContextsExtensions
    {
        /// <summary>
        /// Add the database Context using Sql Server with logs
        /// </summary>
        public static IServiceCollection AddDbContexts(this IServiceCollection services)
        {
            Log.Logger.Information("Setting Db Context");

            services.AddDbContext<BankingDbContext>((sp, options) =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                var loggerFactory = sp.GetRequiredService<ILoggerFactory>();

                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

                options.UseLoggerFactory(loggerFactory);
            });

            return services;
        }
    }
}
