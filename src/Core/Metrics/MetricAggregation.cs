namespace Core.Metrics;

public class MetricAggregation(bool isComplete, int returnedCount, IList<Metric> metrics)
{
    public bool IsComplete { get; set; } = isComplete;
    public int ReturnedCount { get; set; } = returnedCount;
    public IList<Metric> Metrics { get; set; } = metrics;
}