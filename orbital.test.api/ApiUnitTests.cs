using Microsoft.AspNetCore.Mvc.Testing;
using orbital.core;
using orbital.core.Data;
using System.Net;

namespace orbital.test.api
{
    public class ApiUnitTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public ApiUnitTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                // Configure the test environment, e.g., use an in-memory database or mock services
                builder.ConfigureServices(services =>
                {
                    // Remove the real Cosmos DB repository to avoid external dependencies during tests
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(IMeetingRepository));

                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                    // Replace the real repository with an in-memory one for testing
                    services.AddScoped<IMeetingRepository, InMemoryMeetingRepository>();
                });
            });
        }

        [Theory]
        [InlineData("/api/meetings", HttpStatusCode.OK)]
        [InlineData("/api/meetings/1", HttpStatusCode.OK)]
        public async Task GetMeetings_ReturnsOk(string targetUri, System.Net.HttpStatusCode code)
        {
            // Arrange
            var client = _factory.CreateClient();
            // Act
            var response = await client.GetAsync(targetUri);
            // Assert
            Assert.Equal(code, response.StatusCode);
        }

        [Fact]
        public async Task GetMeetings_ReturnsCorrectAmount()
        {
            // Arrange
            var client = _factory.CreateClient();
            // Act
            var response = await client.GetAsync("/api/meetings");
            response.EnsureSuccessStatusCode();

            var meetings = await response.Content.ReadFromJsonAsync<IEnumerable<Meeting>>();
            
            // Assert
            Assert.Equal(2, meetings.Count());
        }

        [Fact]
        public async Task CreateMeeting_ReturnsCreatedStatusCode()
        {
            // Arrange
            var client = _factory.CreateClient();
            var newMeeting = new Meeting
            {
                Id = "3",
                Title = "Test Meeting",
                Description = "This is a test meeting.",
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow.AddHours(1)
            };
            // Act
            var response = await client.PostAsJsonAsync("/api/meetings", newMeeting);
            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var createdMeeting = await response.Content.ReadFromJsonAsync<Meeting>();

            // ? -- should these be part of separate tests (perhaps data driven tests)?
            Assert.NotNull(createdMeeting);
            Assert.Equal(newMeeting.Id, createdMeeting.Id);
        }
    }
}
