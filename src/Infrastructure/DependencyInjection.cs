using System.Data;
using Core.Metrics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("TimescaleDb");

        services.AddScoped<IDbConnection>(_ => new NpgsqlConnection(connectionString));

        services.AddScoped<IMetricRepository, MetricRepository>();

        return services;
    }
}
