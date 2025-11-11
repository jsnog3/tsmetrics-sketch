namespace TSMetricsAPI.Contracts.Metrics;

public record MetricRequest(
    string AbTestName,
    string MetricName,
    DateTimeOffset? Start = null,
    DateTimeOffset? End = null,
    string Granularity = "5m");