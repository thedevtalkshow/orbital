using orbital.core;
using System.Net.Http.Json;

namespace orbital.web.Services
{
    public class MeetingsHttpClient : IMeetingsService
    {
        public readonly HttpClient _httpClient;

        public MeetingsHttpClient(HttpClient httpClient)
        {
            if (httpClient is null)
            {
                throw new ArgumentNullException(nameof(httpClient));
            }

            _httpClient = httpClient;
        }

        public async Task<List<Meeting>> GetMeetingsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/api/meetings");
                if (response.IsSuccessStatusCode)
                {
                    var meetings = await response.Content.ReadFromJsonAsync<List<Meeting>>();
                    return meetings;
                }
                else
                {
                    throw new Exception("Failed to load meetings");
                }

            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> AddMeetingAsync(Meeting meeting)
        {
            HttpResponseMessage response;
            try
            {
                response = await _httpClient.PostAsJsonAsync("/api/meetings", meeting);
            }
            catch (System.Exception ex)
            {                
                throw;
            } 

            return response.IsSuccessStatusCode;
        }
    }
}
