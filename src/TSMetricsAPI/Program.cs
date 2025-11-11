using Infrastructure;
using TSMetricsAPI.Extensions;
using TSMetricsAPI.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddValidators();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.RegisterMetricsEndpoint();

app.Run();
