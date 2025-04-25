using orbital.core;
using System.Net.Http.Json;

namespace orbital.web.Services
{
    internal sealed class MeetingsService(HttpClient client)
    {
        public async Task<List<Meeting>> GetMeetingsAsync()
        {
            List<Meeting> meetings = new();
    
            try
            {
                var response = await client.GetAsync("api/meetings");
                if (response.IsSuccessStatusCode)
                {
                    meetings = await response.Content.ReadFromJsonAsync<List<Meeting>>();
                    if (meetings.TryGetNonEnumeratedCount(out int meetingCount))
                    {
                        //logger.LogInformation($"Fetched {meetingCount} meetings successfully.");
                    }

                    return meetings;
                }
                else
                {
                    //logger.LogError($"Error fetching meetings: {response.StatusCode}");
                    return new List<Meeting>();
                }
            }
            catch (Exception ex)
            {
                //logger.LogError($"Exception fetching meetings: {ex.Message}");
                return new List<Meeting>();
            }
        }

    }
}
