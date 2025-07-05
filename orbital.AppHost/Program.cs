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
var container = database.AddContainer("meetings", "/type", "meetings");

// metadata container
var metadataContainer = database.AddContainer("metadataContainer", "/type", "metadata");

// TODO: check if metadatacontainer is empty and add seed data.
// or does this move to the Imetadatarepository as adefault implementation.

var orbital_api = builder.AddProject<orbital_api>("orbital-api")
        .WithReference(cosmosdb)
        .WithReference(metadataContainer);

builder.AddProject<orbital_web>("orbital-web")
        .WithReference(orbital_api);

builder.Build().Run();
