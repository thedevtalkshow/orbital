using Microsoft.AspNetCore.Authorization;
using orbital.core;
using orbital.core.Services;

namespace orbital.api.Endpoints;

public static class MetadataEndpoints
{
    public static void MapMetadataEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/metadata")
            .WithTags("Metadata");

        // Get event statuses
        group.MapGet("/eventStatuses", async (IMetadataService metadataService) =>
        {
            var items = await metadataService.GetMetadataItemsAsync<EventStatusDefinition>("eventStatus");
            return Results.Ok(items);
        })
        .WithName("GetEventStatuses")
        .WithOpenApi()
        .Produces<IEnumerable<EventStatusDefinition>>(StatusCodes.Status200OK);

        // Get attendance modes
        group.MapGet("/attendanceModes", async (IMetadataService metadataService) =>
        {
            var items = await metadataService.GetMetadataItemsAsync<AttendanceModeDefinition>("attendanceMode");
            return Results.Ok(items);
        })
        .WithName("GetAttendanceModes")
        .WithOpenApi()
        .Produces<IEnumerable<AttendanceModeDefinition>>(StatusCodes.Status200OK);

        // Generic endpoint for any metadata type
        group.MapGet("/{metadataType}", async (string metadataType, IMetadataService metadataService) =>
        {
            var items = await metadataService.GetMetadataItemsAsync<MetadataDefinition>(metadataType);
            return Results.Ok(items);
        })
        .WithName("GetMetadataByType")
        .WithOpenApi()
        .Produces<IEnumerable<MetadataDefinition>>(StatusCodes.Status200OK);

        // Admin endpoints for CRUD operations
        var adminGroup = group.MapGroup("/admin")
            .RequireAuthorization("Admin");

        // Create metadata item
        adminGroup.MapPost("/", async (MetadataDefinition item, IMetadataService metadataService) =>
        {
            // Implementation would depend on your service methods
            // This is a placeholder assuming you have a CreateMetadataItemAsync method
            if (string.IsNullOrEmpty(item.Type) || string.IsNullOrEmpty(item.Value))
            {
                return Results.BadRequest("Type and Value are required");
            }

            var result = await metadataService.CreateMetadataItemAsync(item);
            return Results.Created($"/api/metadata/{item.Type}/{result.Id}", result);
        })
        .WithName("CreateMetadataItem")
        .WithOpenApi()
        .Produces<MetadataDefinition>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest);

        // Update metadata item
        adminGroup.MapPut("/", async (MetadataDefinition item, IMetadataService metadataService) =>
        {
            // Implementation would depend on your service methods
            if (string.IsNullOrEmpty(item.Id) || string.IsNullOrEmpty(item.Type))
            {
                return Results.BadRequest("Id and Type are required");
            }

            var success = await metadataService.UpdateMetadataItemAsync(item);
            if (!success)
            {
                return Results.NotFound();
            }

            // Refresh the cache for this metadata type
            await metadataService.RefreshCacheAsync(item.Type);
            return Results.NoContent();
        })
        .WithName("UpdateMetadataItem")
        .WithOpenApi()
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status400BadRequest);

        // Delete metadata item
        adminGroup.MapDelete("/{metadataType}/{id}", async (string metadataType, string id, IMetadataService metadataService) =>
        {
            // Implementation would depend on your service methods
            var success = await metadataService.DeleteMetadataItemAsync(id, metadataType);
            if (!success)
            {
                return Results.NotFound();
            }

            // Refresh the cache for this metadata type
            await metadataService.RefreshCacheAsync(metadataType);
            return Results.NoContent();
        })
        .WithName("DeleteMetadataItem")
        .WithOpenApi()
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound);
    }
}