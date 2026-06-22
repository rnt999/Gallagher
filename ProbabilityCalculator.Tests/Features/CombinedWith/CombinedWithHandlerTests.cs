using FluentAssertions;
using ProbabilityCalculator.Api.Features.CombinedWith;

namespace ProbabilityCalculator.Tests.Features.CombinedWith;

public class CombinedWithHandlerTests
{
    [Theory]
    [InlineData(0.5, 0.5, 0.25)]
    [InlineData(0, 1, 0)]
    [InlineData(1, 1, 1)]
    [InlineData(0.3, 0.7, 0.21)]
    public async Task Handle_ReturnsCorrectResult(decimal a, decimal b, decimal expected)
    {
        var result = await CombinedWithHandler.Handle(new CombinedWithRequest(a, b), CancellationToken.None);

        result.Result.Should().Be(expected);
    }

    [Theory]
    [InlineData(0.5, 0.5)]
    [InlineData(0, 1)]
    public async Task Handle_ReturnsFormula(decimal a, decimal b)
    {
        var result = await CombinedWithHandler.Handle(new CombinedWithRequest(a, b), CancellationToken.None);

        result.Formula.Should().NotBeNullOrEmpty();
    }
}
