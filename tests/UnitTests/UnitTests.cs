using NUnit.Framework;
using TSMetricsAPI.Contracts.Metrics;
using TSMetricsAPI.Validators;

namespace UnitTests;

public class UnitTests
{
    [Test]
    public void MetricRequestValidator_WhenGranularityUnsupported_FailsValidation()
    {
        var validator = new MetricsRequestValidator();
        var request = new MetricRequest("feature_a", "response_time", granularity: "10m");

        var result = validator.Validate(request);

        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors.Single().ErrorMessage, Does.Contain("Granularity"));
    }}
