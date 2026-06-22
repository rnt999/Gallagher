using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ProbabilityCalculator.Api.Features.CombinedWith;
using ProbabilityCalculator.Tests.Integration.Shared;

namespace ProbabilityCalculator.Tests.Integration;

public class CombinedWithPipelineTests : IClassFixture<CalculationFixture>
{
    private readonly CalculationFixture _fixture;

    public CombinedWithPipelineTests(CalculationFixture fixture) => _fixture = fixture;

    [Fact]
    public void Validator_ResolvesFromDI()
    {
        _fixture.Sp.GetRequiredService<IValidator<CombinedWithRequest>>()
            .Should().BeOfType<CombinedWithValidator>();
    }

    [Fact]
    public async Task ValidRequest_HandlerProducesCorrectResult()
    {
        var result = await CombinedWithHandler.Handle(
            new CombinedWithRequest(0.5m, 0.5m), CancellationToken.None);

        result.Result.Should().Be(0.25m);
    }

    [Fact]
    public void InvalidRequest_ValidatorRejectsBeforeHandlerReaches()
    {
        var validator = _fixture.Sp.GetRequiredService<IValidator<CombinedWithRequest>>();

        var validation = validator.Validate(new CombinedWithRequest(-0.1m, 0.5m));

        validation.IsValid.Should().BeFalse();
        validation.Errors.Should().Contain(e => e.ErrorMessage.Contains("Probability A"));
    }

    [Fact]
    public void BothInputsInvalid_AllErrorsReported()
    {
        var validator = _fixture.Sp.GetRequiredService<IValidator<CombinedWithRequest>>();

        var validation = validator.Validate(new CombinedWithRequest(-0.1m, 1.1m));

        validation.Errors.Should().HaveCount(2);
    }
}
