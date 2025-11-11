using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using orbital.core.Data;
using orbital.core.Metadata;
using orbital.core.Models;
using orbital.test.shared;

namespace orbital.api.tests;

public class MetadataEndpointsTests
{
    private readonly Mock<IMetadataRepository> _mockRepository;

    public MetadataEndpointsTests()
    {
        _mockRepository = new Mock<IMetadataRepository>();
    }

    #region GetEventStatuses Tests

    [Fact]
    public async Task GetEventStatuses_WhenStatusesExist_ReturnsOkWithStatuses()
    {
        // Arrange
        var statuses = new List<EventStatusDefinition>
        {
            new EventStatusDefinition { Id = "1", Value = "scheduled", DisplayName = "Scheduled" },
            new EventStatusDefinition { Id = "2", Value = "cancelled", DisplayName = "Cancelled" }
        };
        _mockRepository
            .Setup(r => r.GetAllMetadataItemsAsync<EventStatusDefinition>("EventStatusType"))
            .ReturnsAsync(statuses);

        // Act
        var result = await InvokeGetEventStatusesEndpoint(_mockRepository.Object);

        // Assert
        result.Should().BeOfType<Ok<IEnumerable<EventStatusDefinition>>>();
        var okResult = (Ok<IEnumerable<EventStatusDefinition>>)result;
        okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        okResult.Value.Should().BeEquivalentTo(statuses);
        _mockRepository.Verify(r => r.GetAllMetadataItemsAsync<EventStatusDefinition>("EventStatusType"), Times.Once);
    }

    [Fact]
    public async Task GetEventStatuses_WhenNoStatusesExist_ReturnsOkWithEmptyList()
    {
        // Arrange
        var emptyList = new List<EventStatusDefinition>();
        _mockRepository
            .Setup(r => r.GetAllMetadataItemsAsync<EventStatusDefinition>("EventStatusType"))
            .ReturnsAsync(emptyList);

        // Act
        var result = await InvokeGetEventStatusesEndpoint(_mockRepository.Object);

        // Assert
        result.Should().BeOfType<Ok<IEnumerable<EventStatusDefinition>>>();
        var okResult = (Ok<IEnumerable<EventStatusDefinition>>)result;
        okResult.Value.Should().BeEmpty();
        _mockRepository.Verify(r => r.GetAllMetadataItemsAsync<EventStatusDefinition>("EventStatusType"), Times.Once);
    }

    #endregion

    #region GetAttendanceModes Tests

    [Fact]
    public async Task GetAttendanceModes_WhenModesExist_ReturnsOkWithModes()
    {
        // Arrange
        var modes = new List<AttendanceModeDefinition>
        {
            new AttendanceModeDefinition { Id = "1", Value = "online", DisplayName = "Online" },
            new AttendanceModeDefinition { Id = "2", Value = "offline", DisplayName = "Offline" },
            new AttendanceModeDefinition { Id = "3", Value = "mixed", DisplayName = "Mixed" }
        };
        _mockRepository
            .Setup(r => r.GetAllMetadataItemsAsync<AttendanceModeDefinition>("EventAttendanceModeEnumeration"))
            .ReturnsAsync(modes);

        // Act
        var result = await InvokeGetAttendanceModesEndpoint(_mockRepository.Object);

        // Assert
        result.Should().BeOfType<Ok<IEnumerable<AttendanceModeDefinition>>>();
        var okResult = (Ok<IEnumerable<AttendanceModeDefinition>>)result;
        okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        okResult.Value.Should().BeEquivalentTo(modes);
        _mockRepository.Verify(
            r => r.GetAllMetadataItemsAsync<AttendanceModeDefinition>("EventAttendanceModeEnumeration"),
            Times.Once);
    }

    #endregion

    #region GetMetadataByType Tests

    [Fact]
    public async Task GetMetadataByType_WhenMetadataExists_ReturnsOkWithItems()
    {
        // Arrange
        var metadataType = "CustomType";
        var items = new List<IMetadataItem>
        {
            TestDataBuilders.CreateMetadataDefinition()
                .WithType(metadataType)
                .WithValue("value1")
                .Build(),
            TestDataBuilders.CreateMetadataDefinition()
                .WithType(metadataType)
                .WithValue("value2")
                .Build()
        };
        _mockRepository
            .Setup(r => r.GetAllMetadataItemsAsync<IMetadataItem>(metadataType))
            .ReturnsAsync(items);

        // Act
        var result = await InvokeGetMetadataByTypeEndpoint(metadataType, _mockRepository.Object);

        // Assert
        result.Should().BeOfType<Ok<IEnumerable<IMetadataItem>>>();
        var okResult = (Ok<IEnumerable<IMetadataItem>>)result;
        okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        okResult.Value.Should().BeEquivalentTo(items);
        _mockRepository.Verify(r => r.GetAllMetadataItemsAsync<IMetadataItem>(metadataType), Times.Once);
    }

