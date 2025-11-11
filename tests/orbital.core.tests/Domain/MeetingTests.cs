using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using orbital.core;
using orbital.test.shared;

namespace orbital.core.tests.Domain;

public class MeetingTests
{
    #region Creation and Default Values Tests

    [Fact]
    public void Meeting_WhenCreated_ShouldHaveDefaultType()
    {
        // Arrange & Act
        var meeting = new Meeting();

        // Assert
        meeting.Type.Should().Be("meeting");
    }

    [Fact]
    public void Meeting_WhenCreated_ShouldInitializeCollectionsAsEmpty()
    {
        // Arrange & Act
        var meeting = new Meeting();

        // Assert
        meeting.Attendees.Should().NotBeNull().And.BeEmpty();
        meeting.Keywords.Should().NotBeNull().And.BeEmpty();
        meeting.Performers.Should().NotBeNull().And.BeEmpty();
        meeting.SubEvents.Should().NotBeNull().And.BeEmpty();
    }

    [Fact]
    public void Meeting_WhenCreated_ShouldHaveEmptyStringDefaults()
    {
        // Arrange & Act
        var meeting = new Meeting();

        // Assert
        meeting.Id.Should().BeEmpty();
        meeting.Title.Should().BeEmpty();
        meeting.Description.Should().BeEmpty();
        meeting.About.Should().BeEmpty();
        meeting.Audience.Should().BeEmpty();
        meeting.Location.Should().BeEmpty();
        meeting.Organizer.Should().BeEmpty();
        meeting.SuperEvent.Should().BeEmpty();
        meeting.Url.Should().BeEmpty();
    }

    #endregion

    #region Title Validation Tests

    [Theory]
    [InlineData("", false)]
    [InlineData("Valid Title", true)]
    [InlineData("A", true)]
    public void Meeting_TitleValidation_ReturnsExpectedResult(string title, bool isValid)
    {
        // Arrange
        var meeting = TestDataBuilders.CreateMeeting()
            .WithTitle(title)
            .Build();

        var validationContext = new ValidationContext(meeting);
        var validationResults = new List<ValidationResult>();

        // Act
        var actualIsValid = Validator.TryValidateObject(meeting, validationContext, validationResults, true);

        // Assert
        actualIsValid.Should().Be(isValid);
        if (!isValid)
        {
            validationResults.Should().Contain(r => r.ErrorMessage!.Contains("Title is required"));
        }
    }

    [Fact]
    public void Meeting_WhenTitleExceedsMaxLength_ShouldFailValidation()
    {
        // Arrange
        var longTitle = new string('A', 101);
        var meeting = TestDataBuilders.CreateMeeting()
            .WithTitle(longTitle)
            .Build();

        var validationContext = new ValidationContext(meeting);
        var validationResults = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(meeting, validationContext, validationResults, true);

        // Assert
        isValid.Should().BeFalse();
        validationResults.Should().Contain(r => r.ErrorMessage!.Contains("Title cannot exceed 100 characters"));
    }

    [Fact]
    public void Meeting_WhenTitleIsMaxLength_ShouldPassValidation()
    {
        // Arrange
        var maxLengthTitle = new string('A', 100);
        var meeting = TestDataBuilders.CreateMeeting()
            .WithTitle(maxLengthTitle)
            .Build();

        var validationContext = new ValidationContext(meeting);
        var validationResults = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(meeting, validationContext, validationResults, true);

        // Assert
        isValid.Should().BeTrue();
    }

    [Fact]
    public void Meeting_WhenTitleIsSet_ShouldRetainValue()
    {
        // Arrange
        var title = "Project Kickoff Meeting";

        // Act
        var meeting = new Meeting { Title = title };

        // Assert
        meeting.Title.Should().Be(title);
    }

    #endregion

    #region Description Validation Tests

    [Fact]
    public void Meeting_WhenDescriptionIsSet_ShouldRetainValue()
    {
        // Arrange
        var description = "Discussing project milestones and deliverables";

        // Act
        var meeting = new Meeting { Description = description };

        // Assert
        meeting.Description.Should().Be(description);
    }

