using FluentAssertions;
using ProbabilityCalculator.Api.Features.Either;

namespace ProbabilityCalculator.Tests.Features.Either;

public class EitherHandlerTests
{
    [Theory]
    [InlineData(0.5, 0.5, 0.75)]
    [InlineData(0, 0, 0)]
    [InlineData(1, 1, 1)]
    [InlineData(0.3, 0.7, 0.79)]
    public async Task Handle_ReturnsCorrectResult(decimal a, decimal b, decimal expected)
    {
        var result = await EitherHandler.Handle(new EitherRequest(a, b), CancellationToken.None);

        result.Result.Should().Be(expected);
    }

    [Theory]
    [InlineData(0.5, 0.5)]
    [InlineData(0, 1)]
    public async Task Handle_ReturnsFormula(decimal a, decimal b)
    {
        var result = await EitherHandler.Handle(new EitherRequest(a, b), CancellationToken.None);

        result.Formula.Should().NotBeNullOrEmpty();
    }
}
