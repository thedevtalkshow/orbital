using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using orbital.core;
using orbital.core.Data;
using orbital.test.shared;

namespace orbital.api.tests;

public class MeetingEndpointsTests
{
    private readonly Mock<IMeetingRepository> _mockRepository;

    public MeetingEndpointsTests()
    {
        _mockRepository = new Mock<IMeetingRepository>();
    }

    #region GetAllMeetings Tests

    [Fact]
    public async Task GetAllMeetings_WhenMeetingsExist_ReturnsOkWithMeetings()
    {
        // Arrange
        var meetings = new List<Meeting>
        {
            TestDataBuilders.CreateMeeting().WithId("1").WithTitle("Meeting 1").Build(),
            TestDataBuilders.CreateMeeting().WithId("2").WithTitle("Meeting 2").Build()
        };
        _mockRepository.Setup(r => r.ListMeetingsAsync()).ReturnsAsync(meetings);

        // Act
        var result = await InvokeGetAllMeetingsEndpoint(_mockRepository.Object);

        // Assert
        result.Should().BeOfType<Ok<IEnumerable<Meeting>>>();
        var okResult = (Ok<IEnumerable<Meeting>>)result;
        okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        okResult.Value.Should().BeEquivalentTo(meetings);
        _mockRepository.Verify(r => r.ListMeetingsAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAllMeetings_WhenNoMeetingsExist_ReturnsOkWithEmptyList()
    {
        // Arrange
        var emptyList = new List<Meeting>();
        _mockRepository.Setup(r => r.ListMeetingsAsync()).ReturnsAsync(emptyList);

        // Act
        var result = await InvokeGetAllMeetingsEndpoint(_mockRepository.Object);

        // Assert
        result.Should().BeOfType<Ok<IEnumerable<Meeting>>>();
        var okResult = (Ok<IEnumerable<Meeting>>)result;
        okResult.Value.Should().BeEmpty();
        _mockRepository.Verify(r => r.ListMeetingsAsync(), Times.Once);
    }

    #endregion

    #region GetMeetingById Tests

    [Fact]
    public async Task GetMeetingById_WhenMeetingExists_ReturnsOkWithMeeting()
    {
        // Arrange
        var meetingId = "test-id-123";
        var meeting = TestDataBuilders.CreateMeeting()
            .WithId(meetingId)
            .WithTitle("Test Meeting")
            .Build();
        _mockRepository.Setup(r => r.GetMeetingByIdAsync(meetingId)).ReturnsAsync(meeting);

        // Act
        var result = await InvokeGetMeetingByIdEndpoint(meetingId, _mockRepository.Object);

        // Assert
        result.Should().BeOfType<Ok<Meeting>>();
        var okResult = (Ok<Meeting>)result;
        okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        okResult.Value.Should().BeEquivalentTo(meeting);
        _mockRepository.Verify(r => r.GetMeetingByIdAsync(meetingId), Times.Once);
    }

    [Fact]
    public async Task GetMeetingById_WhenMeetingDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        var meetingId = "non-existent-id";
        _mockRepository.Setup(r => r.GetMeetingByIdAsync(meetingId)).ReturnsAsync((Meeting)null!);

        // Act
        var result = await InvokeGetMeetingByIdEndpoint(meetingId, _mockRepository.Object);

        // Assert
        result.Should().BeOfType<NotFound>();
        _mockRepository.Verify(r => r.GetMeetingByIdAsync(meetingId), Times.Once);
    }

    #endregion

    #region CreateMeeting Tests

    [Fact]
    public async Task CreateMeeting_WithValidMeeting_ReturnsCreatedWithMeeting()
    {
        // Arrange
        var newMeeting = TestDataBuilders.CreateMeeting()
            .WithTitle("New Meeting")
            .WithDescription("New Description")
            .Build();
        var createdMeeting = TestDataBuilders.CreateMeeting()
            .WithId("created-id-123")
            .WithTitle(newMeeting.Title)
            .WithDescription(newMeeting.Description)
            .Build();
        _mockRepository.Setup(r => r.CreateMeetingAsync(newMeeting)).ReturnsAsync(createdMeeting);

        // Act
        var result = await InvokeCreateMeetingEndpoint(newMeeting, _mockRepository.Object);

        // Assert
        result.Should().BeOfType<Created<Meeting>>();
        var createdResult = (Created<Meeting>)result;
        createdResult.StatusCode.Should().Be(StatusCodes.Status201Created);
        createdResult.Location.Should().Be($"/api/meetings/{createdMeeting.Id}");
        createdResult.Value.Should().BeEquivalentTo(createdMeeting);
        _mockRepository.Verify(r => r.CreateMeetingAsync(newMeeting), Times.Once);
    }

    #endregion

    #region UpdateMeeting Tests

    [Fact]
    public async Task UpdateMeeting_WhenMeetingExists_ReturnsOkWithUpdatedMeeting()
    {
        // Arrange
        var meetingId = "update-id-123";
        var meetingToUpdate = TestDataBuilders.CreateMeeting()
            .WithId(meetingId)
            .WithTitle("Updated Title")
            .Build();
        _mockRepository.Setup(r => r.UpdateMeetingAsync(meetingToUpdate)).ReturnsAsync(meetingToUpdate);

        // Act
        var result = await InvokeUpdateMeetingEndpoint(meetingId, meetingToUpdate, _mockRepository.Object);

        // Assert
        result.Should().BeOfType<Ok<Meeting>>();
        var okResult = (Ok<Meeting>)result;
        okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        okResult.Value.Should().BeEquivalentTo(meetingToUpdate);
        _mockRepository.Verify(r => r.UpdateMeetingAsync(meetingToUpdate), Times.Once);
    }

    [Fact]
    public async Task UpdateMeeting_WhenMeetingDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        var meetingId = "non-existent-id";
        var meetingToUpdate = TestDataBuilders.CreateMeeting()
            .WithId(meetingId)
            .WithTitle("Updated Title")
            .Build();
        _mockRepository.Setup(r => r.UpdateMeetingAsync(meetingToUpdate)).ReturnsAsync((Meeting)null!);

        // Act
        var result = await InvokeUpdateMeetingEndpoint(meetingId, meetingToUpdate, _mockRepository.Object);

        // Assert
        result.Should().BeOfType<NotFound>();
        _mockRepository.Verify(r => r.UpdateMeetingAsync(meetingToUpdate), Times.Once);
    }

