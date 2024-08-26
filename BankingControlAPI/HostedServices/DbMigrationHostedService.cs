using BankingControlAPI.Data;
using BankingControlAPI.Domain.Enums;
using BankingControlAPI.Domain.System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BankingControlAPI.HostedServices
{
    public class DbMigrationHostedService(IServiceProvider services, ILogger<DbMigrationHostedService> logger) : IHostedService
    {
        public async Task StartAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("""{Name} is running.""", nameof(DbMigrationHostedService));

            await DoMigrationAndSeeding(stoppingToken);
        }

        private async Task DoMigrationAndSeeding(CancellationToken stoppingToken)
        {
            using var scope = services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<BankingDbContext>();
            var maintenanceWindow = scope.ServiceProvider.GetRequiredService<MaintenanceWindow>();

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

            try
            {
                maintenanceWindow.Status = MaintenanceStatus.DbMigration;

                logger.LogWarning("Try Migrating the Db");

                var pendingdbTenantsDbMigrations = await dbContext.Database.GetPendingMigrationsAsync(cancellationToken: stoppingToken);
                if (pendingdbTenantsDbMigrations.Any())
                {
                    await dbContext.Database.MigrateAsync(cancellationToken: stoppingToken);
                }

                if (!await dbContext.Roles.AnyAsync(cancellationToken: stoppingToken))
                {
                    var adminRole = new IdentityRole
                    {
                        Name = nameof(RoleEnum.Admin),
                    };
                    var rslt = await roleManager.CreateAsync(adminRole);

                    if (!rslt.Succeeded)
                    {
                        throw new Exception("Couldn't create Admin Role.");
                    }

                    var clientRole = new IdentityRole
                    {
                        Name = nameof(RoleEnum.Client),
                    };
                    rslt = await roleManager.CreateAsync(clientRole);

                    if (!rslt.Succeeded)
                    {
                        throw new Exception("Couldn't create Client Role.");
                    }
                }

                if (!await dbContext.Users.AnyAsync(cancellationToken: stoppingToken))
                {
                    var admin = new IdentityUser
                    {
                        UserName = "admin",
                        Email = "admin@example.com",
                        EmailConfirmed = true,
                    };
                    var rslt = await userManager.CreateAsync(admin, "Admin@123");

                    if (!rslt.Succeeded)
                    {
                        throw new Exception("Couldn't create Admin user.");
                    }

                    await userManager.AddToRoleAsync(admin, nameof(RoleEnum.Admin));

                    var user = new IdentityUser
                    {
                        UserName = "user@example.com",
                        Email = "user@example.com",
                        EmailConfirmed = true,
                    };

                    rslt = await userManager.CreateAsync(user, "user123");

                    if (!rslt.Succeeded)
                    {
                        throw new Exception("couldn't create Client user.");
                    }

                    await userManager.AddToRoleAsync(user, nameof(RoleEnum.Client));
                }

                logger.LogWarning("Finished Migrating the Db");
                await StopAsync(stoppingToken);

                maintenanceWindow.Status = MaintenanceStatus.None;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Problem migrating the database");
                await StopAsync(stoppingToken);
            }
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("""{Name} is stopping.""", nameof(DbMigrationHostedService));

            return Task.CompletedTask;
        }
    }
}
