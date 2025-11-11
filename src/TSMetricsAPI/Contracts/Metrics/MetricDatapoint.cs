namespace TSMetricsAPI.Contracts.Metrics;

public record MetricDatapoint(
    DateTimeOffset Time,
    double Average,
    double Minimum,
    double Maximum);

