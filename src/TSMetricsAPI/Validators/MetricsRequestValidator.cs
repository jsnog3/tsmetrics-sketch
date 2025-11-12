using FluentValidation;
using TSMetricsAPI.Contracts;
using TSMetricsAPI.Contracts.Metrics;

namespace TSMetricsAPI.Validators;

public class MetricsRequestValidator : AbstractValidator<MetricRequest>
{
    public MetricsRequestValidator()
    {
        RuleFor(request => request.AbTest)
            .NotNull()
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(request => request.Metric)
            .NotNull()
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(request => request.Granularity)
            .NotEmpty()
            .Must(IsSupportedGranularity)
            .WithMessage("Granularity must be one of: 5m, 30m, 6h.");

        RuleFor(request => request.End)
            .GreaterThan(request => request.Start!.Value)
            .When(request => request.Start.HasValue && request.End.HasValue);

        RuleFor(request => request.Limit)
            .GreaterThan(0)
            .LessThanOrEqualTo(10000);

        RuleFor(request => request.Skip)
            .GreaterThanOrEqualTo(0);
    }

    private static bool IsSupportedGranularity(string? granularity) =>
        granularity?.Trim().ToLowerInvariant() is "5m" or "30m" or "6h";
}