# orbital.api.tests

Unit tests for the API layer endpoints with mocked dependencies.

## Overview

This project contains **unit tests** for the API layer endpoints defined in `orbital.api`. These tests focus on testing the endpoint behavior in isolation by mocking repository dependencies.

## Key Differences from orbital.test.api

- **orbital.api.tests**: Unit tests with mocked dependencies (fast, isolated)
- **orbital.test.api**: Integration tests using WebApplicationFactory (slower, requires infrastructure)

## Test Structure

### MeetingEndpointsTests
Tests for meeting-related endpoints with mocked `IMeetingRepository`:
- `GetAllMeetings`: List all meetings
- `GetMeetingById`: Get a specific meeting
- `CreateMeeting`: Create a new meeting
- `UpdateMeeting`: Update an existing meeting

### MetadataEndpointsTests
Tests for metadata-related endpoints with mocked `IMetadataRepository`:
- `GetEventStatuses`: Get event status definitions
- `GetAttendanceModes`: Get attendance mode definitions
- `GetMetadataByType`: Get metadata by type
- `CreateMetadataItem`: Create new metadata (admin endpoint)
- `UpdateMetadataItem`: Update existing metadata (admin endpoint)
- `DeleteMetadataItem`: Delete metadata (admin endpoint)

## Test Patterns

All tests follow the **Arrange-Act-Assert** pattern:

```csharp
[Fact]
public async Task GetMeetingById_WhenMeetingExists_ReturnsOkWithMeeting()
{
    // Arrange - Set up mocks and test data
    var meetingId = "test-id-123";
    var meeting = TestDataBuilders.CreateMeeting()
        .WithId(meetingId)
        .Build();
    _mockRepository.Setup(r => r.GetMeetingByIdAsync(meetingId)).ReturnsAsync(meeting);

    // Act - Execute the endpoint logic
    var result = await InvokeGetMeetingByIdEndpoint(meetingId, _mockRepository.Object);

    // Assert - Verify the results
    result.Should().BeOfType<Ok<Meeting>>();
    var okResult = (Ok<Meeting>)result;
    okResult.Value.Should().BeEquivalentTo(meeting);
    _mockRepository.Verify(r => r.GetMeetingByIdAsync(meetingId), Times.Once);
}
```

## Dependencies

- **xUnit**: Test framework
- **Moq**: Mocking framework for repository dependencies
- **FluentAssertions**: Readable assertion syntax
- **orbital.api**: The API project being tested
- **orbital.core**: Domain models and interfaces
- **orbital.test.shared**: Shared test utilities (TestDataBuilders)

## Running Tests

```bash
# Run all API unit tests
dotnet test tests/orbital.api.tests

# Run with detailed output
dotnet test tests/orbital.api.tests --verbosity normal

# Run specific test class
dotnet test tests/orbital.api.tests --filter "FullyQualifiedName~MeetingEndpointsTests"
```

## Test Coverage Focus

✅ Endpoint behavior validation
✅ Status code verification (200 OK, 201 Created, 404 Not Found, 400 Bad Request, etc.)
✅ Response payload validation
✅ Repository interaction verification
✅ Error handling and edge cases
✅ Input validation

## Best Practices

1. **Mock Dependencies**: All repository dependencies are mocked using Moq
2. **Test Isolation**: Each test is independent and can run in any order
3. **Fast Execution**: No database or external dependencies
4. **Clear Naming**: Test names follow the pattern `MethodName_Scenario_ExpectedResult`
5. **Use Test Builders**: Leverage `TestDataBuilders` from `orbital.test.shared` for consistent test data
6. **Verify Interactions**: Always verify that mocked methods are called the expected number of times
