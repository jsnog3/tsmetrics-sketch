using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using NUnit.Framework;

namespace IntegrationTests;

public class IntegrationTests
{
    [Test]
    public async Task GetSupportedMetrics_ReturnsKnownCapabilities()
    {
        await using var factory = new WebApplicationFactory<Program>();
        using var client = factory.CreateClient(new() { AllowAutoRedirect = true });

        var response = await client.GetFromJsonAsync<string[]>("/metrics/capabilities");

        Assert.That(response, Does.Contain("response_time"));
    }
}