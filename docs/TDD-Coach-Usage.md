# TDD Coach Agent Usage Guide

## Overview

The TDD Coach is a specialized GitHub Copilot chat mode designed specifically for the Orbital project. It enforces strict Test-Driven Development (TDD) practices and guides developers through the Red-Green-Refactor cycle.

## Table of Contents

- [What is the TDD Coach?](#what-is-the-tdd-coach)
- [When to Use TDD Coach](#when-to-use-tdd-coach)
- [Accessing TDD Coach](#accessing-tdd-coach)
- [Core Principles](#core-principles)
- [How TDD Coach Works](#how-tdd-coach-works)
- [Example Workflows](#example-workflows)
- [Best Practices](#best-practices)
- [Common Scenarios](#common-scenarios)
- [Tips and Tricks](#tips-and-tricks)
- [Troubleshooting](#troubleshooting)

## What is the TDD Coach?

The TDD Coach is a **custom GitHub Copilot agent** that acts as your personal TDD mentor. It:

- 🚫 **Refuses** to write production code without a failing test
- ✅ **Guides** you through the Red-Green-Refactor cycle
- 🎓 **Teaches** TDD best practices specific to Orbital
- 🔍 **Reviews** your test structure and quality
- 🏃 **Runs** tests using the `runTests` tool to verify cycles
- 💡 **Suggests** edge cases and test scenarios

### Key Features

- **Strict TDD Enforcement**: Won't let you skip the test-first approach
- **Project Context**: Understands Orbital's domain and architecture
- **Interactive Guidance**: Asks clarifying questions about requirements
- **Pattern Recommendations**: Suggests appropriate test patterns
- **Continuous Verification**: Runs tests to confirm Red-Green-Refactor

## When to Use TDD Coach

### Perfect For:

✅ **New Feature Development**
- Implementing new API endpoints
- Adding domain model properties
- Creating new business logic
- Building new services

✅ **Bug Fixes**
- Writing tests that reproduce bugs
- Verifying fixes don't break existing functionality
- Adding regression tests

✅ **Refactoring**
- Ensuring tests stay green during refactoring
- Improving code design safely
- Removing technical debt

✅ **Learning TDD**
- Understanding the Red-Green-Refactor cycle
- Practicing TDD with guided support
- Building good TDD habits

### Not Ideal For:

❌ **UI/Visual Changes**
- Frontend layout adjustments
- Style modifications
- Visual design iterations

❌ **Documentation**
- Writing README files
- Creating API documentation
- Updating comments

❌ **Configuration**
- Changing app settings
- Updating environment variables
- Modifying build scripts

❌ **Exploratory Work**
- Researching solutions
- Prototyping ideas
- Investigating issues

## Accessing TDD Coach

### In GitHub Copilot Chat

1. Open GitHub Copilot Chat in your IDE
2. Type `@workspace` to access agents
3. Select or type **"TDD Coach"** mode
4. Ask your question or describe your feature

### Quick Start Command

```
@workspace /tdd I need to add validation to ensure meeting end time is after start time
```

### Alternative Access

You can also mention TDD Coach in your prompt:
```
Using TDD Coach mode, help me implement a new Speaker model
```

## Core Principles

The TDD Coach enforces these non-negotiable rules:

### 1. RED FIRST - No Production Code Without a Test

```
❌ Wrong:
You: "Write a method to validate meeting times"
TDD Coach: "I'm a TDD coach, so let's write the failing test first!"

✅ Right:
You: "Let's write a test for meeting time validation"
TDD Coach: "Great! Here's a failing test for that behavior..."
```

### 2. GREEN - Write Minimal Code to Pass

The coach will remind you to:
- Write only enough code to make the test pass
- Resist adding extra features
- Keep implementation simple

### 3. REFACTOR - Improve While Staying Green

After tests pass, the coach helps you:
- Identify code smells
- Suggest refactoring opportunities
- Ensure tests remain green

### 4. NO EXCEPTIONS

The coach won't compromise on TDD principles:
- No "simple methods" that skip tests
- No "quick fixes" without verification
- No production code before failing tests

## How TDD Coach Works

### The TDD Coaching Cycle

```
┌─────────────────────────────────────────────────────┐
│ 1. YOU: Describe the behavior you want              │
└─────────────┬───────────────────────────────────────┘
              │
              ▼
┌─────────────────────────────────────────────────────┐
│ 2. COACH: Asks clarifying questions                 │
│    - What should happen when...?                    │
│    - What edge cases should we consider?            │
│    - What's the expected behavior for...?           │
└─────────────┬───────────────────────────────────────┘
              │
              ▼
┌─────────────────────────────────────────────────────┐
│ 3. COACH: Guides you to write a FAILING test        │
│    - Suggests test structure                        │
│    - Recommends test data                           │
│    - Helps with mocking strategy                    │
└─────────────┬───────────────────────────────────────┘
              │
              ▼
┌─────────────────────────────────────────────────────┐
│ 4. COACH: Runs test to verify it FAILS              │
│    Uses runTests tool: ❌ Test fails as expected    │
└─────────────┬───────────────────────────────────────┘
              │
              ▼
┌─────────────────────────────────────────────────────┐
│ 5. COACH: Helps write MINIMAL implementation        │
│    - Just enough to pass the test                   │
│    - No extra features                              │
└─────────────┬───────────────────────────────────────┘
              │
              ▼
┌─────────────────────────────────────────────────────┐
│ 6. COACH: Runs test to verify it PASSES             │
│    Uses runTests tool: ✅ Test passes               │
└─────────────┬───────────────────────────────────────┘
              │
              ▼
┌─────────────────────────────────────────────────────┐
│ 7. COACH: Suggests refactoring improvements         │
│    - Code quality enhancements                      │
│    - Design pattern opportunities                   │
│    - Performance optimizations                      │
└─────────────┬───────────────────────────────────────┘
              │
              ▼
┌─────────────────────────────────────────────────────┐
│ 8. REPEAT: Next behavior or edge case               │
└─────────────────────────────────────────────────────┘
```

### Automatic Test Execution

The TDD Coach can run tests using the `runTests` tool:
- Verifies tests fail in the RED phase
- Confirms tests pass in the GREEN phase
- Ensures refactoring doesn't break tests

## Example Workflows

### Example 1: Adding a New Domain Property

**You:**
```
I need to add a speaker name to the Meeting model
```

**TDD Coach:**
```
Great! Let's use TDD to add this feature. First, let me ask a few questions:

1. Should the speaker name be required or optional?
2. Is there a maximum length for the speaker name?
3. Should we validate the format (e.g., no special characters)?

Once we clarify these, we'll write a failing test that describes the expected behavior.
```

**You:**
```
Speaker name should be optional, max 100 characters, any characters allowed
```

**TDD Coach:**
```
Perfect! Let's start with a failing test. Here's what we'll test first:

[Provides test code for default empty speaker name]

Let me run this test to verify it fails...
[Runs test with runTests tool]
❌ Test fails as expected - Meeting doesn't have SpeakerName property

Now, let's write the minimal code to make this pass:
[Provides minimal implementation]

Let me verify it passes...
[Runs test with runTests tool]
✅ Test passes!

Now, let's add a test for the max length validation...
```

### Example 2: Fixing a Bug

**You:**
```
There's a bug - meetings can be created with end time before start time
```

**TDD Coach:**
```
Perfect opportunity for TDD! Let's write a test that reproduces the bug first.

Here's a failing test that captures the expected behavior:

```csharp
[Fact]
public void Meeting_WhenEndTimeBeforeStartTime_ShouldFailValidation()
{
    // Arrange
    var startTime = DateTime.UtcNow;
    var endTime = startTime.AddHours(-1); // Before start!
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

Let me run this test to verify it fails (confirming the bug exists)...
[Runs test - should fail because validation doesn't exist]

Now that we've confirmed the bug, let's implement the validation...
```

### Example 3: Adding an API Endpoint

**You:**
```
Need to add an endpoint to delete a meeting by ID
```

**TDD Coach:**
```
Excellent! Let's use TDD to build this endpoint. First, some questions:

1. What should the endpoint return when the meeting exists and is deleted successfully?
2. What should happen if the meeting doesn't exist?
3. Should we validate permissions (e.g., only organizer can delete)?

Based on your answers, we'll write tests for each scenario.
```

**You:**
```
1. Return 204 No Content
2. Return 404 Not Found
3. Not yet, just focus on basic deletion
```

**TDD Coach:**
```
Perfect! Let's start with the happy path test:

```csharp
[Fact]
public async Task DeleteMeeting_WhenMeetingExists_ReturnsNoContent()
{
    // Arrange
    var meetingId = "test-123";
    _mockRepository.Setup(r => r.DeleteMeetingAsync(meetingId))
        .ReturnsAsync(true);

    // Act
    var result = await InvokeDeleteMeetingEndpoint(meetingId, _mockRepository.Object);

    // Assert
    result.Should().BeOfType<NoContent>();
    _mockRepository.Verify(r => r.DeleteMeetingAsync(meetingId), Times.Once);
}
```

This test will fail because the endpoint doesn't exist yet. 
Let me verify...

[Runs test]
❌ Failed - DeleteMeetingAsync method doesn't exist

Now let's implement the minimal code...
```

## Best Practices

### 1. Be Specific About Behavior

❌ **Vague:**
```
Add validation to Meeting
```

✅ **Specific:**
```
Add validation to ensure Meeting.Title is required and max 100 characters
```

### 2. Describe One Behavior at a Time

❌ **Too Much:**
```
Add speaker name, bio, twitter handle, and photo URL to Meeting
```

✅ **Focused:**
```
Add speaker name property to Meeting with max 100 character validation
```

### 3. Ask About Edge Cases

```
I need to validate email addresses. What edge cases should I test?
```

The coach will suggest:
- Empty string
- Null value
- Invalid format
- Valid format
- Maximum length
- Special characters

### 4. Request Pattern Recommendations

```
What's the best way to test this repository method that calls the database?
```

The coach will guide you on:
- Mocking strategies
- Test data setup
- Verification approaches

### 5. Use the Coach for Learning

```
Why do we test behavior instead of implementation?
```

The coach explains TDD principles in context.

## Common Scenarios

### Scenario: Coach Refuses to Write Code

**You:**
```
Write a method to calculate meeting duration
```

**Coach:**
```
I'm a TDD coach, so let's write the failing test first! 

What should the method do?
- Accept start and end times?
- Return a TimeSpan?
- Handle same start/end times?

Let me help you create a test that captures the expected behavior.
```

**Action**: Describe the behavior and let the coach guide you to write the test first.

### Scenario: Test Passes Immediately

**You:**
```
The test passed on the first run, but I haven't written the implementation yet!
```

**Coach:**
```
That's a problem! A test that passes without implementation isn't testing anything new. This could mean:

1. The functionality already exists
2. The test isn't actually testing the new behavior
3. The assertion is incorrect

Let's review the test to ensure it actually verifies the behavior we want to add.
```

**Action**: Review and fix the test to ensure it truly fails before implementation.

### Scenario: Asking for Help with Mocking

**You:**
```
How do I mock a repository that returns a list of meetings?
```

**Coach:**
```
Great question! Here's how to mock that with Moq:

```csharp
var meetings = new List<Meeting>
{
    TestDataBuilders.CreateMeeting().WithId("1").Build(),
    TestDataBuilders.CreateMeeting().WithId("2").Build()
};

_mockRepository
    .Setup(r => r.ListMeetingsAsync())
    .ReturnsAsync(meetings);
```

This setup makes the mock return your test data when the method is called.
Always verify the interaction:

```csharp
_mockRepository.Verify(r => r.ListMeetingsAsync(), Times.Once);
```
```

### Scenario: Refactoring Guidance

**You:**
```
Tests are green. What should I refactor?
```

**Coach:**
```
Great job getting to green! Let's look for refactoring opportunities:

1. **Duplication**: Any repeated code?
2. **Naming**: Are names clear and descriptive?
3. **Magic Values**: Any hardcoded values that should be constants?
4. **Complexity**: Any methods that are too long or complex?
5. **SOLID Principles**: Single responsibility, etc.

Show me the code and I'll suggest specific improvements.
```

## Tips and Tricks

### Tip 1: Start Small

Begin with the simplest possible test:
```csharp
[Fact]
public void Meeting_WhenCreated_ShouldNotBeNull()
{
    var meeting = new Meeting();
    meeting.Should().NotBeNull();
}
```

Even obvious tests help establish the TDD rhythm.

### Tip 2: One Assertion Per Concept

Focus each test on one logical concept:
```csharp
// GOOD - One concept: default values
[Fact]
public void Meeting_WhenCreated_ShouldHaveDefaultValues()
{
    var meeting = new Meeting();
    meeting.Type.Should().Be("meeting");
    meeting.Keywords.Should().BeEmpty();
    meeting.Attendees.Should().BeEmpty();
}
```

### Tip 3: Use Theory for Multiple Inputs

Let the coach help with `[Theory]` tests:
```
I need to test title validation with various inputs
```

Coach will suggest using `[Theory]` with `[InlineData]`.

### Tip 4: Let Coach Verify Tests

Ask the coach to run tests:
```
Can you run the tests to verify this works?
```

The coach uses the `runTests` tool to check.

### Tip 5: Ask "What Next?"

After completing a cycle:
```
Test is green and refactored. What should we test next?
```

The coach will suggest:
- Related edge cases
- Error conditions
- Alternative scenarios

## Troubleshooting

### Problem: Coach is Too Strict

**Symptom**: Coach won't let you write any production code

**Solution**: This is by design! TDD Coach enforces test-first strictly. If you need to write code without tests (e.g., for exploration), exit TDD Coach mode and use regular Copilot.

### Problem: Don't Know What Test to Write

**Symptom**: Stuck on what behavior to test first

**Solution**: Ask the coach:
```
I need to implement [feature]. What should I test first?
```

The coach will help break down the feature into testable units.

### Problem: Test Setup is Complex

**Symptom**: Test arrange section is very long

**Solution**: Ask the coach:
```
This test setup is complex. How can I simplify it?
```

The coach will suggest:
- Using test builders
- Extracting helper methods
- Simplifying test scenarios

### Problem: Not Sure How to Mock

**Symptom**: Confused about mocking strategy

**Solution**: Ask specifically:
```
How do I mock a repository that throws an exception?
```

The coach will provide concrete examples.

### Problem: Tests Run Slowly

**Symptom**: Tests take too long

**Solution**: Ask the coach:
```
These tests are slow. How can I speed them up?
```

The coach will identify issues:
- External dependencies
- Database calls
- File I/O
- Unnecessary waits

## Advanced Usage

### Pair Programming with TDD Coach

Use the coach as a pair programming partner:

1. **You**: Write the test
2. **Coach**: Reviews test quality
3. **Coach**: Suggests improvements
4. **You**: Implement the code
5. **Coach**: Verifies implementation is minimal
6. **Both**: Refactor together

### Using Coach for Code Review

Ask the coach to review your tests:
```
Review these tests and suggest improvements:
[paste test code]
```

The coach will check for:
- Test structure (AAA pattern)
- Naming conventions
- Edge case coverage
- Proper mocking
- Clear assertions

### Learning TDD Patterns

Ask the coach about patterns:
```
What's the best way to test validation attributes?
```

```
Show me examples of testing async methods
```

```
How should I structure tests for a repository?
```

## Integration with Development Workflow

### Daily Development Flow

1. **Start Feature**: Open TDD Coach
2. **Discuss Behavior**: Clarify requirements
3. **Write Tests**: Let coach guide TDD cycle
4. **Implement**: Follow Red-Green-Refactor
5. **Review**: Ask coach for refactoring suggestions
6. **Complete**: Exit coach when feature is done

### Team Collaboration

- **Onboarding**: New developers use coach to learn TDD
- **Pair Programming**: Two developers + TDD Coach
- **Code Review**: Reference coach recommendations
- **Standards**: Coach enforces consistent test patterns

## Summary

The TDD Coach is your dedicated mentor for Test-Driven Development in the Orbital project:

✅ **Enforces** strict TDD discipline
✅ **Guides** through Red-Green-Refactor cycle
✅ **Teaches** project-specific patterns
✅ **Verifies** tests fail and pass appropriately
✅ **Suggests** improvements and edge cases

### Remember:

1. **Test First**: Always write a failing test
2. **Minimal Code**: Just enough to pass
3. **Refactor**: Improve while tests are green
4. **Ask Questions**: The coach is here to help
5. **One Step**: Focus on one behavior at a time

### Getting Started

```
@workspace In TDD Coach mode, I need to add [describe your feature]
```

Happy TDD! 🚀

## Additional Resources

- [TDD Guidelines](./TDD-Guidelines.md)
- [Testing Architecture](./Testing-Architecture.md)
- [orbital.core.tests README](../tests/orbital.core.tests/README.md)
- [orbital.api.tests README](../tests/orbital.api.tests/README.md)
