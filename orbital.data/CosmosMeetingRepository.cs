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

        public Task<Meeting> GetMeetingByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Meeting>> GetMeetingsAsync()
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
