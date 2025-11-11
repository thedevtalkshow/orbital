using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using orbital.core;
using orbital.integration.tests.Infrastructure;

namespace orbital.integration.tests.Api;

/// <summary>
/// Integration tests for Meeting API endpoints.
/// These tests exercise the full HTTP request/response cycle with in-memory data storage.
/// </summary>
public class MeetingApiIntegrationTests : ApiIntegrationTestBase
{
    public MeetingApiIntegrationTests(WebApplicationFactory<Program> factory) : base(factory)
    {
    }

    [Theory]
    [InlineData("/api/meetings", HttpStatusCode.OK)]
    [InlineData("/api/meetings/1", HttpStatusCode.OK)]
    public async Task GetMeetings_ReturnsExpectedStatusCode(string targetUri, HttpStatusCode expectedStatusCode)
    {
        // Act
        var response = await Client.GetAsync(targetUri);

        // Assert
        response.StatusCode.Should().Be(expectedStatusCode);
    }

    [Fact]
    public async Task GetAllMeetings_ReturnsCorrectCount()
    {
        // Act
        var response = await Client.GetAsync("/api/meetings");
        response.EnsureSuccessStatusCode();

        var meetings = await response.Content.ReadFromJsonAsync<IEnumerable<Meeting>>();

        // Assert
        meetings.Should().NotBeNull();
        meetings.Should().HaveCount(2, "the in-memory repository initializes with 2 test meetings");
    }

    [Fact]
    public async Task CreateMeeting_ReturnsCreatedStatusCode()
    {
        // Arrange
        var newMeeting = new Meeting
        {
            Id = "3",
            Title = "Test Meeting",
            Description = "This is a test meeting.",
            StartTime = DateTime.UtcNow,
            EndTime = DateTime.UtcNow.AddHours(1)
        };

        // Act
        var response = await Client.PostAsJsonAsync("/api/meetings", newMeeting);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created, "creating a valid meeting should return 201 Created");
        
        var createdMeeting = await response.Content.ReadFromJsonAsync<Meeting>();
        createdMeeting.Should().NotBeNull();
        createdMeeting!.Id.Should().Be(newMeeting.Id);
        createdMeeting.Title.Should().Be(newMeeting.Title);
    }

    [Fact]
    public async Task GetMeetingById_WithValidId_ReturnsMeeting()
    {
        // Act
        var response = await Client.GetAsync("/api/meetings/1");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var meeting = await response.Content.ReadFromJsonAsync<Meeting>();
        meeting.Should().NotBeNull();
        meeting!.Id.Should().Be("1");
        meeting.Title.Should().Be("Test Meeting 1");
    }

    [Fact]
    public async Task UpdateMeeting_WithValidData_ReturnsOk()
    {
        // Arrange
        var updatedMeeting = new Meeting
        {
            Id = "1",
            Title = "Updated Meeting Title",
            Description = "Updated description",
            StartTime = DateTime.UtcNow,
            EndTime = DateTime.UtcNow.AddHours(2)
        };

        // Act
        var response = await Client.PutAsJsonAsync("/api/meetings/1", updatedMeeting);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var result = await response.Content.ReadFromJsonAsync<Meeting>();
        result.Should().NotBeNull();
        result!.Title.Should().Be(updatedMeeting.Title);
    }
}
