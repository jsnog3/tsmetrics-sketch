using Swashbuckle.AspNetCore.Annotations;

namespace TSMetricsAPI.Contracts.Metrics;

[SwaggerSchema("Aggregated metric response")]
public record MetricResponse
{
    [SwaggerSchema("A/B test identifier")]
    public string AbTest { get; init; }

    [SwaggerSchema("Metric identifier")]
    public string Metric { get; init; }

    [SwaggerSchema("Metric unit for the datapoints")]
    public string Unit { get; init; }

    [SwaggerSchema("Number of results skipped")]
    public int Skip { get; init; }

    [SwaggerSchema("Number of datapoints requested")]
    public int Limit { get; init; }

    [SwaggerSchema("Number of datapoints returned")]
    public int Count { get; init; }

    [SwaggerSchema("Indicates if the datapoints are completed in this response")]
    public bool IsComplete { get; init; }

    [SwaggerSchema("Chronologically ordered datapoints")]
    public IReadOnlyList<MetricDatapoint> Datapoints { get; init; }

    public MetricResponse(
        string abTest,
        string metric,
        string unit,
        int skip,
        int limit,
        int count,
        bool isComplete,
        IReadOnlyList<MetricDatapoint> datapoints)
    {
        AbTest = abTest;
        Metric = metric;
        Unit = unit;
        Skip = skip;
        Limit = limit;
        Count = count;
        IsComplete = isComplete;
        Datapoints = datapoints;
    }
}
