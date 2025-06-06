using Microsoft.Azure.Cosmos;
using orbital.core;
using orbital.core.Data;

namespace orbital.data
{
    public class CosmosMeetingRepository : IMeetingRepository
    {
        private readonly CosmosClient _client;
        private readonly Container _container;

        public CosmosMeetingRepository(CosmosClient client)
        {
            _client = client;
            _container = _client.GetContainer("orbital", "meetings");
        }

        public async Task<Meeting> CreateMeetingAsync(Meeting meeting)
        {
            Meeting createdMeeting = await _container.CreateItemAsync(meeting, new PartitionKey("meeting"));
            return createdMeeting;
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
