using BankingControlAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Debug;
using Serilog;

namespace BankingControlAPI.Extensions
{
    public static class ContextsExtensions
    {
        public static IServiceCollection AddDbContexts(this IServiceCollection services)
        {
            Log.Logger.Information("Setting Db Context");

            services.AddDbContext<BankingDbContext>((sp, options) =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

                options.UseLoggerFactory(new LoggerFactory(
                [
                    new DebugLoggerProvider()
                ]));
            });

            return services;
        }
    }
}
