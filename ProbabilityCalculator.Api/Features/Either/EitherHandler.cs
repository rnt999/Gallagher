using ProbabilityCalculator.Api.Shared;

namespace ProbabilityCalculator.Api.Features.Either;

public static class EitherHandler
{
    public static Task<CalculateResponse> Handle(EitherRequest req, CancellationToken ct)
    {
        var result = req.ProbabilityA + req.ProbabilityB - req.ProbabilityA * req.ProbabilityB;
        return Task.FromResult(new CalculateResponse(
            result,
            $"P(A)+P(B)−P(A)P(B) = {req.ProbabilityA}+{req.ProbabilityB}−{req.ProbabilityA * req.ProbabilityB} = {result}"));
    }
}
