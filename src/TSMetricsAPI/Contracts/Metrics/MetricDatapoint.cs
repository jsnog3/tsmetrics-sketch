namespace TSMetricsAPI.Contracts.Metrics;

using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;

[SwaggerSchema("Single aggregated datapoint.")]
public record MetricDatapoint
{
    [JsonPropertyName("t")]
    [SwaggerSchema("Start of the datapoint timestamp in epoch seconds", Format = "int64")]
    public long Time { get; init; }

    [JsonPropertyName("a")]
    [SwaggerSchema("Average metric value for the datapoint")]
    public double Average { get; init; }

    [JsonPropertyName("l")]
    [SwaggerSchema("Minimum observed value for the datapoint")]
    public double Minimum { get; init; }

    [JsonPropertyName("h")]
    [SwaggerSchema("Maximum observed value for the datapoint")]
    public double Maximum { get; init; }

    public MetricDatapoint(long time, double average, double minimum, double maximum)
    {
        Time = time;
        Average = average;
        Minimum = minimum;
        Maximum = maximum;
    }
}
