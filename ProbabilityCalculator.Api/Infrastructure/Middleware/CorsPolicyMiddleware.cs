using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;

namespace ProbabilityCalculator.Api.Infrastructure.Middleware;

public class CorsPolicyMiddleware : IFunctionsWorkerMiddleware
{
    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        var httpContext = context.GetHttpContext();

        if (httpContext is not null)
        {
            var allowedOrigin = Environment.GetEnvironmentVariable("CORS_ALLOWED_ORIGIN") ?? "http://localhost:5174";
            httpContext.Response.Headers.Append("Access-Control-Allow-Origin", allowedOrigin);
            httpContext.Response.Headers.Append("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
            httpContext.Response.Headers.Append("Access-Control-Allow-Headers", "Content-Type");

            if (httpContext.Request.Method == HttpMethods.Options)
            {
                httpContext.Response.StatusCode = StatusCodes.Status204NoContent;
                return;
            }
        }

        await next(context);
    }
}
