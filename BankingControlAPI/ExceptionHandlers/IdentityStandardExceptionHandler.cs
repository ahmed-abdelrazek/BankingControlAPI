using BankingControlAPI.CustomExceptions;
using Humanizer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;

namespace BankingControlAPI.ExceptionHandlers
{
    public class IdentityStandardExceptionHandler(ILogger<IdentityStandardExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is IdentityStandardException ex)
            {
                logger.LogError(exception, "{ExceptionName}", exception.GetType().Name.Humanize());

                httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                var problem = new ValidationProblemDetails(ErrorsToDictionaryForValidationProblem(ex.Errors))
                {
                    Status = httpContext.Response.StatusCode,
                    Type = exception.GetType().Name,
                    Title = "User identity standards was not met",
                    Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}"
                };

                await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken: cancellationToken);

                return true;
            }

            return false;
        }

        private static Dictionary<string, string[]> ErrorsToDictionaryForValidationProblem(IEnumerable<IdentityError>? errors)
        {
            // We expect a single error code and description in the normal case.
            Debug.Assert(errors is { } && errors.Any());

            var errorDictionary = new Dictionary<string, string[]>(1);

            foreach (var error in errors)
            {
                string[] newDescriptions;

                if (errorDictionary.TryGetValue(error.Code, out var descriptions))
                {
                    newDescriptions = new string[descriptions.Length + 1];
                    Array.Copy(descriptions, newDescriptions, descriptions.Length);
                    newDescriptions[descriptions.Length] = error.Description;
                }
                else
                {
                    newDescriptions = [error.Description];
                }

                errorDictionary[error.Code] = newDescriptions;
            }

            return errorDictionary;
        }
    }
}
