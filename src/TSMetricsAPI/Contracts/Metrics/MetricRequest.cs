using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace TSMetricsAPI.Contracts.Metrics;

[SwaggerSchema("Query parameters for the metrics request.")]
public record MetricRequest
{
    [Required, MaxLength(50)]
    [SwaggerSchema("A/B test identifier (e.g.: feature_a)", Nullable = false)]
    public string AbTest { get; init; }

    [Required, MaxLength(50)]
    [SwaggerSchema("Metric identifier (e.g.: response_time)", Nullable = false)]
    public string Metric { get; init; }

    [SwaggerSchema("Inclusive timestamp for the first datapoint in the aggregation")]
    public DateTimeOffset? Start { get; init; }

    [SwaggerSchema("Inclusive timestamp for the last datapoint. Must be > start.")]
    public DateTimeOffset? End { get; init; }

    [SwaggerSchema("Granularity of the aggregate window (one of: 5m, 30m, 6h).")]
    public string Granularity { get; init; }

    [Range(1, 10_000)]
    [SwaggerSchema("Maximum number of datapoints to return (maximum 10,000).")]
    public int Limit { get; init; }

    [Range(0, int.MaxValue)]
    [SwaggerSchema("Number of datapoints to skip (≥ 0).")]
    public int Skip { get; init; }

    public MetricRequest(
        string abTest,
        string metric,
        DateTimeOffset? start = null,
        DateTimeOffset? end = null,
        string granularity = "5m",
        int limit = 10_000,
        int skip = 0)
    {
        AbTest = abTest;
        Metric = metric;
        Start = start;
        End = end;
        Granularity = granularity;
        Limit = limit;
        Skip = skip;
    }
}