    [Fact]
    public void Meeting_WhenDescriptionExceedsMaxLength_ShouldFailValidation()
    {
        // Arrange
        var longDescription = new string('A', 501);
        var meeting = TestDataBuilders.CreateMeeting()
            .WithDescription(longDescription)
            .Build();

        var validationContext = new ValidationContext(meeting);
        var validationResults = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(meeting, validationContext, validationResults, true);

        // Assert
        isValid.Should().BeFalse();
        validationResults.Should().Contain(r => r.ErrorMessage!.Contains("Description cannot exceed 500 characters"));
    }

    [Fact]
    public void Meeting_WhenDescriptionIsMaxLength_ShouldPassValidation()
    {
        // Arrange
        var maxLengthDescription = new string('A', 500);
        var meeting = TestDataBuilders.CreateMeeting()
            .WithDescription(maxLengthDescription)
            .Build();

        var validationContext = new ValidationContext(meeting);
        var validationResults = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(meeting, validationContext, validationResults, true);

        // Assert
        isValid.Should().BeTrue();
    }

    [Fact]
    public void Meeting_WhenDescriptionIsEmpty_ShouldPassValidation()
    {
        // Arrange
        var meeting = TestDataBuilders.CreateMeeting()
            .WithDescription("")
            .Build();

        var validationContext = new ValidationContext(meeting);
        var validationResults = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(meeting, validationContext, validationResults, true);

        // Assert
        isValid.Should().BeTrue();
    }

    #endregion

    #region StartTime and EndTime Validation Tests

    [Fact]
    public void Meeting_WhenCreatedWithValidDates_ShouldAcceptStartAndEndTime()
    {
        // Arrange
        var startTime = DateTime.UtcNow;
        var endTime = startTime.AddHours(1);

        // Act
        var meeting = new Meeting
        {
            StartTime = startTime,
            EndTime = endTime
        };

        // Assert
        meeting.StartTime.Should().Be(startTime);
        meeting.EndTime.Should().Be(endTime);
    }

    [Fact]
    public void Meeting_SetEndTimeBeforeStartTime_ShouldFailValidation()
    {
        // Arrange
        var startTime = TestDataBuilders.GetTestDateTime(2);
        var endTime = TestDataBuilders.GetTestDateTime(1); // 1 hour before start
        var meeting = TestDataBuilders.CreateMeeting()
            .WithTimes(startTime, endTime)
            .Build();

        var validationContext = new ValidationContext(meeting);
        var validationResults = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(meeting, validationContext, validationResults, true);

        // Assert
        isValid.Should().BeFalse();
        validationResults.Should().Contain(r => r.ErrorMessage == "End time must be after start time");
    }

    [Fact]
    public void Meeting_SetEndTimeEqualToStartTime_ShouldFailValidation()
    {
        // Arrange
        var time = TestDataBuilders.GetTestDateTime();
        var meeting = TestDataBuilders.CreateMeeting()
            .WithTimes(time, time)
            .Build();

        var validationContext = new ValidationContext(meeting);
        var validationResults = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(meeting, validationContext, validationResults, true);

        // Assert
        isValid.Should().BeFalse();
        validationResults.Should().Contain(r => r.ErrorMessage == "End time must be after start time");
    }

    [Fact]
    public void Meeting_WhenEndTimeIsAfterStartTime_ShouldPassValidation()
    {
        // Arrange
        var startTime = TestDataBuilders.GetTestDateTime();
        var endTime = TestDataBuilders.GetTestDateTime(2);
        var meeting = TestDataBuilders.CreateMeeting()
            .WithTimes(startTime, endTime)
            .Build();

        var validationContext = new ValidationContext(meeting);
        var validationResults = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(meeting, validationContext, validationResults, true);

        // Assert
        isValid.Should().BeTrue();
    }

    #endregion

    #region Custom Validation Method Tests

    [Fact]
    public void ValidateEndTime_WhenEndTimeAfterStartTime_ReturnsSuccess()
    {
        // Arrange
        var startTime = TestDataBuilders.GetTestDateTime();
        var endTime = TestDataBuilders.GetTestDateTime(1);
        var meeting = new Meeting { StartTime = startTime, EndTime = endTime };
        var context = new ValidationContext(meeting);

        // Act
        var result = Meeting.ValidateEndTime(endTime, context);

        // Assert
        result.Should().Be(ValidationResult.Success);
    }

