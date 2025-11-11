namespace TSMetricsAPI.Contracts.Metrics;

public record MetricResponse(
    string AbTestName,
    string MetricName,
    string Unit,
    IReadOnlyList<MetricDatapoint> Buckets);