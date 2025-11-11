#pragma warning disable ASPIRECOSMOSDB001
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.DependencyInjection;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var cosmosdb = builder.AddAzureCosmosDB("cosmosdb").RunAsPreviewEmulator(
    emulator =>
    {
        emulator.WithDataExplorer(8888);
        emulator.WithDataVolume();
    });

// need to define reference object for each cosmosdb client and container.
var orbitalDatabase = cosmosdb.AddCosmosDatabase("orbital", "orbital");
var meetingContainer = orbitalDatabase.AddContainer("meetingContainer", "/type", "meetings");   // meetings container
var metadataContainer = orbitalDatabase.AddContainer("metadataContainer", "/type", "metadata"); // metadata container

var orbital_api = builder.AddProject<orbital_api>("orbital-api")
        .WaitFor(meetingContainer)
        .WaitFor(metadataContainer)
        .WithReference(orbitalDatabase)
        .WithReference(meetingContainer)
        .WithReference(metadataContainer);

var apiHttpClient = builder.Services.AddHttpClient<orbital_api>(
    static client => client.BaseAddress = new("https+http://orbital-api")
);

// orbital_api.WithSeedDataCommand();

builder.AddProject<orbital_web>("orbital-web")
        .WithReference(orbital_api)
        .WaitFor(orbital_api);

builder.Build().Run();
