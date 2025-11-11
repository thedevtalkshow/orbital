using orbital.core;
using orbital.core.Data;

namespace orbital.test.api
{
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
            // throw new ArgumentException($"Meeting with Id {meeting.Id} now found.");
            return Task.FromResult(new Meeting());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">Unique id of the meeting object</param>
        /// <returns>Returns the unique found meeting or null.</returns>
        public Task<Meeting?> GetMeetingByIdAsync(string id)
        {
            return Task.FromResult(_meetings.SingleOrDefault(m => m.Id == id));
        }

        public Task<IEnumerable<Meeting>> ListMeetingsAsync()
        {
            return Task.FromResult<IEnumerable<Meeting>>(_meetings);
        }
    }
}
