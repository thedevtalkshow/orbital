# Orbital Core Tests - Testing Guidelines

## Overview
This document outlines the testing patterns, conventions, and best practices for the Orbital core domain tests.

## Test Naming Conventions

All tests follow the pattern: `[MethodUnderTest]_[Scenario]_[ExpectedResult]`

### Examples:
```csharp
Meeting_WhenCreated_ShouldHaveDefaultType()
Meeting_SetEndTimeBeforeStartTime_ShouldFailValidation()
ValidateEndTime_WhenEndTimeAfterStartTime_ReturnsSuccess()
```

## Test Organization

Tests are organized by domain entity in the `Domain` folder:
- **MeetingTests.cs** - Tests for Meeting entity
- **MetadataDefinitionTests.cs** - Tests for MetadataDefinition
- **AttendanceModeDefinitionTests.cs** - Tests for AttendanceModeDefinition
- **EventStatusDefinitionTests.cs** - Tests for EventStatusDefinition

### Test Regions

Tests are grouped into logical regions within each test class:

1. **Creation and Default Values Tests** - Test object initialization and default state
2. **Property Behavior Tests** - Test property getters/setters
3. **Validation Tests** - Test data validation rules (split by property for Meeting entity)
4. **Custom Validation Method Tests** - Test custom validation methods
5. **Business Logic Tests** - Test domain-specific business rules
6. **Interface Implementation Tests** - Test interface conformance

## Test Pattern (Arrange-Act-Assert)

All tests follow the AAA (Arrange-Act-Assert) pattern:

```csharp
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
```

## Test Data Builders

Use test data builders from `orbital.test.shared.TestDataBuilders` for creating test objects:

### Meeting Builder
```csharp
var meeting = TestDataBuilders.CreateMeeting()
    .WithTitle("Team Standup")
    .WithTimes(startTime, endTime)
    .WithLocation("Conference Room A")
    .Build();
```

### MetadataDefinition Builder
```csharp
var metadata = TestDataBuilders.CreateMetadataDefinition()
    .WithType("EventStatus")
    .WithValue("EventScheduled")
    .WithDisplayName("Scheduled")
    .Build();
```

### Helper Methods
```csharp
// Get a test DateTime (9 AM UTC + offset)
var startTime = TestDataBuilders.GetTestDateTime();
var endTime = TestDataBuilders.GetTestDateTime(1); // 10 AM UTC

// Get a random string for testing
var randomId = TestDataBuilders.GetRandomString(10);
```

## Testing Validation Attributes

When testing DataAnnotations validation, use the following pattern:

```csharp
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
```

## Theory Tests for Multiple Scenarios

Use `[Theory]` with `[InlineData]` for testing multiple scenarios:

```csharp
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
}
```

## FluentAssertions Usage

Use FluentAssertions for readable test assertions:

```csharp
// Simple equality
result.Should().Be(expected);

// Null checks
result.Should().NotBeNull();
result.Should().BeNull();

// Boolean checks
result.Should().BeTrue();
result.Should().BeFalse();

// String checks
text.Should().BeEmpty();
text.Should().NotBeEmpty();
error.Should().Contain("expected text");

// Collection checks
collection.Should().NotBeNull().And.BeEmpty();
collection.Should().BeEquivalentTo(expectedCollection);

// Type checks
instance.Should().BeAssignableTo<IInterface>();
```

## Custom Validation Methods

When testing custom validation methods directly:

```csharp
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
```

## Performance Requirements

- All core domain tests must complete in under 1 second total
- Individual tests should complete in milliseconds
- Avoid external dependencies (databases, network calls, file I/O)
- Use in-memory test data only

## Test Coverage Goals

Each domain entity should have tests covering:

1. ✅ Default values and initialization
2. ✅ Property getters and setters
3. ✅ Required field validation
4. ✅ String length validation (min/max)
5. ✅ Custom validation logic
6. ✅ Business rules
7. ✅ Edge cases (boundary values, null handling)
8. ✅ Interface implementation (where applicable)

## Current Test Statistics

- **Total Tests**: 69
- **Test Execution Time**: ~53ms
- **Coverage**:
  - Meeting: 24 tests
  - MetadataDefinition: 11 tests
  - AttendanceModeDefinition: 8 tests
  - EventStatusDefinition: 16 tests

## Adding New Tests

When adding new domain models or extending existing ones:

1. Create a new test file: `[EntityName]Tests.cs` in the `Domain` folder
2. Follow the established naming conventions
3. Organize tests using regions
4. Use the AAA pattern
5. Leverage test data builders
6. Ensure tests run quickly (< 1s total)
7. Validate with `dotnet test`

## Running Tests

```bash
# Run all core tests
dotnet test tests/orbital.core.tests/orbital.core.tests.csproj

# Run with detailed output
dotnet test tests/orbital.core.tests/orbital.core.tests.csproj --logger "console;verbosity=detailed"

# Run specific test class
dotnet test --filter "FullyQualifiedName~MeetingTests"

# Run specific test
dotnet test --filter "FullyQualifiedName~Meeting_SetEndTimeBeforeStartTime_ShouldFailValidation"
```

## Best Practices

1. **Keep tests focused** - Each test should verify one specific behavior
2. **Use meaningful names** - Test names should clearly describe what they test
3. **Avoid test interdependencies** - Tests should be able to run in any order
4. **Use builders for complex objects** - Reduce test setup duplication
5. **Test edge cases** - Boundary values, null inputs, empty collections
6. **Make assertions explicit** - Use descriptive FluentAssertions methods
7. **Comment when necessary** - Only if the test logic isn't self-explanatory
8. **Keep tests maintainable** - Refactor test code like production code

## Example: Complete Test Class Structure

```csharp
using FluentAssertions;
using orbital.core.Models;

namespace orbital.core.tests.Domain;

public class NewEntityTests
{
    #region Creation and Default Values Tests

    [Fact]
    public void NewEntity_WhenCreated_ShouldHaveDefaultValues()
    {
        // Arrange & Act
        var entity = new NewEntity();

        // Assert
        entity.Id.Should().BeEmpty();
        entity.IsActive.Should().BeTrue();
    }

    #endregion

    #region Property Behavior Tests

    [Fact]
    public void NewEntity_WhenPropertyIsSet_ShouldRetainValue()
    {
        // Arrange
        var value = "test";

        // Act
        var entity = new NewEntity { Property = value };

        // Assert
        entity.Property.Should().Be(value);
    }

    #endregion

    #region Validation Tests

    [Theory]
    [InlineData("", false)]
    [InlineData("Valid", true)]
    public void NewEntity_PropertyValidation_ReturnsExpectedResult(string value, bool isValid)
    {
        // Test implementation
    }

    #endregion
}
```
