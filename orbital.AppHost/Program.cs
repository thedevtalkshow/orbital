#pragma warning disable ASPIRECOSMOSDB001
using Microsoft.AspNetCore.Builder;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var cosmosdb = builder.AddAzureCosmosDB("cosmosdb").RunAsPreviewEmulator(
    emulator =>
    {
        emulator.WithDataExplorer(8888);
        emulator.WithDataVolume("appData");
        emulator.WithLifetime(ContainerLifetime.Persistent);
    });

builder.AddProject<orbital_web>("web");

builder.Build().Run();
