using Core.Metrics;
using FluentValidation;
using Microsoft.OpenApi.Models;
using TSMetricsAPI.Contracts.Metrics;
using TSMetricsAPI.Mappings;

namespace TSMetricsAPI.Endpoints;

public static class MetricEndpoints
{
    public static void RegisterMetricsEndpoint(this WebApplication app)
    {
        app.MapGet("/metrics", GetAggregationMetrics)
            .RequireRateLimiting("global")
            .WithName("GetMetrics")
            .WithTags("Metrics")
            .WithOpenApi(operation => new OpenApiOperation(operation)
            {
                Summary = "Retrieve aggregated A/B test metrics",
                Description = "Returns paginated metric buckets across the supported granularities"
            })
            .Produces<MetricResponse>(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status429TooManyRequests)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        app.MapGet("/metrics/capabilities", () => new[] { "response_time", "cpu_usage", "ram_usage" })
            .WithName("GetSupportedMetrics")
            .WithTags("Metrics")
            .WithOpenApi(operation => new OpenApiOperation(operation)
            {
                Summary = "Retrieve supported metrics"
            });
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