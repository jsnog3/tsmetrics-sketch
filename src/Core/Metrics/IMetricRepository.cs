namespace Core.Metrics;

public interface IMetricRepository
{
    Task<MetricAggregation> GetMetricAggregation(Aggregation aggregation, CancellationToken cancellationToken = default);
}