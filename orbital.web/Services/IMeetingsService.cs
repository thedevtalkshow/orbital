using orbital.core;

namespace orbital.web.Services
{
    public interface IMeetingsService
    {
        Task<List<Meeting>> GetMeetingsAsync();
        Task<bool> AddMeetingAsync(Meeting meeting);
    }
}