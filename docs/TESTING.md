# Testing Guide

This document explains the testing strategy and how to run different types of tests in the Orbital project.

## Test Organization

### Unit Tests
Fast, isolated tests that test individual components with dependencies mocked or faked.

- **tests/orbital.core.tests**: Domain model and business logic tests
- **tests/orbital.api.tests**: API endpoint logic tests (using mocks)
- **orbital.test.api**: HTTP client service tests (using Moq)

### Integration Tests
Tests that exercise multiple components together, including full HTTP request/response cycles.

- **tests/orbital.integration.tests**: Full API integration tests with in-memory data storage

## Running Tests

### Run All Tests
```bash
dotnet test
```

### Run Only Unit Tests
```bash
# Run all unit test projects
dotnet test tests/orbital.core.tests
dotnet test tests/orbital.api.tests
dotnet test orbital.test.api
```

### Run Only Integration Tests
```bash
dotnet test tests/orbital.integration.tests
```

### Run Tests with Filters
```bash
# Run tests matching a specific name pattern
dotnet test --filter "FullyQualifiedName~Meeting"

# Run tests from a specific category (using Traits if configured)
dotnet test --filter "Category=Integration"
```

### Run Tests with Verbosity
```bash
# Detailed output
dotnet test --logger "console;verbosity=detailed"

# Normal output
dotnet test --logger "console;verbosity=normal"
```

## CI/CD Configuration

### Recommended Pipeline Structure

```yaml
# Example GitHub Actions or Azure DevOps pipeline
steps:
  - name: Unit Tests
    run: |
      dotnet test tests/orbital.core.tests
      dotnet test tests/orbital.api.tests
      dotnet test orbital.test.api
    
  - name: Integration Tests
    run: dotnet test tests/orbital.integration.tests
```

### Separate Test Execution Benefits

1. **Fast Feedback**: Unit tests run quickly, providing immediate feedback
2. **Parallel Execution**: Different test suites can run in parallel
3. **Targeted Debugging**: Easier to identify which layer has issues
4. **Selective Execution**: Run only relevant tests for code changes

## Test Categories

| Project | Type | Speed | Dependencies | Count |
|---------|------|-------|--------------|-------|
| orbital.core.tests | Unit | Fast | None | 69 |
| orbital.api.tests | Unit | Fast | Mocks only | 20 |
| orbital.test.api | Unit | Fast | HTTP mocks | 13 |
| orbital.integration.tests | Integration | Medium | In-memory DB | 6 |

## Best Practices

### When to Write Unit Tests
- Testing business logic and domain models
- Testing individual service methods
- Testing validation rules
- Testing data transformations

### When to Write Integration Tests
- Testing full API endpoints
- Testing database interactions
- Testing external service integrations
- Testing complete workflows

### Test Naming
- Unit tests: `{MethodName}_{Scenario}_{ExpectedBehavior}`
- Integration tests: `{Feature}_{Scenario}_{ExpectedOutcome}`

## Troubleshooting

### Integration Tests Failing
- Ensure you're not running with a real database connection
- Check that test environment is properly configured
- Verify in-memory repositories are registered

### Unit Tests Failing
- Check that all dependencies are properly mocked
- Verify test data setup
- Ensure no external dependencies are being called

## Future Enhancements

- [ ] Add test categories/traits for filtering
- [ ] Set up code coverage reporting
- [ ] Add performance/load testing
- [ ] Add mutation testing
- [ ] Add test containers for real database testing
