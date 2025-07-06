using orbital.core;
using orbital.core.Data;

namespace orbital.api.Endpoints;

public static class MeetingEndpoint
{
    public static void MapMeetingEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/meetings")
            .WithTags("Meeting");

        group.MapGet("/", async (IMeetingRepository repository) =>
        {
            var meetings = await repository.ListMeetingsAsync();
            return Results.Ok(meetings);
        })
        .WithName("GetAllMeetings")
        .WithOpenApi()
        .Produces<IEnumerable<Meeting>>(StatusCodes.Status200OK);

        group.MapGet("/{id}", async (string id, IMeetingRepository repository) =>
        {
            var meeting = await repository.GetMeetingByIdAsync(id);
            
            if (meeting == null)
            {
                return Results.NotFound();
            }

            return Results.Ok(meeting);
        });

        group.MapPost("/", async (Meeting meeting, IMeetingRepository repository) =>
        {
            var createdMeeting = await repository.CreateMeetingAsync(meeting);
            return Results.Created($"/api/meetings/{createdMeeting.Id}", createdMeeting);
        });
    }
}
