using Microsoft.Azure.Cosmos;
using orbital.core;
using orbital.core.Data;
using orbital.data;

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

builder.Services.AddScoped<IMeetingRepository, CosmosMeetingRepository>();

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

app.MapGet("api/hello", () => "Hello World!");

app.MapGet("/api/meetings", async (IMeetingRepository repository) =>
{
    var meetings = await repository.GetMeetingsAsync();
    return Results.Ok(meetings);
});

app.MapGet("/api/meetings/{id}", async (string id, IMeetingRepository repository) =>
{
    var meeting = await repository.GetMeetingByIdAsync(id);
    return meeting;
});

app.MapPost("/api/meetings", async (CosmosClient client, Meeting meeting) =>
{
    try
    {
        var container = client.GetContainer("orbital", "meetings");
        var response = await container.CreateItemAsync(meeting, new PartitionKey(Meeting.Type));
        return Results.Created($"/api/meetings/{meeting.Id}", meeting);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});


app.Run();

public partial class Program { }

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