    [Fact]
    public void ValidateEndTime_WhenEndTimeBeforeStartTime_ReturnsValidationError()
    {
        // Arrange
        var startTime = TestDataBuilders.GetTestDateTime(2);
        var endTime = TestDataBuilders.GetTestDateTime(1);
        var meeting = new Meeting { StartTime = startTime, EndTime = endTime };
        var context = new ValidationContext(meeting);

        // Act
        var result = Meeting.ValidateEndTime(endTime, context);

        // Assert
        result.Should().NotBe(ValidationResult.Success);
        result!.ErrorMessage.Should().Be("End time must be after start time");
    }

    [Fact]
    public void ValidateEndTime_WhenEndTimeEqualsStartTime_ReturnsValidationError()
    {
        // Arrange
        var time = TestDataBuilders.GetTestDateTime();
        var meeting = new Meeting { StartTime = time, EndTime = time };
        var context = new ValidationContext(meeting);

        // Act
        var result = Meeting.ValidateEndTime(time, context);

        // Assert
        result.Should().NotBe(ValidationResult.Success);
        result!.ErrorMessage.Should().Be("End time must be after start time");
    }

    [Fact]
    public void ValidateStartTime_WhenStartTimeIsDefault_ReturnsValidationError()
    {
        // Arrange
        var meeting = new Meeting();
        var context = new ValidationContext(meeting);
        var defaultDateTime = default(DateTime);

        // Act
        var result = Meeting.ValidateStartTime(defaultDateTime, context);

        // Assert
        result.Should().NotBe(ValidationResult.Success);
        result!.ErrorMessage.Should().Be("Start time is required");
    }

    [Fact]
    public void ValidateStartTime_WhenStartTimeIsNotDefault_ReturnsSuccess()
    {
        // Arrange
        var meeting = new Meeting();
        var context = new ValidationContext(meeting);
        var validDateTime = TestDataBuilders.GetTestDateTime();

        // Act
        var result = Meeting.ValidateStartTime(validDateTime, context);

        // Assert
        result.Should().Be(ValidationResult.Success);
    }

    #endregion

    #region Property Behavior Tests

    [Fact]
    public void Meeting_WhenIdIsSet_ShouldRetainValue()
    {
        // Arrange
        var id = Guid.NewGuid().ToString();

        // Act
        var meeting = new Meeting { Id = id };

        // Assert
        meeting.Id.Should().Be(id);
    }

    [Fact]
    public void Meeting_WhenLocationIsSet_ShouldRetainValue()
    {
        // Arrange
        var location = "Conference Room A";

        // Act
        var meeting = new Meeting { Location = location };

        // Assert
        meeting.Location.Should().Be(location);
    }

    [Fact]
    public void Meeting_WhenAttendeesAreAdded_ShouldRetainValues()
    {
        // Arrange
        var attendees = new List<string> { "user1@example.com", "user2@example.com" };

        // Act
        var meeting = new Meeting { Attendees = attendees };

        // Assert
        meeting.Attendees.Should().BeEquivalentTo(attendees);
    }

    [Fact]
    public void Meeting_WhenIsAccessibleForFreeIsSet_ShouldRetainValue()
    {
        // Arrange & Act
        var freeMeeting = new Meeting { IsAccessibleForFree = true };
        var paidMeeting = new Meeting { IsAccessibleForFree = false };

        // Assert
        freeMeeting.IsAccessibleForFree.Should().BeTrue();
        paidMeeting.IsAccessibleForFree.Should().BeFalse();
    }

    [Fact]
    public void Meeting_WhenMaximumAttendeeCapacityIsSet_ShouldRetainValue()
    {
        // Arrange
        var capacity = 100;

        // Act
        var meeting = new Meeting { MaximumAttendeeCapacity = capacity };

        // Assert
        meeting.MaximumAttendeeCapacity.Should().Be(capacity);
    }

    #endregion
}
