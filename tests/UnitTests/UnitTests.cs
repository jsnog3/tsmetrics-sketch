using FluentValidation;
using FluentValidation.Results;
using Moq;
using NUnit.Framework;
using Core.Metrics;
using TSMetricsAPI.Contracts.Metrics;
using TSMetricsAPI.Endpoints;
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
    }
    
    [Test]
    public async Task GetAggregationMetrics_WhenRequestValid_CallsRepository()
    {
        var request = new MetricRequest("feature_a", "response_time"); 
        
        var validatorMock = new Mock<IValidator<MetricRequest>>();
        validatorMock
            .Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        var repositoryMock = new Mock<IMetricRepository>();
        repositoryMock
            .Setup(r => r.GetMetricAggregation(It.IsAny<Aggregation>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new MetricAggregation(true, 0, new List<Metric>()));

        await MetricEndpoints.GetAggregationMetrics(request, repositoryMock.Object, validatorMock.Object, CancellationToken.None);

        repositoryMock.Verify(r => r.GetMetricAggregation(
            It.IsAny<Aggregation>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }
}
