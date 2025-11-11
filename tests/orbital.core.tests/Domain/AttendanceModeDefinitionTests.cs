using FluentAssertions;
using orbital.core.Models;

namespace orbital.core.tests.Domain;

public class AttendanceModeDefinitionTests
{
    #region Creation and Default Values Tests

    [Fact]
    public void AttendanceModeDefinition_WhenCreated_ShouldHaveCorrectType()
    {
        // Arrange & Act
        var attendanceMode = new AttendanceModeDefinition();

        // Assert
        attendanceMode.Type.Should().Be("EventAttendanceModeEnumeration");
    }

    [Fact]
    public void AttendanceModeDefinition_WhenCreated_ShouldInheritDefaultValues()
    {
        // Arrange & Act
        var attendanceMode = new AttendanceModeDefinition();

        // Assert
        attendanceMode.Id.Should().BeEmpty();
        attendanceMode.Value.Should().BeEmpty();
        attendanceMode.DisplayName.Should().BeEmpty();
        attendanceMode.Description.Should().BeEmpty();
        attendanceMode.IsActive.Should().BeTrue();
        attendanceMode.SortOrder.Should().Be(0);
    }

    #endregion

    #region Property Behavior Tests

    [Fact]
    public void AttendanceModeDefinition_WhenValueIsSet_ShouldRetainValue()
    {
        // Arrange
        var value = "OfflineEventAttendanceMode";

        // Act
        var attendanceMode = new AttendanceModeDefinition { Value = value };

        // Assert
        attendanceMode.Value.Should().Be(value);
    }

    [Fact]
    public void AttendanceModeDefinition_WhenDisplayNameIsSet_ShouldRetainValue()
    {
        // Arrange
        var displayName = "Offline";

        // Act
        var attendanceMode = new AttendanceModeDefinition { DisplayName = displayName };

        // Assert
        attendanceMode.DisplayName.Should().Be(displayName);
    }

    [Fact]
    public void AttendanceModeDefinition_WhenDescriptionIsSet_ShouldRetainValue()
    {
        // Arrange
        var description = "The event is only accessible offline";

        // Act
        var attendanceMode = new AttendanceModeDefinition { Description = description };

        // Assert
        attendanceMode.Description.Should().Be(description);
    }

    [Fact]
    public void AttendanceModeDefinition_WhenIsActiveIsSet_ShouldRetainValue()
    {
        // Arrange & Act
        var activeMode = new AttendanceModeDefinition { IsActive = true };
        var inactiveMode = new AttendanceModeDefinition { IsActive = false };

        // Assert
        activeMode.IsActive.Should().BeTrue();
        inactiveMode.IsActive.Should().BeFalse();
    }

    [Fact]
    public void AttendanceModeDefinition_WhenSortOrderIsSet_ShouldRetainValue()
    {
        // Arrange
        var sortOrder = 5;

        // Act
        var attendanceMode = new AttendanceModeDefinition { SortOrder = sortOrder };

        // Assert
        attendanceMode.SortOrder.Should().Be(sortOrder);
    }

    #endregion

    #region Type Consistency Tests

    [Fact]
    public void AttendanceModeDefinition_TypeShouldNotChangeWhenOtherPropertiesSet()
    {
        // Arrange
        var attendanceMode = new AttendanceModeDefinition
        {
            Value = "OnlineEventAttendanceMode",
            DisplayName = "Online",
            Description = "Event is accessible online",
            IsActive = true,
            SortOrder = 1
        };

        // Assert
        attendanceMode.Type.Should().Be("EventAttendanceModeEnumeration");
    }

    [Fact]
    public void AttendanceModeDefinition_TypeCanBeOverridden()
    {
        // Arrange
        var customType = "CustomType";

        // Act
        var attendanceMode = new AttendanceModeDefinition { Type = customType };

        // Assert
        attendanceMode.Type.Should().Be(customType);
    }

    #endregion

    #region Common Attendance Mode Scenarios

    [Theory]
    [InlineData("OnlineEventAttendanceMode", "Online", "The event is accessible online")]
    [InlineData("OfflineEventAttendanceMode", "Offline", "The event is only accessible offline")]
    [InlineData("MixedEventAttendanceMode", "Mixed", "The event is accessible both online and offline")]
    public void AttendanceModeDefinition_CommonScenarios_ShouldWorkCorrectly(
        string value, string displayName, string description)
    {
        // Arrange & Act
        var attendanceMode = new AttendanceModeDefinition
        {
            Value = value,
            DisplayName = displayName,
            Description = description
        };

        // Assert
        attendanceMode.Value.Should().Be(value);
        attendanceMode.DisplayName.Should().Be(displayName);
        attendanceMode.Description.Should().Be(description);
        attendanceMode.Type.Should().Be("EventAttendanceModeEnumeration");
    }

    #endregion
}
