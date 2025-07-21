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
var database = cosmosdb.AddCosmosDatabase("orbital", "orbital");
var meetingContainer = database.AddContainer("meetingContainer", "/type", "meetings");   // meetings container
var metadataContainer = database.AddContainer("metadataContainer", "/type", "metadata"); // metadata container

var orbital_api = builder.AddProject<orbital_api>("orbital-api")
        .WaitFor(meetingContainer)
        .WaitFor(metadataContainer)
        .WithReference(meetingContainer)
        .WithReference(metadataContainer);

builder.AddProject<orbital_web>("orbital-web")
        .WithReference(orbital_api)
        .WaitFor(orbital_api);

builder.Build().Run();
