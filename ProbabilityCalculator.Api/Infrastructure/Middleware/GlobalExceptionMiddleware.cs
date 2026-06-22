using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;

namespace ProbabilityCalculator.Api.Infrastructure.Middleware;

public class GlobalExceptionMiddleware : IFunctionsWorkerMiddleware
{
    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            var logger = context.GetLogger<GlobalExceptionMiddleware>();
            logger.LogError(ex, "Unhandled exception in function {FunctionName}", context.FunctionDefinition.Name);

            var request = await context.GetHttpRequestDataAsync();
            if (request is null)
                throw;

            var response = request.CreateResponse(HttpStatusCode.InternalServerError);
            await response.WriteAsJsonAsync(new { error = "An unexpected error occurred" });

            context.GetInvocationResult().Value = response;
        }
    }
}
