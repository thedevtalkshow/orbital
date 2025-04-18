using Azure.Identity;
using Microsoft.Azure.Cosmos;
using orbital.core;

var builder = WebApplication.CreateBuilder(args);

#region Config

string cosmosAccountEndpoint = "https://cosmosdb-wad2cfjazfcve.documents.azure.com:443/";

#endregion

builder.AddServiceDefaults();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//add openAPI support

builder.AddAzureCosmosClient("cosmosdb");

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

DefaultAzureCredential defaultAzureCredential = new DefaultAzureCredential();

var cosmosClient = new CosmosClient(cosmosAccountEndpoint, defaultAzureCredential);
//var cosmosClient = new CosmosClient("AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==");

Database database = cosmosClient.GetDatabase("orbital");
Container container = database.GetContainer("meetings");

app.MapGet("/api/meetings", () =>
{
    return Results.Ok(container.GetItemLinqQueryable<Meeting>().ToList());
});

app.MapGet("api/meetings/{id}", async (string id) =>
{
    try
    {
        var response = await container.ReadItemAsync<Meeting>(id, new PartitionKey("meeting"));
        return Results.Ok(response.Resource);
    }
    catch (CosmosException ex)
    {
        return Results.NotFound(ex.Message);
    }

});

app.MapPost("/api/meetings", async (Meeting meeting) =>
{
    try
    {
        var response = await container.CreateItemAsync(meeting, new PartitionKey(meeting.type));
        return Results.Created($"/api/meetings/{meeting.id}", meeting);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});




var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
