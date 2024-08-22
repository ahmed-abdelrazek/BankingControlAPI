using Humanizer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BankingControlAPI.ExceptionHandlers
{
    public class DefaultExceptionHandler(ILogger<DefaultExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            logger.LogError(exception, "{ExceptionName}", exception.GetType().Name.Humanize());

            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var problem = new ProblemDetails
            {
                Status = httpContext.Response.StatusCode,
                Type = exception.GetType().Name,
                Title = HttpStatusCode.InternalServerError.Humanize(),
                Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}"
            };
#if DEBUG
            problem.Detail = string.IsNullOrEmpty(exception.Message) ? null : exception.Message;
#endif
            await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken: cancellationToken);

            return true;
        }
    }
}
