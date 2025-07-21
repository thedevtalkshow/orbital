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

var cosmosAction = (CosmosClientOptions clientOptions) =>
{
    clientOptions.SerializerOptions = new CosmosSerializationOptions()
    {
        IgnoreNullValues = true,
        Indented = false,
        PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
    };
    
};
builder.AddAzureCosmosClient("cosmosdb", configureClientOptions: cosmosAction);

//AppHost - container references included with the api project.
builder.AddKeyedAzureCosmosContainer("meetingContainer", configureClientOptions: cosmosAction); 
builder.AddKeyedAzureCosmosContainer("metadataContainer", configureClientOptions: cosmosAction);

builder.Services.AddScoped<IMeetingRepository, CosmosMeetingRepository>();
builder.Services.AddScoped<IMetadataRepository, CosmosMetadataRepository>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseRouting();

// Add CORS middleware - must be before UseHttpsRedirection and endpoint routing
app.UseCors("LocalhostPolicy");

app.UseHttpsRedirection();
app.MapDefaultEndpoints();

// register the meeting endpoints
app.MapMeetingEndpoints();

// register the metadata endpoints
// app.MapMetadataEndpoints();

app.Run();

public partial class Program { }
