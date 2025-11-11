using FluentAssertions;
using orbital.core;

namespace orbital.core.tests.Domain;

public class MeetingTests
{
    [Fact]
    public void Meeting_WhenCreated_ShouldHaveDefaultType()
    {
        // Arrange & Act
        var meeting = new Meeting();

        // Assert
        meeting.Type.Should().Be("meeting");
    }

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
    public void Meeting_WhenTitleIsSet_ShouldRetainValue()
    {
        // Arrange
        var title = "Project Kickoff Meeting";

        // Act
        var meeting = new Meeting { Title = title };

        // Assert
        meeting.Title.Should().Be(title);
    }

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
}
