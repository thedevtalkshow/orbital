# Integration Tests

This project contains integration tests for the Orbital application that test end-to-end functionality across API boundaries.

## Purpose

Integration tests verify that different parts of the system work together correctly. Unlike unit tests that test isolated components with mocks, integration tests:

- Exercise full HTTP request/response cycles
- Test actual API endpoints and routing
- Validate serialization/deserialization
- Ensure proper integration between layers
- Test with realistic (though simplified) data storage

## Test Categories

### API Integration Tests (`Api/`)
Full HTTP endpoint testing using `WebApplicationFactory`:
- Complete request/response cycles
- HTTP status code validation
- Response payload verification
- Error handling scenarios

### Database Integration Tests (Future)
Tests with actual database connections:
- Data persistence verification
- Transaction handling
- Query performance
- Data integrity constraints

### External Service Integration Tests (Future)
Tests for external API dependencies:
- Third-party service integration
- API contract validation
- Resilience and retry logic

## Test Infrastructure

### `Infrastructure/InMemoryMeetingRepository`
Provides fast, isolated test execution without external database dependencies. Pre-populated with test data for consistent test scenarios.

### `Infrastructure/ApiIntegrationTestBase`
Base class for API integration tests that:
- Configures `WebApplicationFactory` with test dependencies
- Replaces external dependencies (like CosmosDB) with in-memory implementations
- Provides configured `HttpClient` for API calls
- Ensures proper test isolation

## Running Integration Tests

### Run All Integration Tests
```bash
dotnet test tests/orbital.integration.tests
```

### Run Specific Test Category
```bash
dotnet test tests/orbital.integration.tests --filter "FullyQualifiedName~Api"
```

### Run in CI/CD
Integration tests are designed to run in CI/CD pipelines without external dependencies. They use in-memory implementations to ensure fast, reliable execution.

## Best Practices

### Test Isolation
- Each test should be independent and not rely on state from other tests
- Use fresh test data or reset state between tests
- Avoid shared mutable state

### Test Data
- Use meaningful test data that represents realistic scenarios
- Document any special test data requirements
- Clean up test data after test execution (if using real database)

### Naming Conventions
- Test class names: `{Feature}IntegrationTests`
- Test method names: `{Method}_{Scenario}_{ExpectedBehavior}`
- Example: `CreateMeeting_WithValidData_ReturnsCreated`

### Assertions
- Use FluentAssertions for readable assertions
- Include helpful assertion messages explaining why the assertion should pass
- Test both happy path and error scenarios

### Performance
- Integration tests are slower than unit tests - keep them focused
- Avoid testing edge cases already covered by unit tests
- Use appropriate timeouts for async operations

## Separation from Unit Tests

Integration tests are separate from unit tests to:
- Allow running fast unit tests frequently during development
- Run slower integration tests less frequently or in CI/CD
- Clearly distinguish between test types and their purposes
- Enable different test configurations and infrastructure

## Future Enhancements

- [ ] Test container support for real database testing
- [ ] Performance and load testing utilities
- [ ] External service mocking infrastructure
- [ ] Test data builders and fixtures
- [ ] Database seeding and cleanup utilities
