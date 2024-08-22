using BankingControlAPI.Domain.Enums;
using BankingControlAPI.Extensions;
using FluentValidation;
using FluentValidation.AspNetCore;
using Serilog;

namespace BankingControlAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .CreateBootstrapLogger();

        builder.Host.UseSerilog(Log.Logger);

        // Add services to the container.
        Log.Logger.Information("Setting Cors");
        const string AllowAllHeadersPolicy = "AllowAllHeadersPolicy";
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(AllowAllHeadersPolicy,
                builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyHeader()
                           .WithMethods("GET", "POST", "PUT", "DELETE");
                });
        });

        Log.Logger.Information("Setting AutoMapper");
        builder.Services.AddAutoMapper(typeof(Program).Assembly);

        Log.Logger.Information("Setting Error Handlers");
        builder.Services.AddErrorHandlers();
        builder.Services.AddProblemDetails();

        Log.Logger.Information("Setting MediatR");
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
        });

        Log.Logger.Information("Setting Fluent Validation");
        builder.Services.AddFluentValidationAutoValidation();
        builder.Services.AddValidatorsFromAssemblyContaining<Program>();

        builder.Services.AddDbContexts();
        builder.Services.AddAuthenticationServices(builder.Configuration);

        Log.Logger.Information("Setting Maintenace");
        builder.Services.AddMaintenance(MaintenanceStatus.None);

        Log.Logger.Information("Setting Controllers");
        builder.Services.AddControllers();

        Log.Logger.Information("Setting Swagger");
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddHostedServices();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors(AllowAllHeadersPolicy);

        app.UseMaintenance();

        app.UseErrorHandlers();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
