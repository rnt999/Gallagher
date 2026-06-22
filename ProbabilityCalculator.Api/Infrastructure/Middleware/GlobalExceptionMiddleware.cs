using Microsoft.Azure.Functions.Worker;
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
            throw;
        }
    }
}
