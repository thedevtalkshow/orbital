using Azure.Identity;
using Microsoft.Azure.Cosmos;
using orbital.core;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add CORS policy scoped to localhost:7024
builder.Services.AddCors(options =>
{
    options.AddPolicy("LocalhostPolicy", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.AddAzureCosmosClient("cosmosdb", configureClientOptions: clientOptions =>
{
    clientOptions.SerializerOptions = new CosmosSerializationOptions()
    {
        IgnoreNullValues = true,
        Indented = false,
        PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
    };
});

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseRouting();

// Add CORS middleware - must be before UseHttpsRedirection and endpoint routing
app.UseCors("LocalhostPolicy");

app.UseHttpsRedirection();

app.MapGet("/api/meetings", async (CosmosClient client) =>
{
    var container = client.GetContainer("orbital", "meetings");
    var resultIterator = container.GetItemQueryIterator<Meeting>("SELECT * FROM c WHERE c.type='meeting'");
    var meetings = new List<Meeting>();

    while (resultIterator.HasMoreResults)
    {
        var resultSet = await resultIterator.ReadNextAsync();
        foreach (var meeting in resultSet)
        {
            meetings.Add(meeting);
        }
    }
    return Results.Ok(meetings);
});

app.MapGet("/api/meetings/{id}", async (CosmosClient client, string id) =>
{
    try
    {
        var container = client.GetContainer("orbital", "meetings");
        var response = await container.ReadItemAsync<Meeting>(id, new PartitionKey("meeting"));
        return Results.Ok(response.Resource);
    }
    catch (CosmosException ex)
    {
        return Results.NotFound(ex.Message);
    }

});

app.MapPost("/api/meetings", async (CosmosClient client, Meeting meeting) =>
{
    try
    {
        var container = client.GetContainer("orbital", "meetings");
        var response = await container.CreateItemAsync(meeting, new PartitionKey(meeting.Type));
        return Results.Created($"/api/meetings/{meeting.Id}", meeting);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});


app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
