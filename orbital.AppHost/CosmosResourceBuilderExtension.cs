
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Aspire.Hosting;

internal static class OrbitalApiResourceCommandBuilder
{
    public static IResourceBuilder<ProjectResource> WithSeedDataCommand(
        this IResourceBuilder<ProjectResource> builder)
    {
        var commandOptions = new CommandOptions
        {
            UpdateState = OnUpdateResourceState,
            IconName = "DatabaseLightning"

        };

        builder.WithCommand(
            name: "seed-data",
            displayName: "Seed Data",
            executeCommand: context => OnRunSeedDataCommand(builder, context),
            commandOptions: commandOptions);
            
        return builder;
    }

    private static ResourceCommandState OnUpdateResourceState(UpdateCommandStateContext context)
    {
        return context.ResourceSnapshot.HealthStatus is HealthStatus.Healthy?
            ResourceCommandState.Enabled :
            ResourceCommandState.Disabled;
    }

    public static async Task<ExecuteCommandResult> OnRunSeedDataCommand(IResourceBuilder<ProjectResource> builder, ExecuteCommandContext context)
    {
        //seed the cosmos db with initial data using the same capabilities as in the orbital_api project.
        HttpClient httpClient = new HttpClient();
        // httpClient.BaseAddress = 

        Console.WriteLine("Seeding data into Cosmos DB...");
        // Example: await SeedDataAsync(cosmosResource.Client);
        Console.WriteLine("Data seeding completed.");

        return CommandResults.Success();
    }
}
