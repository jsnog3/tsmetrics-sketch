using Core.Metrics;
using FluentValidation;
using TSMetricsAPI.Contracts;
using TSMetricsAPI.Contracts.Metrics;
using TSMetricsAPI.Mappings;

namespace TSMetricsAPI.Endpoints;

public static class MetricEndpoints
{
    public static void RegisterMetricsEndpoint(this WebApplication app)
    {
        app.MapGet("/metrics", GetAggregationMetrics)
            .RequireRateLimiting("global")
            .Produces<MetricResponse>(StatusCodes.Status200OK)
            .ProducesValidationProblem();
    }

    public static async Task<IResult> GetAggregationMetrics(
        [AsParameters] MetricRequest request,
        IMetricRepository metricRepository,
        IValidator<MetricRequest> validator,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        var aggregation = request.ToModel();
        var result = await metricRepository.GetMetricAggregation(aggregation, cancellationToken);
        var response = result.ToContract(aggregation);

        return Results.Ok(response);
    }
}