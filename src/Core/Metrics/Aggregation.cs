namespace Core.Metrics;

public enum AggregationGranularity
{
    FiveMinutes,
    ThirtyMinutes,
    SixHours
}

public class Aggregation(
    string abTestName,
    string metricName,
    DateTimeOffset start,
    DateTimeOffset end,
    AggregationGranularity granularity,
    int skip,
    int limit)
{
    public string AbTestName { get; set; } = abTestName;
    public string MetricName { get; set; } = metricName;
    public DateTimeOffset Start{ get; set; } = start;
    public DateTimeOffset End{ get; set; } = end;
    public AggregationGranularity Granularity{ get; set; } = granularity;
    public int Skip { get; set; } = skip;
    public int Limit { get; set; } = limit;
}