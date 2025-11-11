# TDD Guidelines for Orbital

## Overview

This document outlines the Test-Driven Development (TDD) practices, principles, and guidelines for the Orbital project. TDD is a core development methodology for this project, ensuring high-quality, maintainable code with comprehensive test coverage.

## Table of Contents

- [TDD Principles](#tdd-principles)
- [The Red-Green-Refactor Cycle](#the-red-green-refactor-cycle)
- [When to Use TDD](#when-to-use-tdd)
- [Test Structure and Patterns](#test-structure-and-patterns)
- [Naming Conventions](#naming-conventions)
- [Arrange-Act-Assert Pattern](#arrange-act-assert-pattern)
- [Test Data Builders](#test-data-builders)
- [Mocking Guidelines](#mocking-guidelines)
- [Testing Best Practices](#testing-best-practices)
- [Common Testing Scenarios](#common-testing-scenarios)
- [Performance Considerations](#performance-considerations)

## TDD Principles

Test-Driven Development is a software development approach where tests are written **before** the production code. The core principles are:

1. **Write Tests First**: Never write production code without a failing test
2. **Minimal Implementation**: Write only enough code to make the test pass
3. **Continuous Refactoring**: Improve code design while maintaining passing tests
4. **Fast Feedback**: Tests should run quickly to provide immediate feedback
5. **One Test at a Time**: Focus on one behavior per test

### Benefits of TDD

- **Better Design**: Forces you to think about interfaces and behavior first
- **Living Documentation**: Tests serve as executable specifications
- **Regression Safety**: Comprehensive test suite catches breaking changes
- **Confidence**: Safe refactoring with immediate feedback
- **Fewer Bugs**: Issues caught early in development

## The Red-Green-Refactor Cycle

TDD follows a strict cycle:

### 🔴 RED: Write a Failing Test

1. Identify the next small behavior to implement
2. Write a test that describes the expected behavior
3. Run the test and verify it **fails** for the right reason
4. The test should fail because the functionality doesn't exist yet

**Example:**
```csharp
[Fact]
public void Meeting_WhenCreated_ShouldHaveDefaultType()
{
    // Arrange & Act
    var meeting = new Meeting();

    // Assert
    meeting.Type.Should().Be("meeting"); // This will FAIL initially
}
```

### 🟢 GREEN: Make the Test Pass

1. Write the **minimum** code needed to pass the test
2. Don't worry about perfection or optimization yet
3. Run the test and verify it passes
4. Resist the urge to add extra functionality

**Example:**
```csharp
public class Meeting
{
    public string Type { get; set; } = "meeting"; // Minimal implementation
}
```

### 🔵 REFACTOR: Improve the Code

1. Clean up the code while keeping all tests green
2. Remove duplication
3. Improve naming and structure
4. Optimize performance if needed
5. Run tests frequently to ensure nothing breaks

**Example:**
```csharp
public class Meeting
{
    private const string DefaultType = "meeting";
    
    public string Type { get; set; } = DefaultType; // Better implementation
}
```

### The Cycle Continues

Repeat this cycle for each new behavior:
1. Write a failing test (RED)
2. Make it pass (GREEN)
3. Refactor (REFACTOR)
4. Commit
5. Next test...

## When to Use TDD

### Always Use TDD For:

- **New Features**: All new functionality should start with a test
- **Bug Fixes**: Write a test that reproduces the bug, then fix it
- **Core Business Logic**: Domain models, validation rules, calculations
- **API Endpoints**: Request/response behavior, status codes
- **Data Access**: Repository implementations, data transformations
- **Service Layer**: Business logic orchestration

### TDD May Not Be Required For:

- **UI/Visual Design**: Manual verification may be more appropriate
- **Exploratory Spikes**: Quick prototypes (but convert to TDD for production)
- **Configuration Files**: Simple configuration changes
- **Third-Party Integrations**: When mocking is complex or impractical

## Test Structure and Patterns

### Project Organization

The Orbital project follows a clear test organization:

```
tests/
├── orbital.core.tests/          # Fast unit tests for domain logic
│   └── Domain/                  # Domain model tests
│       ├── MeetingTests.cs
│       ├── MetadataDefinitionTests.cs
│       └── ...
├── orbital.api.tests/           # Unit tests for API endpoints
│   ├── MeetingEndpointsTests.cs
│   └── MetadataEndpointsTests.cs
└── orbital.test.shared/         # Shared test utilities
    └── TestDataBuilders.cs
```

### Test File Organization

Tests are organized using regions within each test class:

```csharp
public class MeetingTests
{
    #region Creation and Default Values Tests
    // Tests for object initialization
    #endregion

    #region Property Behavior Tests
    // Tests for property getters/setters
    #endregion

    #region Validation Tests
    // Tests for validation rules
    #endregion

    #region Business Logic Tests
    // Tests for domain-specific rules
    #endregion
}
```

## Naming Conventions

### Test Method Names

Follow the pattern: `[MethodUnderTest]_[Scenario]_[ExpectedResult]`

**Examples:**
```csharp
Meeting_WhenCreated_ShouldHaveDefaultType()
Meeting_WhenTitleIsEmpty_ShouldFailValidation()
GetMeetingById_WhenMeetingExists_ReturnsOkWithMeeting()
GetMeetingById_WhenMeetingDoesNotExist_ReturnsNotFound()
ValidateEndTime_WhenEndTimeBeforeStartTime_ReturnsValidationError()
```

### Test Class Names

Test classes should be named after the class they test with a `Tests` suffix:

```csharp
Meeting.cs → MeetingTests.cs
MeetingEndpoints.cs → MeetingEndpointsTests.cs
MetadataDefinition.cs → MetadataDefinitionTests.cs
```

## Arrange-Act-Assert Pattern

All tests should follow the **AAA (Arrange-Act-Assert)** pattern:

### Structure

```csharp
[Fact]
public void Method_Scenario_ExpectedResult()
{
    // Arrange - Set up test data and dependencies
    var input = "test data";
    var mockRepo = new Mock<IRepository>();
    mockRepo.Setup(r => r.GetData()).Returns("expected");

    // Act - Execute the method being tested
    var result = systemUnderTest.MethodToTest(input);

    // Assert - Verify expectations
    result.Should().Be("expected");
    mockRepo.Verify(r => r.GetData(), Times.Once);
}
```

### Guidelines

1. **Arrange**: Keep setup focused and minimal
2. **Act**: Typically a single line that calls the method under test
3. **Assert**: Verify the expected outcome and side effects
4. Use blank lines to separate the three sections for clarity

## Test Data Builders

Use the `TestDataBuilders` class from `orbital.test.shared` to create test objects:

### Meeting Builder

```csharp
var meeting = TestDataBuilders.CreateMeeting()
    .WithTitle("Team Standup")
    .WithDescription("Daily team sync")
    .WithTimes(startTime, endTime)
    .WithLocation("Conference Room A")
    .WithEventStatus("EventScheduled")
    .Build();
```

### MetadataDefinition Builder

```csharp
var metadata = TestDataBuilders.CreateMetadataDefinition()
    .WithType("EventStatus")
    .WithValue("EventScheduled")
    .WithDisplayName("Scheduled")
    .WithDescription("Event is scheduled")
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

### Benefits of Test Data Builders

- **Consistency**: All tests use valid default data
- **Readability**: Fluent interface makes intent clear
- **Maintainability**: Changes to object structure update in one place
- **Focus**: Only specify values relevant to the test

## Mocking Guidelines

Use **Moq** framework for creating mock objects in tests.

### When to Mock

Mock external dependencies that:
- Access databases
- Make network calls
- Interact with file systems
- Have complex setup requirements
- Are slow to execute

### When NOT to Mock

Don't mock:
- **Domain Objects**: Test real domain models
- **Value Objects**: Use actual instances
- **Simple DTOs**: Create real objects
- **The Class Under Test**: Only mock dependencies

### Basic Mocking

```csharp
// Create a mock
var mockRepository = new Mock<IMeetingRepository>();

// Setup a method to return a value
mockRepository
    .Setup(r => r.GetMeetingByIdAsync("123"))
    .ReturnsAsync(meeting);

// Setup a method to return null
mockRepository
    .Setup(r => r.GetMeetingByIdAsync("non-existent"))
    .ReturnsAsync((Meeting)null!);

// Verify a method was called
mockRepository.Verify(r => r.GetMeetingByIdAsync("123"), Times.Once);

// Verify a method was never called
mockRepository.Verify(r => r.DeleteMeetingAsync(It.IsAny<string>()), Times.Never);
```

### Advanced Mocking Patterns

```csharp
// Match any value
mockRepository
    .Setup(r => r.CreateMeetingAsync(It.IsAny<Meeting>()))
    .ReturnsAsync((Meeting m) => m);

// Match with predicate
mockRepository
    .Setup(r => r.UpdateMeetingAsync(It.Is<Meeting>(m => m.Title == "Test")))
    .ReturnsAsync(true);

// Throw an exception
mockRepository
    .Setup(r => r.GetMeetingByIdAsync("error"))
    .ThrowsAsync(new Exception("Database error"));

// Callback for side effects
var capturedMeeting = new Meeting();
mockRepository
    .Setup(r => r.CreateMeetingAsync(It.IsAny<Meeting>()))
    .Callback<Meeting>(m => capturedMeeting = m)
    .ReturnsAsync((Meeting m) => m);
```

### Mocking Best Practices

1. **Mock Interfaces**: Mock interfaces, not concrete classes
2. **Minimal Setups**: Only setup methods that will be called
3. **Verify Interactions**: Always verify important method calls
4. **Clear Intent**: Use descriptive variable names for mocks
5. **One Mock Per Dependency**: Don't reuse mocks across unrelated tests

## Testing Best Practices

### 1. Keep Tests Fast

- **Target**: All unit tests should run in under 1 second total
- **Individual Tests**: Should complete in milliseconds
- **No I/O**: Avoid database, network, or file system access
- **In-Memory Only**: Use in-memory data and mocks

### 2. One Assertion Focus Per Test

While multiple assertions are allowed, focus on one logical concept:

```csharp
// GOOD - Testing one concept with related assertions
[Fact]
public void Meeting_WhenCreated_ShouldHaveDefaultValues()
{
    var meeting = new Meeting();
    
    meeting.Type.Should().Be("meeting");
    meeting.Keywords.Should().BeEmpty();
    meeting.Attendees.Should().BeEmpty();
}

// LESS IDEAL - Testing multiple unrelated concepts
[Fact]
public void Meeting_MultipleBehaviors_VariousResults()
{
    var meeting = new Meeting();
    
    meeting.Type.Should().Be("meeting"); // Creation behavior
    meeting.Title = "Test";
    meeting.Title.Should().Be("Test"); // Property behavior
    // ... (too many unrelated concepts)
}
```

### 3. Test Behavior, Not Implementation

Focus on **what** the code does, not **how** it does it:

```csharp
// GOOD - Tests behavior
[Fact]
public void GetMeetingById_WhenMeetingExists_ReturnsOkWithMeeting()
{
    // Tests the contract: give ID, get meeting
    var result = await endpoint.GetMeetingById("123");
    result.Should().BeOfType<Ok<Meeting>>();
}

// AVOID - Tests implementation details
[Fact]
public void GetMeetingById_CallsRepositoryGetMethod()
{
    // Too focused on implementation
    await endpoint.GetMeetingById("123");
    mockRepo.Verify(r => r.GetMeetingByIdAsync("123"));
}
```

### 4. Use Descriptive Test Names

Test names should clearly describe the scenario:

```csharp
// GOOD
Meeting_WhenTitleExceedsMaxLength_ShouldFailValidation()
GetAllMeetings_WhenNoMeetingsExist_ReturnsOkWithEmptyList()

// AVOID
TestTitle()
TestMeetingValidation()
Test1()
```

### 5. Test Edge Cases and Boundaries

Always test boundary conditions:

```csharp
[Theory]
[InlineData("")]           // Empty string
[InlineData("A")]          // Minimum valid
[InlineData("A string with exactly 100 characters...")]  // Maximum valid
[InlineData("A string with 101 characters...")]          // Just over max
public void Meeting_TitleLengthValidation_ReturnsExpectedResult(string title)
{
    // Test implementation
}
```

### 6. Avoid Test Interdependencies

Each test should be independent:

```csharp
// GOOD - Independent test
[Fact]
public void Test1()
{
    var meeting = TestDataBuilders.CreateMeeting().Build();
    // Test logic
}

// AVOID - Depends on shared state
private Meeting _sharedMeeting; // Don't do this

[Fact]
public void Test1()
{
    _sharedMeeting = new Meeting(); // Modified by other tests
}
```

### 7. Use FluentAssertions

Use FluentAssertions for readable assertions:

```csharp
// GOOD - Fluent and readable
result.Should().Be(expected);
meeting.Title.Should().NotBeNullOrEmpty();
meetings.Should().HaveCount(3);
result.Should().BeOfType<Ok<Meeting>>();

// AVOID - Less readable
Assert.Equal(expected, result);
Assert.True(!string.IsNullOrEmpty(meeting.Title));
Assert.Equal(3, meetings.Count);
```

### 8. Test Error Conditions

Always test error scenarios:

```csharp
[Fact]
public void CreateMeeting_WhenTitleIsEmpty_ShouldFailValidation()
{
    // Arrange
    var meeting = new Meeting { Title = "" };
    var context = new ValidationContext(meeting);
    var results = new List<ValidationResult>();

    // Act
    var isValid = Validator.TryValidateObject(meeting, context, results, true);

    // Assert
    isValid.Should().BeFalse();
    results.Should().Contain(r => r.ErrorMessage!.Contains("Title is required"));
}
```

### 9. Use Theory for Multiple Scenarios

Use `[Theory]` with `[InlineData]` for testing multiple inputs:

```csharp
[Theory]
[InlineData("EventScheduled", true)]
[InlineData("EventCancelled", true)]
[InlineData("", false)]
[InlineData(null, false)]
public void EventStatus_Validation_ReturnsExpectedResult(string status, bool isValid)
{
    var meeting = TestDataBuilders.CreateMeeting()
        .WithEventStatus(status)
        .Build();
    
    var result = ValidateMeeting(meeting);
    result.IsValid.Should().Be(isValid);
}
```

### 10. Clean Up Test Code

Refactor tests just like production code:

- Extract common setup to helper methods
- Use test builders for complex objects
- Remove duplication
- Keep tests readable and maintainable

## Common Testing Scenarios

### Testing Domain Models

```csharp
[Fact]
public void Meeting_WhenEndTimeBeforeStartTime_ShouldFailValidation()
{
    // Arrange
    var startTime = TestDataBuilders.GetTestDateTime();
    var endTime = startTime.AddHours(-1); // Before start
    var meeting = TestDataBuilders.CreateMeeting()
        .WithTimes(startTime, endTime)
        .Build();

    var context = new ValidationContext(meeting);
    var results = new List<ValidationResult>();

    // Act
    var isValid = Validator.TryValidateObject(meeting, context, results, true);

    // Assert
    isValid.Should().BeFalse();
    results.Should().Contain(r => 
        r.ErrorMessage!.Contains("End time must be after start time"));
}
```

### Testing API Endpoints

```csharp
[Fact]
public async Task GetMeetingById_WhenMeetingExists_ReturnsOkWithMeeting()
{
    // Arrange
    var meetingId = "test-123";
    var meeting = TestDataBuilders.CreateMeeting()
        .WithId(meetingId)
        .Build();
    _mockRepository.Setup(r => r.GetMeetingByIdAsync(meetingId))
        .ReturnsAsync(meeting);

    // Act
    var result = await InvokeGetMeetingByIdEndpoint(meetingId, _mockRepository.Object);

    // Assert
    result.Should().BeOfType<Ok<Meeting>>();
    var okResult = (Ok<Meeting>)result;
    okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
    okResult.Value.Should().BeEquivalentTo(meeting);
    _mockRepository.Verify(r => r.GetMeetingByIdAsync(meetingId), Times.Once);
}
```

### Testing Validation Attributes

```csharp
[Fact]
public void Meeting_WhenTitleIsTooLong_ShouldFailValidation()
{
    // Arrange
    var tooLongTitle = new string('A', 101); // Max is 100
    var meeting = TestDataBuilders.CreateMeeting()
        .WithTitle(tooLongTitle)
        .Build();

    var context = new ValidationContext(meeting);
    var results = new List<ValidationResult>();

    // Act
    var isValid = Validator.TryValidateObject(meeting, context, results, true);

    // Assert
    isValid.Should().BeFalse();
    results.Should().Contain(r => 
        r.ErrorMessage!.Contains("Title cannot exceed 100 characters"));
}
```

### Testing Collections

```csharp
[Fact]
public void GetAllMeetings_WhenMultipleMeetingsExist_ReturnsAllInCorrectOrder()
{
    // Arrange
    var meetings = new List<Meeting>
    {
        TestDataBuilders.CreateMeeting().WithId("1").Build(),
        TestDataBuilders.CreateMeeting().WithId("2").Build(),
        TestDataBuilders.CreateMeeting().WithId("3").Build()
    };
    _mockRepository.Setup(r => r.ListMeetingsAsync())
        .ReturnsAsync(meetings);

    // Act
    var result = await endpoint.GetAllMeetings();

    // Assert
    result.Should().HaveCount(3);
    result.Should().BeEquivalentTo(meetings);
}
```

### Testing Null Handling

```csharp
[Fact]
public void ProcessMeeting_WhenMeetingIsNull_ThrowsArgumentNullException()
{
    // Arrange
    Meeting nullMeeting = null!;

    // Act
    Action act = () => service.ProcessMeeting(nullMeeting);

    // Assert
    act.Should().Throw<ArgumentNullException>()
        .WithParameterName("meeting");
}
```

## Performance Considerations

### Fast Test Execution

- **Unit Tests**: Should complete in milliseconds
- **Test Suite**: Total execution under 1 second
- **No External Dependencies**: Use mocks instead of real databases
- **Parallel Execution**: xUnit runs tests in parallel by default

### Optimizing Test Performance

```csharp
// GOOD - Fast, in-memory test
[Fact]
public void Meeting_Validation_IsPerformant()
{
    var meeting = TestDataBuilders.CreateMeeting().Build();
    var context = new ValidationContext(meeting);
    var results = new List<ValidationResult>();
    
    var isValid = Validator.TryValidateObject(meeting, context, results, true);
    // Completes in microseconds
}

// AVOID - Slow test with database
[Fact]
public async Task Meeting_DatabaseTest_IsSlow()
{
    await database.SaveMeetingAsync(meeting); // Slow!
    var result = await database.GetMeetingAsync(id);
    // Takes milliseconds to seconds
}
```

### Test Isolation

Each test should:
- Create its own test data
- Use fresh mocks
- Not depend on execution order
- Not share state with other tests

## Running Tests

### Command Line

```bash
# Run all core tests
dotnet test tests/orbital.core.tests/orbital.core.tests.csproj

# Run all API tests
dotnet test tests/orbital.api.tests/orbital.api.tests.csproj

# Run all tests in solution
dotnet test

# Run with detailed output
dotnet test --verbosity normal

# Run specific test class
dotnet test --filter "FullyQualifiedName~MeetingTests"

# Run specific test method
dotnet test --filter "FullyQualifiedName~Meeting_WhenCreated_ShouldHaveDefaultType"

# List all tests without running
dotnet test --list-tests
```

### Visual Studio Code

- Tests are automatically discovered in Test Explorer
- Run individual tests by clicking the play button
- Debug tests by clicking the debug icon
- View test results inline

## Continuous Integration

Tests are automatically run on:
- Every commit
- Pull request creation
- Pull request updates
- Merge to main branch

### CI Requirements

- All tests must pass before merge
- Test coverage must meet minimum thresholds
- No test warnings or errors
- Fast execution (under configured timeout)

## Summary

TDD is not just about testing—it's a design methodology that leads to better software:

1. **Write tests first** to clarify requirements
2. **Make them pass** with minimal code
3. **Refactor** to improve design
4. **Repeat** for each behavior

By following these guidelines, you'll create a robust, well-tested, and maintainable codebase that evolves safely over time.

## Additional Resources

- [Testing Architecture Documentation](./Testing-Architecture.md)
- [TDD Coach Usage Guide](./TDD-Coach-Usage.md)
- [orbital.core.tests README](../tests/orbital.core.tests/README.md)
- [orbital.api.tests README](../tests/orbital.api.tests/README.md)
