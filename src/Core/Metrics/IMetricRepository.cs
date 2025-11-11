namespace Core.Metrics;

public interface IMetricRepository
{
    Task<IEnumerable<Metric>> GetMetricAggregation(Aggregation aggregation, CancellationToken cancellationToken = default);
}