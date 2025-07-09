using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace orbital.core.Data
{
    public interface IMeetingRepository
    {
        Task<IEnumerable<Meeting>> ListMeetingsAsync();
        Task<Meeting> GetMeetingByIdAsync(string id);
        Task<Meeting> CreateMeetingAsync(Meeting meeting);

        /// <summary>
        /// Seeds the repository with initial meeting data if none exists.
        /// </summary>
        public async Task SeedMeetingsAsync()
        {
            var meetings = await ListMeetingsAsync();
            if (meetings.Any())
            {
                return; // Database already seeded
            }

            // Define meeting location
            const string meetingLocation = "Tech Hub Conference Center, 123 Innovation Drive";

            // Create 10 meetings for the past 10 months, happening on the third Wednesday at 6 PM
            var currentDate = DateTime.UtcNow;
            var currentMonth = new DateTime(currentDate.Year, currentDate.Month, 1);

            // .NET related meeting topics with descriptions and keywords
            var meetingTopics = new[]
            {
                new {
                    Title = "Introduction to Blazor WebAssembly",
                    Description = "Discover the power of C# in the browser with Blazor WebAssembly. This session covers the basics of component development, state management, and integration with APIs.",
                    Keywords = new List<string> { "Blazor", "WebAssembly", "frontend", ".NET", "C#", "web development" }
                },
                new {
                    Title = "ASP.NET Core Performance Optimization",
                    Description = "Learn advanced techniques for optimizing ASP.NET Core applications. Topics include caching strategies, asynchronous programming patterns, and database optimization.",
                    Keywords = new List<string> { "ASP.NET Core", "performance", "optimization", "caching", "async", "scalability" }
                },
                new {
                    Title = "Building Microservices with .NET",
                    Description = "Explore architectural patterns for building resilient microservices using .NET technologies. We'll cover service communication, containerization, and orchestration with Kubernetes.",
                    Keywords = new List<string> { "microservices", "architecture", "Docker", "Kubernetes", "containers", "distributed systems" }
                },
                new {
                    Title = "Entity Framework Core Deep Dive",
                    Description = "Take a deep dive into Entity Framework Core, exploring advanced querying, performance tuning, migrations, and data modeling strategies for complex domains.",
                    Keywords = new List<string> { "Entity Framework", "EF Core", "ORM", "database", "data access", "LINQ" }
                },
                new {
                    Title = "Secure Authentication in .NET Applications",
                    Description = "Implement secure authentication and authorization in .NET applications using Identity, JWT tokens, OAuth 2.0, and OpenID Connect.",
                    Keywords = new List<string> { "security", "authentication", "JWT", "OAuth", "Identity", "authorization" }
                },
                new {
                    Title = "Testing Strategies for .NET Applications",
                    Description = "Learn effective testing strategies for .NET applications, including unit testing, integration testing, and end-to-end testing using frameworks like xUnit, NUnit, and Playwright.",
                    Keywords = new List<string> { "testing", "TDD", "unit tests", "integration tests", "xUnit", "test automation" }
                },
                new {
                    Title = ".NET MAUI Cross-Platform Development",
                    Description = "Build cross-platform mobile and desktop applications with .NET MAUI. This session covers UI design, platform-specific features, and deployment strategies.",
                    Keywords = new List<string> { "MAUI", "mobile", "cross-platform", "iOS", "Android", "Windows" }
                },
                new {
                    Title = "Minimal APIs in ASP.NET Core",
                    Description = "Explore the new minimal API approach in ASP.NET Core for building lightweight, high-performance web services with reduced ceremony and boilerplate code.",
                    Keywords = new List<string> { "minimal APIs", "ASP.NET Core", "lightweight", "REST", "endpoints", "services" }
                },
                new {
                    Title = "Machine Learning with ML.NET",
                    Description = "Integrate machine learning capabilities into your .NET applications using ML.NET. Learn about classification, regression, and recommendation systems without leaving the .NET ecosystem.",
                    Keywords = new List<string> { "ML.NET", "machine learning", "AI", "data science", "predictive modeling", "analytics" }
                },
                new {
                    Title = "What's New in C# 12 and .NET 9",
                    Description = "Get up to speed with the latest features and improvements in C# 12 and .NET 9. We'll showcase new language features, performance improvements, and framework enhancements.",
                    Keywords = new List<string> { "C# 12", ".NET 9", "language features", "performance", "new features", "updates" }
                }
            };

            for (int i = 0; i < 10; i++)
            {
                // Start from 9 months ago and move forward
                var targetMonth = currentMonth.AddMonths(i - 9);

                // Find the third Wednesday of the month
                var firstDayOfMonth = new DateTime(targetMonth.Year, targetMonth.Month, 1);
                int firstWednesday = ((int)DayOfWeek.Wednesday - (int)firstDayOfMonth.DayOfWeek + 7) % 7 + 1;
                var thirdWednesday = new DateTime(targetMonth.Year, targetMonth.Month, firstWednesday).AddDays(14);

                // Set meeting time to 6 PM
                var startTime = new DateTime(thirdWednesday.Year, thirdWednesday.Month, thirdWednesday.Day, 18, 0, 0, DateTimeKind.Utc);
                var endTime = startTime.AddHours(2); // 2-hour meeting

                // Get the meeting topic (cycling through the topics if there are more than 10 months)
                var topicIndex = i % meetingTopics.Length;
                var topic = meetingTopics[topicIndex];

                var meeting = new Meeting
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = topic.Title,
                    Description = topic.Description,
                    StartTime = startTime,
                    EndTime = endTime,
                    Keywords = topic.Keywords,
                    Location = meetingLocation,
                    IsAccessibleForFree = true,
                    Audience = ".NET Developers",
                    EventAttendanceMode = "OfflineEventAttendanceMode",
                    MaximumAttendeeCapacity = 50,
                    Organizer = ".NET User Group"
                };

                await CreateMeetingAsync(meeting);
            }
        }
    }
}