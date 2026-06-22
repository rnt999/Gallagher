using FluentValidation.TestHelper;
using ProbabilityCalculator.Api.Features.Either;

namespace ProbabilityCalculator.Tests.Features.Either;

public class EitherValidatorTests
{
    private readonly EitherValidator _validator = new();

    [Theory]
    [InlineData(0, 0)]
    [InlineData(0.5, 0.5)]
    [InlineData(1, 1)]
    public void ValidInput_NoErrors(decimal a, decimal b) =>
        _validator.TestValidate(new EitherRequest(a, b)).ShouldNotHaveAnyValidationErrors();

    [Theory]
    [InlineData(-0.1, 0.5)]
    [InlineData(1.1, 0.5)]
    public void InvalidA_FailsOnA(decimal a, decimal b) =>
        _validator.TestValidate(new EitherRequest(a, b))
                  .ShouldHaveValidationErrorFor(x => x.ProbabilityA);

    [Theory]
    [InlineData(0.5, -0.1)]
    [InlineData(0.5, 1.1)]
    public void InvalidB_FailsOnB(decimal a, decimal b) =>
        _validator.TestValidate(new EitherRequest(a, b))
                  .ShouldHaveValidationErrorFor(x => x.ProbabilityB);
}
