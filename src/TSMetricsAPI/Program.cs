var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/ab-experiment", () => {
    var experiment = new[]
    {
        new { id = 1, name = "experiment 1" },
        new { id = 2, name = "experiment 2" },
    };
    
    return Results.Ok(experiment);
})
.WithName("GetABExperiment");

app.Run();
