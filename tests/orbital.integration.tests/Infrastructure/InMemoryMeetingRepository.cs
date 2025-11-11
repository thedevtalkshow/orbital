using orbital.core;
using orbital.core.Data;

namespace orbital.integration.tests.Infrastructure;

/// <summary>
/// In-memory implementation of IMeetingRepository for integration testing.
/// Provides fast, isolated test execution without external database dependencies.
/// </summary>
public class InMemoryMeetingRepository : IMeetingRepository
{
    private readonly List<Meeting> _meetings;

    public InMemoryMeetingRepository()
    {
        _meetings = new List<Meeting>
        {
            new Meeting { Id = "1", Title = "Test Meeting 1", Description = "This is a test meeting." },
            new Meeting { Id = "2", Title = "Test Meeting 2", Description = "This is another test meeting." }
        };
    }

    public Task<Meeting> CreateMeetingAsync(Meeting meeting)
    {
        _meetings.Add(meeting);
        return Task.FromResult(meeting);
    }

    public Task<Meeting> UpdateMeetingAsync(Meeting meeting)
    {
        var meetingIndex = _meetings.FindIndex(m => m.Id == meeting.Id);
        if (meetingIndex != -1)
        {
            _meetings[meetingIndex] = meeting;
            return Task.FromResult(meeting);
        }
        // Return null for non-existent meeting
        return Task.FromResult<Meeting>(null!);
    }

    public Task<Meeting> GetMeetingByIdAsync(string id)
    {
        return Task.FromResult(_meetings.SingleOrDefault(m => m.Id == id))!;
    }

    public Task<IEnumerable<Meeting>> ListMeetingsAsync()
    {
        return Task.FromResult<IEnumerable<Meeting>>(_meetings);
    }
}
