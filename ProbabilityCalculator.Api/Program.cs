using Azure.Monitor.OpenTelemetry.Exporter;
using FluentValidation;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Azure.Functions.Worker.OpenTelemetry;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProbabilityCalculator.Api.Infrastructure.Logging;
using ProbabilityCalculator.Api.Infrastructure.Middleware;

var builder = FunctionsApplication.CreateBuilder(args);

builder.UseMiddleware<GlobalExceptionMiddleware>();
builder.UseMiddleware<CorsPolicyMiddleware>();
builder.ConfigureFunctionsWebApplication();

builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddSingleton<ICalculationLogger, FileCalculationLogger>();

if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("APPLICATIONINSIGHTS_CONNECTION_STRING")))
{
    builder.Services.AddOpenTelemetry()
        .UseFunctionsWorkerDefaults()
        .UseAzureMonitorExporter();
}

await builder.Build().RunAsync();
