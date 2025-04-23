#pragma warning disable ASPIRECOSMOSDB001
using Azure.Provisioning.CosmosDB;
using Microsoft.AspNetCore.Builder;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var cosmosdb = builder.AddAzureCosmosDB("cosmosdb").RunAsPreviewEmulator(
    emulator =>
    {
        emulator.WithDataExplorer(8888);
        emulator.WithDataVolume();
        emulator.WithLifetime(ContainerLifetime.Persistent);
    });

var database = cosmosdb.AddCosmosDatabase("orbital", "orbital");
var container = database.AddContainer("meetings", "/type", "meetings");

var orbital_api = builder.AddProject<Projects.orbital_api>("orbital-api")
        .WithReference(cosmosdb);

builder.AddProject<orbital_web>("web")
        .WithReference(orbital_api);

builder.Build().Run();
