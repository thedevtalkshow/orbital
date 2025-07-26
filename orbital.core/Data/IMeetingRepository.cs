using orbital.core;

namespace orbital.core.Data
{
    public interface IMeetingRepository
    {
        Task<IEnumerable<Meeting>> ListMeetingsAsync();

        Task<Meeting> GetMeetingByIdAsync(string id);

        Task<Meeting> CreateMeetingAsync(Meeting meeting);

        Task<Meeting> UpdateMeetingAsync(Meeting meeting);
    }
}