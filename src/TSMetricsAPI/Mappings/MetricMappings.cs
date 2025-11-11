using System;
using Core.Metrics;
using TSMetricsAPI.Contracts;
using TSMetricsAPI.Contracts.Metrics;

namespace TSMetricsAPI.Mappings;

public static class MetricMappings
{
    public static MetricResponse ToContract(this MetricAggregation result, Aggregation aggregation)
    {
        var unit = result.Metrics.FirstOrDefault()?.Unit ?? string.Empty;
        var datapoint = result.Metrics.Select(ToDatapoint).ToList();

        return new MetricResponse(
            aggregation.AbTestName,
            aggregation.MetricName,
            unit,
            aggregation.Skip,
            aggregation.Limit,
            result.ReturnedCount,
            result.IsComplete,
            datapoint);
    }

    public static Aggregation ToModel(this MetricRequest request)
    {
        return new Aggregation(
            request.AbTestName,
            request.MetricName,
            request.Start ?? DateTimeOffset.UtcNow.AddDays(-3),
            request.End ?? DateTimeOffset.UtcNow,
            ParseGranularity(request.Granularity),
            request.Skip,
            request.Limit);
    }

    private static MetricDatapoint ToDatapoint(Metric metric) =>
        new(
            metric.Time.ToUnixTimeSeconds(),
            Math.Round(metric.Average, 3),
            Math.Round(metric.Minimum, 3),
            Math.Round(metric.Maximum, 3));

    private static AggregationGranularity ParseGranularity(string? granularity)
    {
        return granularity?.Trim().ToLowerInvariant() switch
        {
            "30m" => AggregationGranularity.ThirtyMinutes,
            "6h" => AggregationGranularity.SixHours,
            _ => AggregationGranularity.FiveMinutes
        };
    }

    private static string ToContractValue(this AggregationGranularity granularity)
    {
        return granularity switch
        {
            AggregationGranularity.ThirtyMinutes => "30m",
            AggregationGranularity.SixHours => "6h",
            _ => "5m"
        };
    }
}