using System.ComponentModel.DataAnnotations;

namespace orbital.core;

public class Meeting
{
    public string Id { get; set; }

    public static string Type => "meeting";

    [Required(ErrorMessage = "Title is required")]
    [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
    public string Title { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Start time is required")]
    [Display(Name = "Start Time")]
    public DateTime StartTime { get; set; }

    [Required(ErrorMessage = "End time is required")]
    [Display(Name = "End Time")]
    [CustomValidation(typeof(Meeting), nameof(ValidateEndTime))]
    public DateTime EndTime { get; set; }

    // Custom validation method to ensure EndTime is after StartTime
    public static ValidationResult ValidateEndTime(DateTime endTime, ValidationContext context)
    {
        var meeting = (Meeting)context.ObjectInstance;

        if (endTime <= meeting.StartTime)
        {
            return new ValidationResult("End time must be after start time");
        }

        return ValidationResult.Success;
    }
}
