using Infrastructure;
using TSMetricsAPI.Endpoints;
using TSMetricsAPI.Middleware;
using TSMetricsAPI.Extensions;
using System.Text.Json;
using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

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

        var payload = new
        {
            status = StatusCodes.Status429TooManyRequests,
            error = "rate_limit_exceeded",
            message = "Too many requests. Please retry later."
        };

        await context.HttpContext.Response.WriteAsJsonAsync(payload, cancellationToken: token);
    };
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseRateLimiter();
app.UseMiddleware<GlobalExceptionMiddleware>();

app.RegisterMetricsEndpoint();

app.Run();
