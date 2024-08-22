using BankingControlAPI.Domain.Enums;
using BankingControlAPI.Domain.System;
using System.Net;
using System.Text;

namespace BankingControlAPI.Middlewares
{
    /// <summary>
    /// Stops the server from responding to request if database migeration wasn't completed
    /// </summary>
    public class MaintenanceMiddleware(RequestDelegate next, MaintenanceWindow window, ILogger<MaintenanceMiddleware> logger)
    {
        private readonly ILogger logger = logger;
        private readonly string[] whiteList = ["/swagger/index.html"];

        public async Task Invoke(HttpContext context)
        {
            if (window.Status is not MaintenanceStatus.None && !whiteList.Any(x => x == context.Request.Path))
            {
                logger.LogCritical("Site is down for {Status}", window.Status.ToString());
                context.Response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
                context.Response.Headers.Append("Retry-After", window.RetryAfterInSeconds.ToString());
                context.Response.ContentType = "text/html";
            }

            switch (window.Status)
            {
                case MaintenanceStatus.DbMigration:
                    await context.Response
                        .WriteAsync(Encoding.UTF8.GetString(Encoding.UTF8.GetBytes("Setting the database.")), Encoding.UTF8);
                    break;
                case MaintenanceStatus.None:
                default:
                    await next.Invoke(context);
                    break;
            }
        }
    }
}
