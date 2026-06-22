using FluentValidation;

namespace ProbabilityCalculator.Api.Features.Either;

public class EitherValidator : AbstractValidator<EitherRequest>
{
    public EitherValidator()
    {
        RuleFor(x => x.ProbabilityA)
            .InclusiveBetween(0, 1)
            .WithMessage("Probability A must be between 0 and 1.");

        RuleFor(x => x.ProbabilityB)
            .InclusiveBetween(0, 1)
            .WithMessage("Probability B must be between 0 and 1.");
    }
}