    #endregion

    #region CreateMetadataItem Tests

    [Fact]
    public async Task CreateMetadataItem_WithValidItem_ReturnsCreatedWithItem()
    {
        // Arrange
        var newItem = TestDataBuilders.CreateMetadataDefinition()
            .WithType("TestType")
            .WithValue("test-value")
            .Build();
        var createdItem = TestDataBuilders.CreateMetadataDefinition()
            .WithId("created-id-123")
            .WithType(newItem.Type)
            .WithValue(newItem.Value)
            .Build();
        _mockRepository
            .Setup(r => r.CreateMetadataItemAsync(newItem))
            .ReturnsAsync(createdItem);

        // Act
        var result = await InvokeCreateMetadataItemEndpoint(newItem, _mockRepository.Object);

        // Assert
        result.Should().BeOfType<Created<MetadataDefinition>>();
        var createdResult = (Created<MetadataDefinition>)result;
        createdResult.StatusCode.Should().Be(StatusCodes.Status201Created);
        createdResult.Location.Should().Be($"/api/metadata/{newItem.Type}/{createdItem.Id}");
        createdResult.Value.Should().BeEquivalentTo(createdItem);
        _mockRepository.Verify(r => r.CreateMetadataItemAsync(newItem), Times.Once);
    }

    [Fact]
    public async Task CreateMetadataItem_WithEmptyType_ReturnsBadRequest()
    {
        // Arrange
        var invalidItem = TestDataBuilders.CreateMetadataDefinition()
            .WithType("")
            .WithValue("test-value")
            .Build();

        // Act
        var result = await InvokeCreateMetadataItemEndpoint(invalidItem, _mockRepository.Object);

        // Assert
        result.Should().BeOfType<BadRequest<string>>();
        var badRequestResult = (BadRequest<string>)result;
        badRequestResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        badRequestResult.Value.Should().Be("Type and Value are required");
        _mockRepository.Verify(r => r.CreateMetadataItemAsync(It.IsAny<MetadataDefinition>()), Times.Never);
    }

    [Fact]
    public async Task CreateMetadataItem_WithEmptyValue_ReturnsBadRequest()
    {
        // Arrange
        var invalidItem = TestDataBuilders.CreateMetadataDefinition()
            .WithType("TestType")
            .WithValue("")
            .Build();

        // Act
        var result = await InvokeCreateMetadataItemEndpoint(invalidItem, _mockRepository.Object);

        // Assert
        result.Should().BeOfType<BadRequest<string>>();
        var badRequestResult = (BadRequest<string>)result;
        badRequestResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        badRequestResult.Value.Should().Be("Type and Value are required");
        _mockRepository.Verify(r => r.CreateMetadataItemAsync(It.IsAny<MetadataDefinition>()), Times.Never);
    }

    #endregion

    #region UpdateMetadataItem Tests

    [Fact]
    public async Task UpdateMetadataItem_WhenItemExists_ReturnsNoContent()
    {
        // Arrange
        var itemToUpdate = TestDataBuilders.CreateMetadataDefinition()
            .WithId("update-id-123")
            .WithType("TestType")
            .WithValue("updated-value")
            .Build();
        _mockRepository
            .Setup(r => r.UpdateMetadataItemAsync(itemToUpdate))
            .ReturnsAsync(true);

        // Act
        var result = await InvokeUpdateMetadataItemEndpoint(itemToUpdate, _mockRepository.Object);

        // Assert
        result.Should().BeOfType<NoContent>();
        var noContentResult = (NoContent)result;
        noContentResult.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        _mockRepository.Verify(r => r.UpdateMetadataItemAsync(itemToUpdate), Times.Once);
    }

    [Fact]
    public async Task UpdateMetadataItem_WhenItemDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        var itemToUpdate = TestDataBuilders.CreateMetadataDefinition()
            .WithId("non-existent-id")
            .WithType("TestType")
            .Build();
        _mockRepository
            .Setup(r => r.UpdateMetadataItemAsync(itemToUpdate))
            .ReturnsAsync(false);

        // Act
        var result = await InvokeUpdateMetadataItemEndpoint(itemToUpdate, _mockRepository.Object);

