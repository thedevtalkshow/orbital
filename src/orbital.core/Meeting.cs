using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace orbital.core;

public class Meeting
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public string Type { get; set; } = "meeting";

    [Required(ErrorMessage = "Title is required")]
    [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Start time is required")]
    [Display(Name = "Start Time")]
    [CustomValidation(typeof(Meeting), nameof(ValidateStartTime))]
    [JsonPropertyName("startTime")]
    public DateTime StartTime { get; set; }

    [Required(ErrorMessage = "End time is required")]
    [Display(Name = "End Time")]
    [CustomValidation(typeof(Meeting), nameof(ValidateEndTime))]
    [JsonPropertyName("endTime")]
    public DateTime EndTime { get; set; }

    // Schema.org properties

    /// <summary>
    /// The subject matter of the content.
    /// </summary>
    public string About { get; set; } = string.Empty;

    // TODO:update this to be the Attendee type
    /// <summary>
    /// A person or organization attending the event.
    /// </summary>
    public List<string> Attendees { get; set; } = new List<string>();

    /// <summary>
    /// An intended audience, i.e. a group for whom something was created.
    /// </summary>
    public string Audience { get; set; } = string.Empty;

    /// <summary>
    /// The time admission will commence.
    /// </summary>
    public DateTime? DoorTime { get; set; }

    // TODO: can this item be calculated on save event or when the properties are updated?
    /// <summary>
    /// The duration of the item in ISO 8601 duration format.
    /// </summary>
    public string Duration { get; set; } = string.Empty;

    /// <summary>
    /// The eventAttendanceMode indicates whether it occurs online, offline, or a mix.
    /// </summary>
    public string EventAttendanceMode { get; set; } = string.Empty;

    /// <summary>
    /// An eventStatus represents its status (e.g., cancelled or rescheduled).
    /// </summary>
    public string EventStatus { get; set; } = string.Empty;

    /// <summary>
    /// The language of the content or performance.
    /// </summary>
    public string InLanguage { get; set; } = string.Empty;

    /// <summary>
    /// A flag to signal that the event is accessible for free.
    /// </summary>
    public bool IsAccessibleForFree { get; set; }

    /// <summary>
    /// Keywords or tags used to describe the event.
    /// </summary>
    public List<string> Keywords { get; set; } = new List<string>();

    /// <summary>
    /// The location where the event is happening.
    /// </summary>
    public string Location { get; set; } = string.Empty;

    /// <summary>
    /// The total number of individuals that may attend the event.
    /// </summary>
    public int? MaximumAttendeeCapacity { get; set; }

    /// <summary>
    /// The maximum physical attendee capacity of an Event.
    /// </summary>
    public int? MaximumPhysicalAttendeeCapacity { get; set; }

    /// <summary>
    /// An organizer of the Event.
    /// </summary>
    public string Organizer { get; set; } = string.Empty;

    /// <summary>
    /// A performer at the event.
    /// </summary>
    public List<string> Performers { get; set; } = new List<string>();

    /// <summary>
    /// Used for rescheduled or cancelled events. Contains the previously scheduled start date.
    /// </summary>
    public DateTime? PreviousStartDate { get; set; }

    /// <summary>
    /// The number of attendee places for an event that remain unallocated.
    /// </summary>
    public int? RemainingAttendeeCapacity { get; set; }

    /// <summary>
    /// An Event that is part of this event.
    /// </summary>
    public List<string> SubEvents { get; set; } = new List<string>();

    /// <summary>
    /// An event that this event is a part of.
    /// </summary>
    public string SuperEvent { get; set; } = string.Empty;

    /// <summary>
    /// URL of the item.
    /// </summary>
    public string Url { get; set; } = string.Empty;

    // Custom validation method to ensure EndTime is after StartTime
    public static ValidationResult ValidateEndTime(DateTime endTime, ValidationContext context)
    {
        var meeting = (Meeting)context.ObjectInstance;

        if (endTime <= meeting.StartTime)
        {
            return new ValidationResult("End time must be after start time");
        }

        return ValidationResult.Success!;
    }

    public static ValidationResult ValidateStartTime(DateTime startTime, ValidationContext context)
    {
        if (startTime == default(DateTime))
        {
            return new ValidationResult("Start time is required");
        }

        return ValidationResult.Success!;
    }
}
