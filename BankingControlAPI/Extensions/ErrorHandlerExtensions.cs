using BankingControlAPI.ExceptionHandlers;

namespace BankingControlAPI.Extensions
{
    public static class ErrorHandlerExtensions
    {
        public static IServiceCollection AddErrorHandlers(this IServiceCollection services)
        {
            services.AddExceptionHandler<DefaultExceptionHandler>();

            return services;
        }

        public static IApplicationBuilder UseErrorHandlers(this IApplicationBuilder builder)
        {
            return builder.UseExceptionHandler(opt => { });
        }
    }
}
