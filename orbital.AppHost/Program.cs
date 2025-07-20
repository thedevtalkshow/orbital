#pragma warning disable ASPIRECOSMOSDB001
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var cosmosdb = builder.AddAzureCosmosDB("cosmosdb").RunAsPreviewEmulator(
    emulator =>
    {
        emulator.WithDataExplorer(8888);
        emulator.WithDataVolume();
        emulator.WithLifetime(ContainerLifetime.Persistent);
    });

// need to define reference object for each cosmosdb client and container.
// meetings container
var database = cosmosdb.AddCosmosDatabase("orbital", "orbital");
var meetingContainer = database.AddContainer("meetingContainer", "/type", "meetings");

// metadata container
var metadataContainer = database.AddContainer("metadataContainer", "/type", "metadata");

var orbital_api = builder.AddProject<orbital_api>("orbital-api")
        .WaitFor(meetingContainer)
        .WithReference(cosmosdb)
        // .WithReference(meetingContainer)
        .WithReference(metadataContainer);

builder.AddProject<orbital_web>("orbital-web")
        .WithReference(orbital_api)
        .WaitFor(orbital_api);

builder.Build().Run();
