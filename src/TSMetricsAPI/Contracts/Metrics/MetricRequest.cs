namespace TSMetricsAPI.Contracts.Metrics;

public record MetricRequest(
    string AbTestName,
    string MetricName,
    DateTimeOffset? Start = null,
    DateTimeOffset? End = null,
    string Granularity = "5m",
    int Limit = 10_000,
    int Skip = 0);