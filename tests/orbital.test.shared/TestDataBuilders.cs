using orbital.core;
using orbital.core.Metadata;
using orbital.core.Models;

namespace orbital.test.shared;

/// <summary>
/// Provides test data builders for domain models.
/// </summary>
public static class TestDataBuilders
{
    /// <summary>
    /// Creates a default valid DateTime for testing.
    /// </summary>
    public static DateTime GetTestDateTime(int hoursFromNow = 0)
    {
        return DateTime.UtcNow.Date.AddHours(9 + hoursFromNow); // 9 AM + offset
    }

    /// <summary>
    /// Creates a random string for testing purposes.
    /// </summary>
    public static string GetRandomString(int length = 10)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var random = new Random();
        return new string(Enumerable.Range(0, length).Select(_ => chars[random.Next(chars.Length)]).ToArray());
    }

    /// <summary>
    /// Builder for creating Meeting instances with valid default values for testing.
    /// </summary>
    public class MeetingBuilder
    {
        private string _id = Guid.NewGuid().ToString();
        private string _title = "Test Meeting";
        private string _description = "Test Description";
        private DateTime _startTime = GetTestDateTime();
        private DateTime _endTime = GetTestDateTime(1);
        private string _about = string.Empty;
        private List<string> _attendees = new();
        private string _audience = string.Empty;
        private DateTime? _doorTime = null;
        private string _duration = string.Empty;
        private string _eventAttendanceMode = string.Empty;
        private string _eventStatus = string.Empty;
        private string _inLanguage = string.Empty;
        private bool _isAccessibleForFree = true;
        private List<string> _keywords = new();
        private string _location = string.Empty;
        private int? _maximumAttendeeCapacity = null;
        private int? _maximumPhysicalAttendeeCapacity = null;
        private string _organizer = string.Empty;
        private List<string> _performers = new();
        private DateTime? _previousStartDate = null;
        private int? _remainingAttendeeCapacity = null;
        private List<string> _subEvents = new();
        private string _superEvent = string.Empty;
        private string _url = string.Empty;

        public MeetingBuilder WithId(string id)
        {
            _id = id;
            return this;
        }

        public MeetingBuilder WithTitle(string title)
        {
            _title = title;
            return this;
        }

        public MeetingBuilder WithDescription(string description)
        {
            _description = description;
            return this;
        }

        public MeetingBuilder WithStartTime(DateTime startTime)
        {
            _startTime = startTime;
            return this;
        }

        public MeetingBuilder WithEndTime(DateTime endTime)
        {
            _endTime = endTime;
            return this;
        }

        public MeetingBuilder WithTimes(DateTime startTime, DateTime endTime)
        {
            _startTime = startTime;
            _endTime = endTime;
            return this;
        }

        public MeetingBuilder WithLocation(string location)
        {
            _location = location;
            return this;
        }

        public MeetingBuilder WithAttendees(List<string> attendees)
        {
            _attendees = attendees;
            return this;
        }

        public MeetingBuilder WithEventStatus(string eventStatus)
        {
            _eventStatus = eventStatus;
            return this;
        }

        public Meeting Build()
        {
            return new Meeting
            {
                Id = _id,
                Title = _title,
                Description = _description,
                StartTime = _startTime,
                EndTime = _endTime,
                About = _about,
                Attendees = _attendees,
                Audience = _audience,
                DoorTime = _doorTime,
                Duration = _duration,
                EventAttendanceMode = _eventAttendanceMode,
                EventStatus = _eventStatus,
                InLanguage = _inLanguage,
                IsAccessibleForFree = _isAccessibleForFree,
                Keywords = _keywords,
                Location = _location,
                MaximumAttendeeCapacity = _maximumAttendeeCapacity,
                MaximumPhysicalAttendeeCapacity = _maximumPhysicalAttendeeCapacity,
                Organizer = _organizer,
                Performers = _performers,
                PreviousStartDate = _previousStartDate,
                RemainingAttendeeCapacity = _remainingAttendeeCapacity,
                SubEvents = _subEvents,
                SuperEvent = _superEvent,
                Url = _url
            };
        }
    }

    /// <summary>
    /// Creates a MeetingBuilder with default valid values.
    /// </summary>
    public static MeetingBuilder CreateMeeting()
    {
        return new MeetingBuilder();
    }

    /// <summary>
    /// Builder for creating MetadataDefinition instances for testing.
    /// </summary>
    public class MetadataDefinitionBuilder
    {
        private string _id = Guid.NewGuid().ToString();
        private string _type = "TestType";
        private string _value = "test-value";
        private string _displayName = "Test Display Name";
        private string _description = "Test Description";
        private bool _isActive = true;
        private int _sortOrder = 0;

        public MetadataDefinitionBuilder WithId(string id)
        {
            _id = id;
            return this;
        }

        public MetadataDefinitionBuilder WithType(string type)
        {
            _type = type;
            return this;
        }

        public MetadataDefinitionBuilder WithValue(string value)
        {
            _value = value;
            return this;
        }

        public MetadataDefinitionBuilder WithDisplayName(string displayName)
        {
            _displayName = displayName;
            return this;
        }

        public MetadataDefinitionBuilder WithDescription(string description)
        {
            _description = description;
            return this;
        }

        public MetadataDefinitionBuilder WithIsActive(bool isActive)
        {
            _isActive = isActive;
            return this;
        }

        public MetadataDefinitionBuilder WithSortOrder(int sortOrder)
        {
            _sortOrder = sortOrder;
            return this;
        }

        public MetadataDefinition Build()
        {
            return new MetadataDefinition
            {
                Id = _id,
                Type = _type,
                Value = _value,
                DisplayName = _displayName,
                Description = _description,
                IsActive = _isActive,
                SortOrder = _sortOrder
            };
        }
    }

    /// <summary>
    /// Creates a MetadataDefinitionBuilder with default valid values.
    /// </summary>
    public static MetadataDefinitionBuilder CreateMetadataDefinition()
    {
        return new MetadataDefinitionBuilder();
    }
}
