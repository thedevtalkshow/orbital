# TDD Test Projects

This directory contains test projects optimized for Test-Driven Development (TDD) workflow with fast feedback loops.

## Project Structure

```
tests/
├── orbital.core.tests/      # Fast unit tests (TDD focus)
│   └── Domain/              # Domain model tests
└── orbital.test.shared/     # Shared test utilities
```

## Projects

### orbital.core.tests
Pure unit tests for the core domain logic. This project is optimized for TDD workflow:
- **Fast execution**: Tests run in < 1 second
- **Minimal dependencies**: Only xUnit, Moq, FluentAssertions
- **Focus**: Core domain logic and business rules

**Usage:**
```bash
# Run all core tests
dotnet test tests/orbital.core.tests

# Run with detailed output
dotnet test tests/orbital.core.tests --verbosity normal

# List all tests
dotnet test tests/orbital.core.tests --list-tests
```

### orbital.test.shared
Shared test utilities and helpers used across test projects.
- Test data builders
- Common test fixtures
- Shared test utilities

## Dependencies

All test projects use:
- **xUnit** - Test framework
- **Moq** - Mocking framework
- **FluentAssertions** - Fluent assertion library

## TDD Workflow

1. Write a failing test in `orbital.core.tests`
2. Run tests to verify failure
3. Implement minimum code to pass the test
4. Refactor while keeping tests green
5. Repeat

## Best Practices

- Keep tests fast (< 1 second for the entire suite)
- One assertion per test (when practical)
- Use descriptive test names (GivenWhenThen or similar)
- Organize tests by domain area
- Use shared utilities from `orbital.test.shared`

## VS Code Integration

Tests are automatically discovered by the VS Code Test Explorer. You can:
- Run individual tests
- Debug tests
- View test results inline
