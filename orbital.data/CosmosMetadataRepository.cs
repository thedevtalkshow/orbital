using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using Microsoft.Azure.Cosmos;
using orbital.core.Data;
using orbital.core.Metadata;

namespace orbital.data;

public class CosmosMetadataRepository : IMetadataRepository
{
    private readonly Container _container;
    private readonly CosmosClient _client;

    public CosmosMetadataRepository(CosmosClient client, string databaseName, string containerName)
    {
        _client = client;
        _container = _client.GetContainer(databaseName, containerName);
    }

    public async Task<IEnumerable<T>> GetAllMetadataItemsAsync<T>(string metadataType) where T : IMetadataItem
    {
        var query = new QueryDefinition("SELECT * FROM c WHERE c.type = @type AND c.isActive = true ORDER BY c.sortOrder")
            .WithParameter("@type", metadataType);

        var iterator = _container.GetItemQueryIterator<T>(query);

        var results = new List<T>();
        while (iterator.HasMoreResults)
        {
            var response = await iterator.ReadNextAsync();
            results.AddRange(response);
        }

        return results;
    }

    public async Task<T> GetMetadataItemByValueAsync<T>(string metadataType, string value) where T : IMetadataItem
    {
        var query = new QueryDefinition("SELECT * FROM c WHERE c.type = @type AND c.value = @value")
            .WithParameter("@type", metadataType)
            .WithParameter("@value", value);

        var iterator = _container.GetItemQueryIterator<T>(query);

        if (iterator.HasMoreResults)
        {
            var response = await iterator.ReadNextAsync();
            return response.FirstOrDefault(Activator.CreateInstance<T>());
        }

        return Activator.CreateInstance<T>();
    }

    public async Task<bool> IsValidMetadataValueAsync(string metadataType, string value)
    {
        var query = new QueryDefinition("SELECT VALUE COUNT(1) FROM c WHERE c.type = @type AND c.value = @value AND c.isActive = true")
            .WithParameter("@type", metadataType)
            .WithParameter("@value", value);

        var iterator = _container.GetItemQueryIterator<int>(query);

        if (iterator.HasMoreResults)
        {
            var response = await iterator.ReadNextAsync();
            return response.FirstOrDefault() > 0;
        }

        return false;
    }

    public async Task<T> CreateMetadataItemAsync<T>(T item) where T : IMetadataItem
    {
        if (string.IsNullOrEmpty(item.Id))
        {
            item.Id = Guid.NewGuid().ToString();
        }

        var response = await _container.CreateItemAsync(item, new PartitionKey(item.Type));
        return response.Resource;
    }

    public async Task<bool> UpdateMetadataItemAsync<T>(T item) where T : IMetadataItem
    {
        try
        {
            await _container.ReplaceItemAsync(item, item.Id, new PartitionKey(item.Type));
            return true;
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return false;
        }
    }

    public async Task<bool> DeleteMetadataItemAsync<T>(string id, string metadataType) where T : IMetadataItem
    {
        try
        {
            await _container.DeleteItemAsync<T>(id, new PartitionKey(metadataType));

            return true;
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return false;
        }
    }
}
