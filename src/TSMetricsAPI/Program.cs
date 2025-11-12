using Infrastructure;
using TSMetricsAPI.Endpoints;
using TSMetricsAPI.Middleware;
using TSMetricsAPI.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "TSMetrics API",
        Version = "v1",
        Description = "Aggregated metrics API for A/B tests."
    });
    options.EnableAnnotations();
});

builder.Services.AddValidators();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("global", limiterOptions =>
    {
        limiterOptions.PermitLimit = 10;
        limiterOptions.Window = TimeSpan.FromSeconds(60);
        limiterOptions.QueueLimit = 0;
    });

    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        context.HttpContext.Response.ContentType = "application/json";
        var problem = new ProblemDetails
        {
            Title = "Too Many Requests",
            Status = StatusCodes.Status429TooManyRequests,
            Detail = "Too many requests. Please retry later."
        };
        await context.HttpContext.Response.WriteAsJsonAsync(problem, cancellationToken: token);
    }; 
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "TSMetrics API v1");
        options.RoutePrefix = "swagger";
    });
}

app.UseRateLimiter();
app.UseMiddleware<GlobalExceptionMiddleware>();

app.RegisterMetricsEndpoint();
app.RegisterAbTestEndpoints();

app.Run();

public partial class Program { } // For integration tests
