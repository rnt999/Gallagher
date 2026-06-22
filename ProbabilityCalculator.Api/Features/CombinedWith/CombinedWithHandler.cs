using ProbabilityCalculator.Api.Shared;

namespace ProbabilityCalculator.Api.Features.CombinedWith;

public static class CombinedWithHandler
{
    public static Task<CalculateResponse> Handle(CombinedWithRequest req, CancellationToken ct)
    {
        var result = req.ProbabilityA * req.ProbabilityB;
        return Task.FromResult(new CalculateResponse(
            result,
            $"P(A)×P(B) = {req.ProbabilityA}×{req.ProbabilityB} = {result}"));
    }
}
