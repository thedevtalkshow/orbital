# Testing Architecture for Orbital

## Overview

This document describes the testing architecture, project structure, and organization of tests in the Orbital meeting management system. Understanding this architecture is essential for contributing to the project and maintaining high-quality test coverage.

## Table of Contents

- [Test Project Structure](#test-project-structure)
- [Project Descriptions](#project-descriptions)
- [Dependencies and Frameworks](#dependencies-and-frameworks)
- [Test Organization Patterns](#test-organization-patterns)
- [Test Execution Strategy](#test-execution-strategy)
- [Integration with CI/CD](#integration-with-cicd)

## Test Project Structure

The Orbital solution follows a clear separation of test concerns:

```
orbital/
├── orbital.core/              # Domain models and interfaces
├── orbital.api/               # API endpoints
├── orbital.data/              # Data access layer
├── orbital.web/               # Blazor frontend
├── orbital.test/              # Integration tests (legacy)
├── orbital.test.api/          # Integration tests (legacy)
└── tests/
    ├── orbital.core.tests/    # ⚡ Fast unit tests for domain logic
    ├── orbital.api.tests/     # ⚡ Fast unit tests for API endpoints
    └── orbital.test.shared/   # 🔧 Shared test utilities
```

### Test Project Philosophy

The `tests/` directory contains projects optimized for **Test-Driven Development (TDD)**:

- **Speed**: All tests complete in under 1 second
- **Isolation**: No external dependencies (database, network, file system)
- **Focus**: Each project tests a specific layer
- **Feedback**: Immediate results for rapid iteration

## Project Descriptions

### orbital.core.tests

**Purpose**: Fast unit tests for core domain models and business logic

**Location**: `tests/orbital.core.tests/`

**Focus Areas**:
- Domain model validation
- Business rules and logic
- Entity behavior
- Value object semantics
- Interface contracts

**Characteristics**:
- ⚡ Lightning fast execution (< 1 second total)
- 🎯 Pure unit tests with no mocking required
- 📦 Minimal dependencies (xUnit, Moq, FluentAssertions)
- 🧪 Perfect for TDD workflow

**Test Structure**:
```
orbital.core.tests/
├── Domain/
│   ├── MeetingTests.cs
│   ├── MetadataDefinitionTests.cs
│   ├── AttendanceModeDefinitionTests.cs
│   └── EventStatusDefinitionTests.cs
└── orbital.core.tests.csproj
```

**Example Test**:
```csharp
[Fact]
public void Meeting_WhenCreated_ShouldHaveDefaultType()
{
    // Arrange & Act
    var meeting = new Meeting();

    // Assert
    meeting.Type.Should().Be("meeting");
}
```

**Test Count**: 69 tests covering all domain models

**Execution Time**: ~53ms

### orbital.api.tests

**Purpose**: Fast unit tests for API endpoints with mocked dependencies

**Location**: `tests/orbital.api.tests/`

**Focus Areas**:
- API endpoint behavior
- HTTP status codes
- Request/response validation
- Repository interaction verification
- Error handling

**Characteristics**:
- ⚡ Fast execution with mocked repositories
- 🎭 Uses Moq for dependency mocking
- 🔍 Tests endpoint logic in isolation
- ✅ Validates HTTP results and status codes

**Test Structure**:
```
orbital.api.tests/
├── MeetingEndpointsTests.cs
├── MetadataEndpointsTests.cs
└── orbital.api.tests.csproj
```

**Example Test**:
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

**Key Differences from Integration Tests**:
- Uses **mocked** repositories instead of real databases
- Tests **endpoint logic** in isolation
- **Much faster** execution (no database setup/teardown)
- **Simpler** test setup

### orbital.test.shared

**Purpose**: Shared test utilities, builders, and helpers used across test projects

**Location**: `tests/orbital.test.shared/`

**Contents**:
- Test data builders (fluent API for creating test objects)
- Common test fixtures
- Shared helper methods
- Test constants and utilities

**Characteristics**:
- 🔧 Reusable test infrastructure
- 🏗️ Builder pattern for test data
- 📐 Consistent test data across projects
- 🎯 Reduces duplication in tests

**Main Component**: `TestDataBuilders.cs`

**Available Builders**:

#### MeetingBuilder
```csharp
var meeting = TestDataBuilders.CreateMeeting()
    .WithId("custom-id")
    .WithTitle("Team Standup")
    .WithDescription("Daily sync")
    .WithTimes(startTime, endTime)
    .WithLocation("Room 101")
    .WithEventStatus("EventScheduled")
    .WithAttendanceMode("OnlineEventAttendanceMode")
    .WithKeywords(new[] { "standup", "team" })
    .Build();
```

#### MetadataDefinitionBuilder
```csharp
var metadata = TestDataBuilders.CreateMetadataDefinition()
    .WithType("EventStatus")
    .WithValue("EventScheduled")
    .WithDisplayName("Scheduled")
    .WithDescription("Event is scheduled to take place")
    .WithSchemaOrgUrl("https://schema.org/EventScheduled")
    .Build();
```

#### Helper Methods
```csharp
// Get a test DateTime (9 AM UTC + offset)
var startTime = TestDataBuilders.GetTestDateTime();
var endTime = TestDataBuilders.GetTestDateTime(1); // 10 AM UTC

// Get a random string
var randomId = TestDataBuilders.GetRandomString(10);
```

**Benefits**:
- **Consistency**: All tests use valid, realistic data
- **Readability**: Clear, fluent syntax
- **Maintainability**: One place to update test data
- **Flexibility**: Only specify values relevant to test

### Legacy Test Projects

#### orbital.test

**Purpose**: Legacy integration tests for the distributed application

**Status**: Being migrated to new TDD-focused structure

**Note**: New tests should be added to `tests/orbital.core.tests` or `tests/orbital.api.tests`

#### orbital.test.api

**Purpose**: Legacy integration tests with WebApplicationFactory

**Status**: Being superseded by `tests/orbital.api.tests`

**Difference**: Uses real infrastructure vs. mocked dependencies

## Dependencies and Frameworks

### Core Testing Frameworks

All test projects use a consistent set of testing tools:

| Package | Version | Purpose |
|---------|---------|---------|
| **xUnit** | Latest | Test framework and test runner |
| **xunit.runner.visualstudio** | Latest | Visual Studio integration |
| **Moq** | Latest | Mocking framework for dependencies |
| **FluentAssertions** | Latest | Readable assertion syntax |
| **Microsoft.NET.Test.Sdk** | Latest | .NET test infrastructure |

### Project References

Test projects reference:

- **orbital.core.tests**:
  - `orbital.core` (domain models)
  - `orbital.test.shared` (test utilities)

- **orbital.api.tests**:
  - `orbital.api` (API endpoints)
  - `orbital.core` (domain models)
  - `orbital.test.shared` (test utilities)

### Why These Tools?

**xUnit**:
- Modern, extensible test framework
- Parallel test execution by default
- Excellent .NET integration
- Industry standard for .NET projects

**Moq**:
- Fluent, readable mocking syntax
- Easy verification of interactions
- Supports complex mocking scenarios
- Well-documented and maintained

**FluentAssertions**:
- Natural, readable assertion syntax
- Better error messages than standard assertions
- Rich set of assertion extensions
- Improves test maintainability

## Test Organization Patterns

### File Organization

Each test class corresponds to a production class:

```
Production Code          Test Code
─────────────────────────────────────────────
Meeting.cs          →    MeetingTests.cs
MeetingEndpoints.cs →    MeetingEndpointsTests.cs
MetadataDefinition.cs →  MetadataDefinitionTests.cs
```

### Within Test Files

Tests are organized using `#region` directives:

```csharp
public class MeetingTests
{
    #region Creation and Default Values Tests
    
    [Fact]
    public void Meeting_WhenCreated_ShouldHaveDefaultType() { }
    
    #endregion

    #region Property Behavior Tests
    
    [Fact]
    public void Meeting_WhenTitleIsSet_ShouldRetainValue() { }
    
    #endregion

    #region Title Validation Tests
    
    [Theory]
    [InlineData("", false)]
    [InlineData("Valid", true)]
    public void Meeting_TitleValidation_ReturnsExpectedResult() { }
    
    #endregion

    #region Business Logic Tests
    
    [Fact]
    public void Meeting_CalculateDuration_ReturnsCorrectValue() { }
    
    #endregion
}
```

### Common Regions

1. **Creation and Default Values Tests**: Object initialization
2. **Property Behavior Tests**: Property getters/setters
3. **Validation Tests**: Data validation rules (often split by property)
4. **Custom Validation Method Tests**: Custom validation logic
5. **Business Logic Tests**: Domain-specific rules
6. **Interface Implementation Tests**: Interface contract compliance

### Naming Conventions

**Test Method Names**: `[MethodUnderTest]_[Scenario]_[ExpectedResult]`

Examples:
- `Meeting_WhenCreated_ShouldHaveDefaultType`
- `GetMeetingById_WhenMeetingExists_ReturnsOkWithMeeting`
- `ValidateEndTime_WhenEndTimeBeforeStartTime_ReturnsValidationError`

**Test Class Names**: `[ClassUnderTest]Tests`

Examples:
- `MeetingTests`
- `MeetingEndpointsTests`
- `MetadataDefinitionTests`

## Test Execution Strategy

### Local Development

**Fast Feedback Loop**:
```bash
# Run specific test project (< 1 second)
dotnet test tests/orbital.core.tests

# Run all TDD tests (< 2 seconds)
dotnet test tests/

# Watch mode for continuous testing
dotnet watch test --project tests/orbital.core.tests
```

**IDE Integration**:
- Visual Studio: Test Explorer with one-click run/debug
- VS Code: Test Explorer extension with inline results
- Rider: Integrated test runner

### Test Execution Order

xUnit executes tests:
1. **In Parallel**: By default (within each test class)
2. **Randomly**: No guaranteed order
3. **Isolated**: Each test is independent

**Implication**: Tests must not depend on execution order or shared state

### Performance Targets

| Test Type | Target Execution Time |
|-----------|----------------------|
| Individual Unit Test | < 10ms |
| Test Class | < 100ms |
| Test Project | < 1 second |
| All TDD Tests | < 2 seconds |

### Achieving Fast Tests

**Do**:
- Use in-memory data
- Mock external dependencies
- Keep test setup minimal
- Avoid Thread.Sleep or delays

**Don't**:
- Access databases
- Make network calls
- Read/write files
- Start external processes

## Integration with CI/CD

### Continuous Integration

Tests run automatically on:
- Every commit to feature branches
- Pull request creation
- Pull request updates
- Merge to main branch

### CI Pipeline Stages

1. **Restore**: Download NuGet packages
2. **Build**: Compile all projects
3. **Test**: Run all test projects
4. **Report**: Generate test results and coverage

### CI Configuration

```yaml
# Example GitHub Actions workflow
- name: Run Tests
  run: dotnet test --no-build --verbosity normal
  
- name: Test Coverage
  run: dotnet test --collect:"XPlat Code Coverage"
```

### Quality Gates

All of the following must pass before merge:
- ✅ All tests pass
- ✅ No test warnings
- ✅ Code coverage meets threshold
- ✅ Build succeeds
- ✅ Code quality checks pass

### Test Results Reporting

CI generates:
- **Test Summary**: Pass/fail counts
- **Test Details**: Individual test results
- **Coverage Report**: Code coverage metrics
- **Trend Analysis**: Test performance over time

## Test Coverage Goals

### Overall Coverage Targets

- **Core Domain Models**: 100% coverage
- **API Endpoints**: 95%+ coverage
- **Business Logic**: 100% coverage
- **Data Access**: 90%+ coverage (with integration tests)

### Coverage by Project

| Project | Target Coverage | Focus |
|---------|----------------|-------|
| orbital.core | 100% | All domain logic |
| orbital.api | 95% | Endpoint behavior |
| orbital.data | 90% | Repository implementations |

### What to Test

**Must Test**:
- All public APIs
- Business rules and validation
- Error conditions and edge cases
- Domain model behavior
- Critical user workflows

**May Skip**:
- Auto-generated code
- Simple property getters/setters
- Trivial wrapper methods
- Third-party library code

## Adding New Tests

### When Adding a New Feature

1. **Create Test File**: Match the production class name
2. **Set Up Structure**: Use appropriate regions
3. **Write Failing Test**: Follow TDD Red-Green-Refactor
4. **Use Test Builders**: Leverage `TestDataBuilders`
5. **Follow Patterns**: Match existing test style
6. **Verify Speed**: Ensure fast execution

### Example: Adding Tests for New Domain Model

```csharp
// 1. Create new test file: SpeakerTests.cs
using FluentAssertions;
using orbital.core.Models;
using orbital.test.shared;

namespace orbital.core.tests.Domain;

public class SpeakerTests
{
    #region Creation and Default Values Tests
    
    [Fact]
    public void Speaker_WhenCreated_ShouldHaveEmptyId()
    {
        // Arrange & Act
        var speaker = new Speaker();
        
        // Assert
        speaker.Id.Should().BeEmpty();
    }
    
    #endregion
    
    #region Validation Tests
    
    [Theory]
    [InlineData("", false)]
    [InlineData("Valid Name", true)]
    public void Speaker_NameValidation_ReturnsExpectedResult(
        string name, bool isValid)
    {
        // Test implementation
    }
    
    #endregion
}
```

### Adding to Test Builders

```csharp
// In TestDataBuilders.cs
public class SpeakerBuilder
{
    private string _id = Guid.NewGuid().ToString();
    private string _name = "Test Speaker";
    // ... other fields
    
    public SpeakerBuilder WithName(string name)
    {
        _name = name;
        return this;
    }
    
    public Speaker Build()
    {
        return new Speaker
        {
            Id = _id,
            Name = _name,
            // ... other properties
        };
    }
}

public static SpeakerBuilder CreateSpeaker() => new SpeakerBuilder();
```

## Best Practices Summary

### Test Project Organization
- ✅ Keep test projects focused and fast
- ✅ Use shared test utilities
- ✅ Mirror production code structure
- ✅ Organize tests with regions

### Test Implementation
- ✅ Follow Arrange-Act-Assert pattern
- ✅ Use descriptive test names
- ✅ One logical concept per test
- ✅ Test behavior, not implementation
- ✅ Use test builders for consistency

### Test Execution
- ✅ Keep tests fast (< 1 second total)
- ✅ Ensure test isolation
- ✅ Run tests frequently
- ✅ Fix failing tests immediately

### Test Maintenance
- ✅ Refactor tests like production code
- ✅ Remove duplication
- ✅ Update tests when requirements change
- ✅ Review test coverage regularly

## Running All Tests

### Quick Reference

```bash
# Run all TDD tests
dotnet test tests/

# Run specific project
dotnet test tests/orbital.core.tests
dotnet test tests/orbital.api.tests

# Run with detailed output
dotnet test tests/ --verbosity normal

# Run specific test class
dotnet test --filter "FullyQualifiedName~MeetingTests"

# Run specific test
dotnet test --filter "Meeting_WhenCreated_ShouldHaveDefaultType"

# List all tests
dotnet test --list-tests

# Watch mode
dotnet watch test --project tests/orbital.core.tests
```

### Visual Studio / VS Code

- Open Test Explorer
- Click "Run All Tests"
- View results inline
- Debug failing tests

## Summary

The Orbital testing architecture is designed to support Test-Driven Development with:

- **Fast feedback**: Tests complete in under 1 second
- **Clear organization**: Logical project and file structure
- **Consistent patterns**: Standardized approach across all tests
- **Shared utilities**: Reusable test builders and helpers
- **CI/CD integration**: Automated testing on every change

By following this architecture, developers can confidently practice TDD and maintain high-quality code with comprehensive test coverage.

## Additional Resources

- [TDD Guidelines](./TDD-Guidelines.md)
- [TDD Coach Usage Guide](./TDD-Coach-Usage.md)
- [orbital.core.tests README](../tests/orbital.core.tests/README.md)
- [orbital.api.tests README](../tests/orbital.api.tests/README.md)
- [Test Project README](../tests/README.md)
