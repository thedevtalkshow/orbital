using Microsoft.Azure.Cosmos;
using orbital.core;
using orbital.core.Data;
using Microsoft.Extensions.DependencyInjection;

namespace orbital.data
{
    public class CosmosMeetingRepository : IMeetingRepository
    {
        private readonly CosmosClient _client;
        private readonly Container _container;

        // public CosmosMeetingRepository(CosmosClient client)
        // {
        //     _client = client;
        //     _container = _client.GetContainer("orbital", "meetings");
        // }

        public CosmosMeetingRepository([FromKeyedServices("meetingContainer")] Container meetingContainer)
        {
            _container = meetingContainer;
        }

        public async Task<Meeting> CreateMeetingAsync(Meeting meeting)
        {
            if (string.IsNullOrEmpty(meeting.Id))
            {
                meeting.Id = Guid.NewGuid().ToString();
            }
            Meeting createdMeeting = await _container.CreateItemAsync(meeting, new PartitionKey("meeting"));
            return createdMeeting;
        }

        public async Task<Meeting> UpdateMeetingAsync(Meeting meeting)
        {
            // check if exists => replace
            var checkMeeting = await GetMeetingByIdAsync(meeting.Id);

            if (checkMeeting != null)
            {
                Meeting createdMeeting = await _container.ReplaceItemAsync(meeting, meeting.Id, new PartitionKey("meeting"));

                return createdMeeting;
            }

            return null;

        }

        public async Task<Meeting> GetMeetingByIdAsync(string id)
        {
            try
            {
                ItemResponse<Meeting> response = await _container.ReadItemAsync<Meeting>(id, new PartitionKey("meeting"));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                // Item not found
                return null;
            }
        }

        public async Task<IEnumerable<Meeting>> ListMeetingsAsync()
        {
            var resultIterator = _container.GetItemQueryIterator<Meeting>("SELECT * FROM c WHERE c.type='meeting'");
            var meetings = new List<Meeting>();

            while (resultIterator.HasMoreResults)
            {
                var resultSet = await resultIterator.ReadNextAsync();
                meetings.AddRange(resultSet);
            }
            return meetings;
        }
    }
}
