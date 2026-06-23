using System.Diagnostics;
using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using ProbabilityCalculator.Api.Infrastructure.Logging;

namespace ProbabilityCalculator.Api.Features.CombinedWith;

public class CombinedWithFunction(IValidator<CombinedWithRequest> validator, ICalculationLogger logger)
{
    [Function("combined-with")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v1/combined-with")]
        HttpRequest req)
    {
        try
        {
            var body = await req.ReadFromJsonAsync<CombinedWithRequest>(req.HttpContext.RequestAborted);
            if (body is null)
                return new BadRequestObjectResult(new { error = "Invalid request body" });

            var validation = validator.Validate(body);
            if (!validation.IsValid)
            {
                return new BadRequestObjectResult(new
                {
                    error = "Validation failed",
                    errors = validation.Errors.Select(e => e.ErrorMessage)
                });
            }

            var result = await CombinedWithHandler.Handle(body, req.HttpContext.RequestAborted);

            var correlationId = Activity.Current?.Id ?? Guid.NewGuid().ToString();
            await logger.LogAsync(nameof(CombinedWithRequest), correlationId, body, result);

            return new OkObjectResult(result);
        }
        catch (JsonException)
        {
            return new BadRequestObjectResult(new { error = "Invalid JSON payload" });
        }
    }
}
