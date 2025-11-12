using Microsoft.OpenApi.Models;

namespace TSMetricsAPI.Endpoints;

public static class AbTestEndpoints
{
    public static void RegisterAbTestEndpoints(this WebApplication app)
    {
        app.MapGet("/abtest", () => new[] { "feature_a", "feature_b" })
            .WithName("GetABTests")
            .WithTags("ABTest")
            .WithOpenApi(operation => new OpenApiOperation(operation)
            {
                Summary = "Retrieve supported ABTests"
            });
    }    
}