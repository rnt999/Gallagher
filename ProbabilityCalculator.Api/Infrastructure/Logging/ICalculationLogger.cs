namespace ProbabilityCalculator.Api.Infrastructure.Logging;

public interface ICalculationLogger
{
    Task LogAsync<TRequest, TResponse>(
        string operationName, string correlationId, TRequest request, TResponse response);
}
