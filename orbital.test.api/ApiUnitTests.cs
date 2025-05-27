using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace orbital.test.api
{
    public class ApiUnitTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public ApiUnitTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetMeetings_ReturnsOk()
        {
            // Arrange
            var client = _factory.CreateClient();
            // Act
            var response = await client.GetAsync("/api/meetings");
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
