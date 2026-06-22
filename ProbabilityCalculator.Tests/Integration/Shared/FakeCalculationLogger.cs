using ProbabilityCalculator.Api.Infrastructure.Logging;

namespace ProbabilityCalculator.Tests.Integration.Shared;

public class FakeCalculationLogger : ICalculationLogger
{
    public List<(string Operation, string CorrelationId)> Entries { get; } = [];

    public Task LogAsync<TRequest, TResponse>(
        string operationName, string correlationId, TRequest request, TResponse response)
    {
        Entries.Add((operationName, correlationId));
        return Task.CompletedTask;
    }
}
