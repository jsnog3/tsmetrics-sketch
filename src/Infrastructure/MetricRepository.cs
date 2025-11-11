using System.Data;
using System.Data.Common;
using Core.Metrics;
using Dapper;

namespace Infrastructure;

public class MetricRepository : IMetricRepository
{
    private readonly IDbConnection _connection;

    public MetricRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<IEnumerable<Metric>> GetMetricAggregation(Aggregation aggregation, CancellationToken cancellationToken = default)
    {
        var viewName = ResolveViewName(aggregation.Granularity);

        var sql = $"""
                    SELECT bucket_start AS "Time",
                           avg_value   AS "Average",
                           min_value   AS "Minimum",
                           max_value   AS "Maximum",
                           unit        AS "Unit"
                    FROM {viewName}
                    WHERE test_name = @AbTestName
                      AND metric_name = @MetricName
                      AND bucket_start >= @Start
                      AND bucket_start <= @End
                    ORDER BY bucket_start;
                    """;
        
        if (_connection is DbConnection db && db.State != ConnectionState.Open)
        {
            await db.OpenAsync(cancellationToken);
        }

        var parameters = new
        {
            AbTestName = aggregation.AbTestName,
            MetricName = aggregation.MetricName,
            Start = aggregation.Start,
            End = aggregation.End
        };

        var command = new CommandDefinition(sql, parameters, cancellationToken: cancellationToken);
        var rows = await _connection.QueryAsync<Metric>(command);
        return rows.AsList();
    }

    private static string ResolveViewName(AggregationGranularity granularity) => granularity switch
    {
        AggregationGranularity.ThirtyMinutes => "abtest_metrics_30m",
        AggregationGranularity.SixHours => "abtest_metrics_6h",
        _ => "abtest_metrics_5m"
    };
}