using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ProbabilityCalculator.Api.Features.Either;
using ProbabilityCalculator.Tests.Integration.Shared;

namespace ProbabilityCalculator.Tests.Integration;

public class EitherPipelineTests : IClassFixture<CalculationFixture>
{
    private readonly CalculationFixture _fixture;

    public EitherPipelineTests(CalculationFixture fixture) => _fixture = fixture;

    [Fact]
    public void Validator_ResolvesFromDI()
    {
        _fixture.Sp.GetRequiredService<IValidator<EitherRequest>>()
            .Should().BeOfType<EitherValidator>();
    }

    [Fact]
    public async Task ValidRequest_HandlerProducesCorrectResult()
    {
        var result = await EitherHandler.Handle(
            new EitherRequest(0.5m, 0.5m), CancellationToken.None);

        result.Result.Should().Be(0.75m);
    }

    [Fact]
    public void InvalidRequest_ValidatorRejectsInput()
    {
        var validator = _fixture.Sp.GetRequiredService<IValidator<EitherRequest>>();

        var validation = validator.Validate(new EitherRequest(0.5m, 1.1m));

        validation.IsValid.Should().BeFalse();
    }
}
