using Microsoft.Extensions.Logging;

namespace orbital.test
{
    public class ApiResourceIntegrationTest
    {
        private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(30);

        // Use NUnit's [TestCase] attribute for generic input pattern
        [TestCase("/", HttpStatusCode.NotFound, TestName = "GetApiResourceRootReturnsNotFoundStatusCode")]
        [TestCase("/api/meetings", HttpStatusCode.OK, TestName = "GetApiMeetingsListReturnsOkStatusCode")]
        [TestCase("/api/meetings/3", HttpStatusCode.OK, TestName = "GetApiMeetingsItemReturnsOkStatusCode")]
        public async Task GetApiEndpointReturnsExpectedStatusCode(string route, HttpStatusCode expectedStatusCode)
        {
            // Arrange
            var appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.orbital_AppHost>();
            appHost.Services.AddLogging(logging =>
            {
                logging.SetMinimumLevel(LogLevel.Debug);
                logging.AddFilter(appHost.Environment.ApplicationName, LogLevel.Debug);
                logging.AddFilter("Aspire.", LogLevel.Debug);
            });
            appHost.Services.ConfigureHttpClientDefaults(clientBuilder =>
            {
                clientBuilder.AddStandardResilienceHandler();
            });

            await using var app = await appHost.BuildAsync().WaitAsync(DefaultTimeout);
            await app.StartAsync().WaitAsync(DefaultTimeout);

            // Act
            var httpClient = app.CreateHttpClient("orbital-api");
            await app.ResourceNotifications.WaitForResourceHealthyAsync("orbital-api").WaitAsync(DefaultTimeout);
            var response = await httpClient.GetAsync(route);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(expectedStatusCode));
        }
    }
}