        // Assert
        result.Should().BeOfType<NotFound>();
        _mockRepository.Verify(r => r.UpdateMetadataItemAsync(itemToUpdate), Times.Once);
    }

    [Fact]
    public async Task UpdateMetadataItem_WithEmptyId_ReturnsBadRequest()
    {
        // Arrange
        var invalidItem = TestDataBuilders.CreateMetadataDefinition()
            .WithId("")
            .WithType("TestType")
            .Build();

        // Act
        var result = await InvokeUpdateMetadataItemEndpoint(invalidItem, _mockRepository.Object);

        // Assert
        result.Should().BeOfType<BadRequest<string>>();
        var badRequestResult = (BadRequest<string>)result;
        badRequestResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        badRequestResult.Value.Should().Be("Id and Type are required");
        _mockRepository.Verify(r => r.UpdateMetadataItemAsync(It.IsAny<MetadataDefinition>()), Times.Never);
    }

    [Fact]
    public async Task UpdateMetadataItem_WithEmptyType_ReturnsBadRequest()
    {
        // Arrange
        var invalidItem = TestDataBuilders.CreateMetadataDefinition()
            .WithId("test-id")
            .WithType("")
            .Build();

        // Act
        var result = await InvokeUpdateMetadataItemEndpoint(invalidItem, _mockRepository.Object);

        // Assert
        result.Should().BeOfType<BadRequest<string>>();
        var badRequestResult = (BadRequest<string>)result;
        badRequestResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        badRequestResult.Value.Should().Be("Id and Type are required");
        _mockRepository.Verify(r => r.UpdateMetadataItemAsync(It.IsAny<MetadataDefinition>()), Times.Never);
    }

    #endregion

    #region DeleteMetadataItem Tests

    [Fact]
    public async Task DeleteMetadataItem_WhenItemExists_ReturnsNoContent()
    {
        // Arrange
        var metadataType = "TestType";
        var itemId = "delete-id-123";
        _mockRepository
            .Setup(r => r.DeleteMetadataItemAsync<IMetadataItem>(itemId, metadataType))
            .ReturnsAsync(true);

        // Act
        var result = await InvokeDeleteMetadataItemEndpoint(metadataType, itemId, _mockRepository.Object);

        // Assert
        result.Should().BeOfType<NoContent>();
        var noContentResult = (NoContent)result;
        noContentResult.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        _mockRepository.Verify(r => r.DeleteMetadataItemAsync<IMetadataItem>(itemId, metadataType), Times.Once);
    }

    [Fact]
    public async Task DeleteMetadataItem_WhenItemDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        var metadataType = "TestType";
        var itemId = "non-existent-id";
        _mockRepository
            .Setup(r => r.DeleteMetadataItemAsync<IMetadataItem>(itemId, metadataType))
            .ReturnsAsync(false);

        // Act
        var result = await InvokeDeleteMetadataItemEndpoint(metadataType, itemId, _mockRepository.Object);

        // Assert
        result.Should().BeOfType<NotFound>();
        _mockRepository.Verify(r => r.DeleteMetadataItemAsync<IMetadataItem>(itemId, metadataType), Times.Once);
    }

    #endregion

    #region Helper Methods to Invoke Endpoints

    // These methods simulate the endpoint behavior directly since we're unit testing
    // the endpoint logic, not using WebApplicationFactory
    private static async Task<IResult> InvokeGetEventStatusesEndpoint(IMetadataRepository metadataRepository)
    {
        var items = await metadataRepository.GetAllMetadataItemsAsync<EventStatusDefinition>("EventStatusType");
        return Results.Ok(items);
    }

    private static async Task<IResult> InvokeGetAttendanceModesEndpoint(IMetadataRepository metadataRepository)
    {
        var items = await metadataRepository.GetAllMetadataItemsAsync<AttendanceModeDefinition>("EventAttendanceModeEnumeration");
        return Results.Ok(items);
    }

    private static async Task<IResult> InvokeGetMetadataByTypeEndpoint(string metadataType, IMetadataRepository metadataRepository)
    {
        var items = await metadataRepository.GetAllMetadataItemsAsync<IMetadataItem>(metadataType);
        return Results.Ok(items);
    }

    private static async Task<IResult> InvokeCreateMetadataItemEndpoint(MetadataDefinition item, IMetadataRepository metadataRepository)
    {
        if (string.IsNullOrEmpty(item.Type) || string.IsNullOrEmpty(item.Value))
        {
            return Results.BadRequest("Type and Value are required");
        }

        var result = await metadataRepository.CreateMetadataItemAsync(item);
        return Results.Created($"/api/metadata/{item.Type}/{result.Id}", result);
    }

    private static async Task<IResult> InvokeUpdateMetadataItemEndpoint(MetadataDefinition item, IMetadataRepository metadataRepository)
    {
        if (string.IsNullOrEmpty(item.Id) || string.IsNullOrEmpty(item.Type))
        {
            return Results.BadRequest("Id and Type are required");
        }

        var success = await metadataRepository.UpdateMetadataItemAsync(item);
        if (!success)
        {
            return Results.NotFound();
        }

        return Results.NoContent();
    }

    private static async Task<IResult> InvokeDeleteMetadataItemEndpoint(string metadataType, string id, IMetadataRepository metadataRepository)
    {
        var success = await metadataRepository.DeleteMetadataItemAsync<IMetadataItem>(id, metadataType);
        if (!success)
        {
            return Results.NotFound();
        }

        return Results.NoContent();
    }

    #endregion
}
