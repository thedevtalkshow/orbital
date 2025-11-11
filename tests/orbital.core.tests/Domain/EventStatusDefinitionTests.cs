using FluentAssertions;
using orbital.core.Models;

namespace orbital.core.tests.Domain;

public class EventStatusDefinitionTests
{
    #region Creation and Default Values Tests

    [Fact]
    public void EventStatusDefinition_WhenCreated_ShouldHaveCorrectType()
    {
        // Arrange & Act
        var eventStatus = new EventStatusDefinition();

        // Assert
        eventStatus.Type.Should().Be("EventStatusType");
    }

    [Fact]
    public void EventStatusDefinition_WhenCreated_ShouldInheritDefaultValues()
    {
        // Arrange & Act
        var eventStatus = new EventStatusDefinition();

        // Assert
        eventStatus.Id.Should().BeEmpty();
        eventStatus.Value.Should().BeEmpty();
        eventStatus.DisplayName.Should().BeEmpty();
        eventStatus.Description.Should().BeEmpty();
        eventStatus.IsActive.Should().BeTrue();
        eventStatus.SortOrder.Should().Be(0);
    }

    [Fact]
    public void EventStatusDefinition_WhenCreated_RequiresPreviousStartDateShouldBeFalse()
    {
        // Arrange & Act
        var eventStatus = new EventStatusDefinition();

        // Assert
        eventStatus.RequiresPreviousStartDate.Should().BeFalse();
    }

    #endregion

    #region Property Behavior Tests

    [Fact]
    public void EventStatusDefinition_WhenValueIsSet_ShouldRetainValue()
    {
        // Arrange
        var value = "EventScheduled";

        // Act
        var eventStatus = new EventStatusDefinition { Value = value };

        // Assert
        eventStatus.Value.Should().Be(value);
    }

    [Fact]
    public void EventStatusDefinition_WhenDisplayNameIsSet_ShouldRetainValue()
    {
        // Arrange
        var displayName = "Scheduled";

        // Act
        var eventStatus = new EventStatusDefinition { DisplayName = displayName };

        // Assert
        eventStatus.DisplayName.Should().Be(displayName);
    }

    [Fact]
    public void EventStatusDefinition_WhenDescriptionIsSet_ShouldRetainValue()
    {
        // Arrange
        var description = "The event is scheduled to occur";

        // Act
        var eventStatus = new EventStatusDefinition { Description = description };

        // Assert
        eventStatus.Description.Should().Be(description);
    }

    [Fact]
    public void EventStatusDefinition_WhenRequiresPreviousStartDateIsSet_ShouldRetainValue()
    {
        // Arrange & Act
        var requiresDate = new EventStatusDefinition { RequiresPreviousStartDate = true };
        var notRequiresDate = new EventStatusDefinition { RequiresPreviousStartDate = false };

        // Assert
        requiresDate.RequiresPreviousStartDate.Should().BeTrue();
        notRequiresDate.RequiresPreviousStartDate.Should().BeFalse();
    }

    [Fact]
    public void EventStatusDefinition_WhenIsActiveIsSet_ShouldRetainValue()
    {
        // Arrange & Act
        var activeStatus = new EventStatusDefinition { IsActive = true };
        var inactiveStatus = new EventStatusDefinition { IsActive = false };

        // Assert
        activeStatus.IsActive.Should().BeTrue();
        inactiveStatus.IsActive.Should().BeFalse();
    }

    [Fact]
    public void EventStatusDefinition_WhenSortOrderIsSet_ShouldRetainValue()
    {
        // Arrange
        var sortOrder = 10;

        // Act
        var eventStatus = new EventStatusDefinition { SortOrder = sortOrder };

        // Assert
        eventStatus.SortOrder.Should().Be(sortOrder);
    }

    #endregion

    #region Type Consistency Tests

    [Fact]
    public void EventStatusDefinition_TypeShouldNotChangeWhenOtherPropertiesSet()
    {
        // Arrange
        var eventStatus = new EventStatusDefinition
        {
            Value = "EventScheduled",
            DisplayName = "Scheduled",
            Description = "Event is scheduled",
            IsActive = true,
            SortOrder = 1,
            RequiresPreviousStartDate = false
        };

        // Assert
        eventStatus.Type.Should().Be("EventStatusType");
    }

    [Fact]
    public void EventStatusDefinition_TypeCanBeOverridden()
    {
        // Arrange
        var customType = "CustomEventStatusType";

        // Act
        var eventStatus = new EventStatusDefinition { Type = customType };

        // Assert
        eventStatus.Type.Should().Be(customType);
    }

    #endregion

    #region Common Event Status Scenarios

    [Theory]
    [InlineData("EventScheduled", "Scheduled", "The event is scheduled to occur", false)]
    [InlineData("EventCancelled", "Cancelled", "The event has been cancelled", true)]
    [InlineData("EventRescheduled", "Rescheduled", "The event has been rescheduled", true)]
    [InlineData("EventPostponed", "Postponed", "The event has been postponed", true)]
    public void EventStatusDefinition_CommonScenarios_ShouldWorkCorrectly(
        string value, string displayName, string description, bool requiresPreviousStartDate)
    {
        // Arrange & Act
        var eventStatus = new EventStatusDefinition
        {
            Value = value,
            DisplayName = displayName,
            Description = description,
            RequiresPreviousStartDate = requiresPreviousStartDate
        };

        // Assert
        eventStatus.Value.Should().Be(value);
        eventStatus.DisplayName.Should().Be(displayName);
        eventStatus.Description.Should().Be(description);
        eventStatus.RequiresPreviousStartDate.Should().Be(requiresPreviousStartDate);
        eventStatus.Type.Should().Be("EventStatusType");
    }

    #endregion

    #region Business Logic Tests

    [Fact]
    public void EventStatusDefinition_CancelledStatus_ShouldRequirePreviousStartDate()
    {
        // Arrange & Act
        var cancelledStatus = new EventStatusDefinition
        {
            Value = "EventCancelled",
            DisplayName = "Cancelled",
            RequiresPreviousStartDate = true
        };

        // Assert
        cancelledStatus.RequiresPreviousStartDate.Should().BeTrue();
    }

    [Fact]
    public void EventStatusDefinition_RescheduledStatus_ShouldRequirePreviousStartDate()
    {
        // Arrange & Act
        var rescheduledStatus = new EventStatusDefinition
        {
            Value = "EventRescheduled",
            DisplayName = "Rescheduled",
            RequiresPreviousStartDate = true
        };

        // Assert
        rescheduledStatus.RequiresPreviousStartDate.Should().BeTrue();
    }

    [Fact]
    public void EventStatusDefinition_ScheduledStatus_ShouldNotRequirePreviousStartDate()
    {
        // Arrange & Act
        var scheduledStatus = new EventStatusDefinition
        {
            Value = "EventScheduled",
            DisplayName = "Scheduled",
            RequiresPreviousStartDate = false
        };

        // Assert
        scheduledStatus.RequiresPreviousStartDate.Should().BeFalse();
    }

    #endregion
}
