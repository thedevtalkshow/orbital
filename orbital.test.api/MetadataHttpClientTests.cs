using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Moq;
using Moq.Protected;
using orbital.core.Metadata;
using orbital.core.Models;
using orbital.web.Services;

namespace orbital.test.api;

public class MetadataHttpClientTests : IDisposable
{
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private readonly HttpClient _httpClient;
    private readonly MetadataHttpClient _metadataHttpClient;

    public MetadataHttpClientTests()
    {
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        _httpClient = new HttpClient(_mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri("https://localhost/")
        };
        _metadataHttpClient = new MetadataHttpClient(_httpClient);
    }

    #region Constructor Tests

    [Fact]
    public void Constructor_WithValidHttpClient_ShouldCreateInstance()
    {
        // Arrange
        var httpClient = new HttpClient();

        // Act
        var client = new MetadataHttpClient(httpClient);

        // Assert
        Assert.NotNull(client);
    }

    #endregion

    #region GetMetadataItemsAsync Tests

    [Fact]
    public async Task GetMetadataItemsAsync_WithCacheHit_ShouldReturnCachedData()
    {
        // Arrange
        var testData = new List<TestMetadataItem>
        {
            new() { Id = "1", Type = "TestType", Value = "Test1", DisplayName = "Test Item 1", IsActive = true },
            new() { Id = "2", Type = "TestType", Value = "Test2", DisplayName = "Test Item 2", IsActive = true }
        };

        // First call to populate cache
        SetupHttpResponse(HttpStatusCode.OK, testData);
        await _metadataHttpClient.GetMetadataItemsAsync<TestMetadataItem>("TestType");

        // Act - Second call should hit cache
        var result = await _metadataHttpClient.GetMetadataItemsAsync<TestMetadataItem>("TestType");

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Equal(testData[0].Id, result.First().Id);
        Assert.Equal(testData[1].Id, result.Last().Id);

        // Verify HTTP was called only once (for initial cache population)
        _mockHttpMessageHandler.Protected()
            .Verify("SendAsync", Times.Once(), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
    }

    [Fact]
    public async Task GetMetadataItemsAsync_WithCacheMiss_ShouldCallApiAndCacheResult()
    {
        // Arrange
        var testData = new List<TestMetadataItem>
        {
            new() { Id = "1", Type = "TestType", Value = "Test1", DisplayName = "Test Item 1", IsActive = true }
        };
        SetupHttpResponse(HttpStatusCode.OK, testData);

        // Act
        var result = await _metadataHttpClient.GetMetadataItemsAsync<TestMetadataItem>("TestType");

        // Assert
        Assert.Single(result);
        Assert.Equal(testData[0].Id, result.First().Id);

        // Verify HTTP was called
        _mockHttpMessageHandler.Protected()
            .Verify("SendAsync", Times.Once(), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
    }

    [Fact]
    public async Task GetMetadataItemsAsync_WithHttpFailure_ShouldThrowException()
    {
        // Arrange
        SetupHttpResponse<TestMetadataItem>(HttpStatusCode.InternalServerError, null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(
            () => _metadataHttpClient.GetMetadataItemsAsync<TestMetadataItem>("TestType"));

        Assert.Equal("Failed to load TestType metadata", exception.Message);
    }

    [Fact]
    public async Task GetMetadataItemsAsync_WithNullResponse_ShouldReturnEmptyList()
    {
        // Arrange
        SetupHttpResponse(HttpStatusCode.OK, (List<TestMetadataItem>?)null);

        // Act
        var result = await _metadataHttpClient.GetMetadataItemsAsync<TestMetadataItem>("TestType");

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetMetadataItemsAsync_WithEmptyResponse_ShouldReturnEmptyList()
    {
        // Arrange
        var emptyData = new List<TestMetadataItem>();
        SetupHttpResponse(HttpStatusCode.OK, emptyData);

        // Act
        var result = await _metadataHttpClient.GetMetadataItemsAsync<TestMetadataItem>("TestType");

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetMetadataItemsAsync_WithCorrectEndpoint_ShouldCallCorrectUrl()
    {
        // Arrange
        var testData = new List<TestMetadataItem>
        {
            new() { Id = "1", Type = "EventStatusType", Value = "Test1", DisplayName = "Test Item 1", IsActive = true }
        };
        
        var expectedUri = "https://localhost/api/metadata/EventStatusType";
        HttpRequestMessage? capturedRequest = null;

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync((HttpRequestMessage request, CancellationToken token) =>
            {
                capturedRequest = request;
                var json = JsonSerializer.Serialize(testData);
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(json, Encoding.UTF8, "application/json")
                };
            });

        // Act
        await _metadataHttpClient.GetMetadataItemsAsync<TestMetadataItem>("EventStatusType");

        // Assert
        Assert.NotNull(capturedRequest);
        Assert.Equal(expectedUri, capturedRequest.RequestUri?.ToString());
        Assert.Equal(HttpMethod.Get, capturedRequest.Method);
    }

    [Fact]
    public async Task GetMetadataItemsAsync_WithDifferentTypes_ShouldCacheSeparately()
    {
        // Arrange
        var eventStatusData = new List<EventStatusDefinition>
        {
            new() { Id = "1", Value = "Draft", DisplayName = "Draft Status" }
        };
        var attendanceModeData = new List<AttendanceModeDefinition>
        {
            new() { Id = "2", Value = "Online", DisplayName = "Online Mode" }
        };

        // Setup different responses for different endpoints
        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req => req.RequestUri!.ToString().Contains("EventStatusType")),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(() =>
            {
                var json = JsonSerializer.Serialize(eventStatusData);
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(json, Encoding.UTF8, "application/json")
                };
            });

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req => req.RequestUri!.ToString().Contains("EventAttendanceModeEnumeration")),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(() =>
            {
                var json = JsonSerializer.Serialize(attendanceModeData);
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(json, Encoding.UTF8, "application/json")
                };
            });

        // Act
        var eventStatusResult = await _metadataHttpClient.GetMetadataItemsAsync<EventStatusDefinition>("EventStatusType");
        var attendanceModeResult = await _metadataHttpClient.GetMetadataItemsAsync<AttendanceModeDefinition>("EventAttendanceModeEnumeration");

        // Assert
        Assert.Single(eventStatusResult);
        Assert.Equal("Draft", eventStatusResult.First().Value);

        Assert.Single(attendanceModeResult);
        Assert.Equal("Online", attendanceModeResult.First().Value);

        // Verify both endpoints were called
        _mockHttpMessageHandler.Protected()
            .Verify("SendAsync", Times.Exactly(2), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
    }

    #endregion

    #region ClearCache Tests

    [Fact]
    public async Task ClearCache_WithNullParameter_ShouldClearAllCache()
    {
        // Arrange - Populate cache with data
        var testData = new List<TestMetadataItem>
        {
            new() { Id = "1", Type = "TestType", Value = "Test1", DisplayName = "Test Item 1", IsActive = true }
        };
        SetupHttpResponse(HttpStatusCode.OK, testData);
        await _metadataHttpClient.GetMetadataItemsAsync<TestMetadataItem>("TestType");

        // Act
        _metadataHttpClient.ClearCache(null);

        // Reset mock to verify new HTTP call
        _mockHttpMessageHandler.Reset();
        SetupHttpResponse(HttpStatusCode.OK, testData);

        // Make another call - should hit API again
        await _metadataHttpClient.GetMetadataItemsAsync<TestMetadataItem>("TestType");

        // Assert - Verify HTTP was called again (cache was cleared)
        _mockHttpMessageHandler.Protected()
            .Verify("SendAsync", Times.Once(), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
    }

    [Fact]
    public async Task ClearCache_WithSpecificMetadataType_ShouldClearOnlyThatType()
    {
        // Arrange - Populate cache with two different types
        var testData1 = new List<TestMetadataItem>
        {
            new() { Id = "1", Type = "Type1", Value = "Test1", DisplayName = "Test Item 1", IsActive = true }
        };
        var testData2 = new List<TestMetadataItem>
        {
            new() { Id = "2", Type = "Type2", Value = "Test2", DisplayName = "Test Item 2", IsActive = true }
        };

        SetupHttpResponse(HttpStatusCode.OK, testData1);
        await _metadataHttpClient.GetMetadataItemsAsync<TestMetadataItem>("Type1");

        SetupHttpResponse(HttpStatusCode.OK, testData2);
        await _metadataHttpClient.GetMetadataItemsAsync<TestMetadataItem>("Type2");

        // Act - Clear only Type1 cache
        _metadataHttpClient.ClearCache("Type1");

        // Reset mock to track new calls
        _mockHttpMessageHandler.Reset();
        SetupHttpResponse(HttpStatusCode.OK, testData1);

        // Make calls to both types
        await _metadataHttpClient.GetMetadataItemsAsync<TestMetadataItem>("Type1"); // Should hit API
        await _metadataHttpClient.GetMetadataItemsAsync<TestMetadataItem>("Type2"); // Should hit cache

        // Assert - Only Type1 should have made a new HTTP call
        _mockHttpMessageHandler.Protected()
            .Verify("SendAsync", Times.Once(), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
    }

    [Fact]
    public void ClearCache_WithNonExistentType_ShouldNotThrow()
    {
        // Act & Assert - Should not throw exception
        var exception = Record.Exception(() => _metadataHttpClient.ClearCache("NonExistentType"));
        Assert.Null(exception);
    }

    #endregion

    #region NotImplemented Methods Tests

    [Fact]
    public void GetMetadataItemByValue_ShouldThrowNotImplementedException()
    {
        // Act & Assert
        Assert.Throws<NotImplementedException>(() =>
            _metadataHttpClient.GetMetadataItemByValue<TestMetadataItem>("TestType", "TestValue"));
    }

    [Fact]
    public void IsValidMetadataValue_ShouldThrowNotImplementedException()
    {
        // Act & Assert
        Assert.Throws<NotImplementedException>(() =>
            _metadataHttpClient.IsValidMetadataValue("TestType", "TestValue"));
    }

    #endregion

    #region Helper Methods

    private void SetupHttpResponse<T>(HttpStatusCode statusCode, T? data)
    {
        var response = new HttpResponseMessage(statusCode);
        
        if (statusCode == HttpStatusCode.OK)
        {
            var json = data == null ? "null" : JsonSerializer.Serialize(data);
            response.Content = new StringContent(json, Encoding.UTF8, "application/json");
        }

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);
    }

    #endregion

    #region Test Data Classes

    public class TestMetadataItem : IMetadataItem
    {
        public string Id { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public int SortOrder { get; set; } = 0;
    }

    #endregion

    #region Dispose

    public void Dispose()
    {
        _httpClient?.Dispose();
    }

    #endregion
}
