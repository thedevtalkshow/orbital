using orbital.core;

namespace orbital.web.Services
{
    public interface IMeetingStateService
    {
        Meeting? CurrentEditingMeeting { get; set; }
        void ClearEditingMeeting();
    }

    public class MeetingStateService : IMeetingStateService
    {
        public Meeting? CurrentEditingMeeting { get; set; }

        public void ClearEditingMeeting()
        {
            CurrentEditingMeeting = null;
        }
    }
}
