using BankingControlAPI.CustomExceptions;
using Humanizer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BankingControlAPI.ExceptionHandlers
{
    public class NotFoundExceptionHandler(ILogger<NotFoundExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is NotFoundException)
            {
                logger.LogError(exception, "{ExceptionName}", exception.GetType().Name.Humanize());

                httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;

                var problem = new ProblemDetails
                {
                    Status = httpContext.Response.StatusCode,
                    Type = exception.GetType().Name,
                    Title = exception.Message,
                    Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}"
                };

                await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken: cancellationToken);

                return true;
            }

            return false;
        }
    }
}
