using FluentValidation;

namespace ProbabilityCalculator.Api.Features.CombinedWith;

public class CombinedWithValidator : AbstractValidator<CombinedWithRequest>
{
    public CombinedWithValidator()
    {
        RuleFor(x => x.ProbabilityA)
            .InclusiveBetween(0, 1)
            .WithMessage("Probability A must be between 0 and 1.");

        RuleFor(x => x.ProbabilityB)
            .InclusiveBetween(0, 1)
            .WithMessage("Probability B must be between 0 and 1.");
    }
}
