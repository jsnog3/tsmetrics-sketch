namespace Core.Metrics;

public class Metric
{
    public Metric(DateTime time, double average, double minimum, double maximum, string unit)
    {
        Time = new DateTimeOffset(time.ToUniversalTime(), TimeSpan.Zero);
        Average = average;
        Minimum = minimum;
        Maximum = maximum;
        Unit = unit;
    }

    public DateTimeOffset Time { get; set; }
    public double Average { get; set; }
    public double Minimum { get; set; }
    public double Maximum { get; set; }
    public string Unit { get; set; }
}
