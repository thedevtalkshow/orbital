// using Microsoft.AspNetCore.Authorization;
using orbital.core.Data;
using orbital.core.Metadata;
using orbital.core.Models;

namespace orbital.api.Endpoints;

public static class MetadataEndpoints
{
    public static void MapMetadataEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/metadata")
            .WithTags("Metadata");

        // Get event statuses
        group.MapGet("/EventStatusType", async (IMetadataRepository metadataRepository) =>
        {
            var items = await metadataRepository.GetAllMetadataItemsAsync<EventStatusDefinition>("EventStatusType");
            return Results.Ok(items);
        })
        .WithName("GetEventStatuses")
        .WithOpenApi()
        .Produces<IEnumerable<EventStatusDefinition>>(StatusCodes.Status200OK);

        // Get attendance modes
        group.MapGet("/EventAttendanceModeEnumeration", async (IMetadataRepository metadataRepository) =>
        {
            var items = await metadataRepository.GetAllMetadataItemsAsync<AttendanceModeDefinition>("EventAttendanceModeEnumeration");
            return Results.Ok(items);
        })
        .WithName("GetAttendanceModes")
        .WithOpenApi()
        .Produces<IEnumerable<AttendanceModeDefinition>>(StatusCodes.Status200OK);

        // Generic endpoint for any metadata type
        group.MapGet("/{metadataType}", async (string metadataType, IMetadataRepository metadataRepository) =>
        {
            var items = await metadataRepository.GetAllMetadataItemsAsync<IMetadataItem>(metadataType);
            return Results.Ok(items);
        })
        .WithName("GetMetadataByType")
        .WithOpenApi()
        .Produces<IEnumerable<MetadataDefinition>>(StatusCodes.Status200OK);

        // // Admin endpoints for CRUD operations
        var adminGroup = group.MapGroup("/admin");
            //.RequireAuthorization("Admin");

        // Create metadata item
        adminGroup.MapPost("/", async (MetadataDefinition item, IMetadataRepository metadataRepository) =>
        {
            // Implementation would depend on your service methods
            // This is a placeholder assuming you have a CreateMetadataItemAsync method
            if (string.IsNullOrEmpty(item.Type) || string.IsNullOrEmpty(item.Value))
            {
                return Results.BadRequest("Type and Value are required");
            }

            var result = await metadataRepository.CreateMetadataItemAsync(item);
            return Results.Created($"/api/metadata/{item.Type}/{result.Id}", result);
        })
        .WithName("CreateMetadataItem")
        .WithOpenApi()
        .Produces<MetadataDefinition>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest);

        // Update metadata item
        adminGroup.MapPut("/", async (MetadataDefinition item, IMetadataRepository metadataRepository) =>
        {
            // Implementation would depend on your service methods
            if (string.IsNullOrEmpty(item.Id) || string.IsNullOrEmpty(item.Type))
            {
                return Results.BadRequest("Id and Type are required");
            }

            var success = await metadataRepository.UpdateMetadataItemAsync(item);
            if (!success)
            {
                return Results.NotFound();
            }

            // Refresh the cache for this metadata type
            // await metadataRepository.RefreshCacheAsync(item.Type);
            return Results.NoContent();
        })
        .WithName("UpdateMetadataItem")
        .WithOpenApi()
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status400BadRequest);

        // Delete metadata item
        adminGroup.MapDelete("/{metadataType}/{id}", async (string metadataType, string id, IMetadataRepository metadataRepository) =>
        {
            // Implementation would depend on your service methods
            var success = await metadataRepository.DeleteMetadataItemAsync<IMetadataItem>(id, metadataType);
            if (!success)
            {
                return Results.NotFound();
            }

            // Refresh the cache for this metadata type
            // await metadataRepository.RefreshCacheAsync(metadataType);
            return Results.NoContent();
        })
        .WithName("DeleteMetadataItem")
        .WithOpenApi()
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound);
    }
}