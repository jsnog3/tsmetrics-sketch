namespace TSMetricsAPI.Contracts.Metrics;

using System.Text.Json.Serialization;

public record MetricDatapoint(
    [property: JsonPropertyName("t")] long Time,
    [property: JsonPropertyName("a")] double Average,
    [property: JsonPropertyName("l")] double Minimum,
    [property: JsonPropertyName("h")] double Maximum);

