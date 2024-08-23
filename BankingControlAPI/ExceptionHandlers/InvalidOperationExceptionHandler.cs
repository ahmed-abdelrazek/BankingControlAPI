using BankingControlAPI.Extensions;
using Humanizer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BankingControlAPI.ExceptionHandlers
{
    public class InvalidOperationExceptionHandler(ILogger<InvalidOperationExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var error = exception.GetInnerException();

            if (error is InvalidOperationException)
            {
                logger.LogError(error, "{ExceptionName}", error.GetType().Name.Humanize());

                httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                var problem = new ProblemDetails
                {
                    Status = httpContext.Response.StatusCode,
                    Type = error.GetType().Name,
                    Title = error.Message,
                    Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}"
                };

                await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken: cancellationToken);

                return true;
            }

            return false;
        }
    }
}
