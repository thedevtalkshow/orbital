using Microsoft.Azure.Cosmos;
using orbital.api.Endpoints;
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

builder.AddAzureCosmosContainer(connectionName: "metadataContainer");

builder.Services.AddScoped<IMeetingRepository, CosmosMeetingRepository>();
builder.Services.AddScoped<IMetadataRepository, CosmosMetadataRepository>();

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

app.MapGet("/api/meetings", async (IMeetingRepository repository) =>
{
    var meetings = await repository.ListMeetingsAsync();
    return Results.Ok(meetings);
});

app.MapGet("/api/meetings/{id}", async (string id, IMeetingRepository repository) =>
{
    var meeting = await repository.GetMeetingByIdAsync(id);
    
    if (meeting == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(meeting);
});

app.MapPost("/api/meetings", async (Meeting meeting, IMeetingRepository repository) =>
{
    var createdMeeting = await repository.CreateMeetingAsync(meeting);
    return Results.Created($"/api/meetings/{createdMeeting.Id}", createdMeeting);
});

// register the metadata endpoints
app.MapMetadataEndpoints();

app.Run();

public partial class Program { }
