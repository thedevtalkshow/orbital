using Microsoft.Azure.Cosmos;
using orbital.core;
using orbital.core.Data;
using Container = Microsoft.Azure.Cosmos.Container;

namespace orbital.data.CosmosDb;

public class CosmosMeetingRepository : IMeetingRepository
{
    private readonly CosmosClient _client;
    private readonly Container _container;

    public CosmosMeetingRepository(CosmosClient client)
    {
        _client = client;
        _container = _client.GetContainer("orbital", "meetings");
    }

    /// <summary>
    /// Asynchronously retrieves all meetings from the Cosmos DB container.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a non-null collection of <see cref="Meeting"/> objects.
    /// If no meetings are found, an empty collection is returned.
    /// </returns>
    public async Task<IEnumerable<Meeting>> GetMeetingsAsync()
    {
        try
        {
            var query = _container.GetItemQueryIterator<Meeting>("SELECT * FROM c WHERE c.type='meeting'");
            var results = new List<Meeting>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response);
            }
            return results;
        }
        catch (CosmosException ex)
        {
            // Handle Cosmos DB exceptions (e.g., log the error)
            throw new Exception($"Error retrieving meetings: {ex.Message}", ex);
        }
    }
}
