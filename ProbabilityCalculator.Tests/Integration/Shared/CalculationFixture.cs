using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ProbabilityCalculator.Api.Features.CombinedWith;
using ProbabilityCalculator.Api.Infrastructure.Logging;

namespace ProbabilityCalculator.Tests.Integration.Shared;

public class CalculationFixture
{
    public ServiceProvider Sp { get; }
    public FakeCalculationLogger Logger { get; } = new();

    public CalculationFixture()
    {
        var services = new ServiceCollection();

        // Mirror Program.cs registrations — verifies the DI scan finds the right validators.
        services.AddValidatorsFromAssemblyContaining<CombinedWithValidator>();
        services.AddSingleton<ICalculationLogger>(Logger);

        Sp = services.BuildServiceProvider();
    }
}
