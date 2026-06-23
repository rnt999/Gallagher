using System.Diagnostics;
using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using ProbabilityCalculator.Api.Infrastructure.Logging;
using ProbabilityCalculator.Api.Shared;

namespace ProbabilityCalculator.Api.Features.Either;

public class EitherFunction(IValidator<EitherRequest> validator, ICalculationLogger logger)
{
    [Function("either")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v1/either")]
        HttpRequest req)
    {
        try
        {
            var body = await req.ReadFromJsonAsync<EitherRequest>(req.HttpContext.RequestAborted);
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

            var result = await EitherHandler.Handle(body, req.HttpContext.RequestAborted);

            var correlationId = Activity.Current?.Id ?? Guid.NewGuid().ToString();
            await logger.LogAsync(nameof(EitherRequest), correlationId, body, result);

            return new OkObjectResult(result);
        }
        catch (JsonException)
        {
            return new BadRequestObjectResult(new { error = "Invalid JSON payload" });
        }
    }
}
