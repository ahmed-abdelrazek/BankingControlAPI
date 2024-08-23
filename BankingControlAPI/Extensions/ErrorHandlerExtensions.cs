using BankingControlAPI.ExceptionHandlers;

namespace BankingControlAPI.Extensions
{
    public static class ErrorHandlerExtensions
    {
        public static IServiceCollection AddErrorHandlers(this IServiceCollection services)
        {
            services.AddExceptionHandler<ForbiddenActionExceptionHandler>();
            services.AddExceptionHandler<InvalidOperationExceptionHandler>();
            services.AddExceptionHandler<UnauthorizedAccessExceptionHandler>();
            services.AddExceptionHandler<DefaultExceptionHandler>();

            return services;
        }

        public static IApplicationBuilder UseErrorHandlers(this IApplicationBuilder builder)
        {
            return builder.UseExceptionHandler(opt => { });
        }

        public static Exception GetInnerException(this Exception ex)
        {
            if (ex.InnerException is null)
            {
                return ex;
            }

            do
            {
                ex = ex.InnerException;
            }
            while (ex.InnerException is { });

            return ex;
        }
    }
}
