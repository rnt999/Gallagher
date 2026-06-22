using FluentValidation.TestHelper;
using ProbabilityCalculator.Api.Features.CombinedWith;

namespace ProbabilityCalculator.Tests.Features.CombinedWith;

public class CombinedWithValidatorTests
{
    private readonly CombinedWithValidator _validator = new();

    [Theory]
    [InlineData(0, 0)]
    [InlineData(0.5, 0.5)]
    [InlineData(1, 1)]
    public void ValidInput_NoErrors(decimal a, decimal b) =>
        _validator.TestValidate(new CombinedWithRequest(a, b)).ShouldNotHaveAnyValidationErrors();

    [Theory]
    [InlineData(-0.1, 0.5)]
    [InlineData(1.1, 0.5)]
    public void InvalidA_FailsOnA(decimal a, decimal b) =>
        _validator.TestValidate(new CombinedWithRequest(a, b))
                  .ShouldHaveValidationErrorFor(x => x.ProbabilityA);

    [Theory]
    [InlineData(0.5, -0.1)]
    [InlineData(0.5, 1.1)]
    public void InvalidB_FailsOnB(decimal a, decimal b) =>
        _validator.TestValidate(new CombinedWithRequest(a, b))
                  .ShouldHaveValidationErrorFor(x => x.ProbabilityB);
}