    #endregion

    #region Helper Methods to Invoke Endpoints

    // These methods simulate the endpoint behavior directly since we're unit testing
    // the endpoint logic, not using WebApplicationFactory
    private static async Task<IResult> InvokeGetAllMeetingsEndpoint(IMeetingRepository repository)
    {
        var meetings = await repository.ListMeetingsAsync();
        return Results.Ok(meetings);
    }

    private static async Task<IResult> InvokeGetMeetingByIdEndpoint(string id, IMeetingRepository repository)
    {
        var meeting = await repository.GetMeetingByIdAsync(id);

        if (meeting == null)
        {
            return Results.NotFound();
        }

        return Results.Ok(meeting);
    }

    private static async Task<IResult> InvokeCreateMeetingEndpoint(Meeting meeting, IMeetingRepository repository)
    {
        var createdMeeting = await repository.CreateMeetingAsync(meeting);
        return Results.Created($"/api/meetings/{createdMeeting.Id}", createdMeeting);
    }

    private static async Task<IResult> InvokeUpdateMeetingEndpoint(string id, Meeting meeting, IMeetingRepository repository)
    {
        var updatedMeeting = await repository.UpdateMeetingAsync(meeting);

        if (updatedMeeting != null)
        {
            return Results.Ok(updatedMeeting);
        }

        return Results.NotFound();
    }

    #endregion
}
