namespace TSMetricsAPI.Contracts.Metrics;

public record MetricResponse(
    string AbTestName,
    string MetricName,
    string Unit,
    int Skip,
    int Limit,
    int Count,
    bool IsComplete,
    IReadOnlyList<MetricDatapoint> Buckets);