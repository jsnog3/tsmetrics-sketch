using FluentValidation;
using TSMetricsAPI.Contracts;
using TSMetricsAPI.Contracts.Metrics;
using TSMetricsAPI.Validators;

namespace TSMetricsAPI.Extensions;

public static class ValidatorExtensions
{
    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddTransient<IValidator<MetricRequest>, MetricsRequestValidator>();
        return services;
    }
}