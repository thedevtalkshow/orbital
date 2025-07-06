using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using Microsoft.Azure.Cosmos;
using orbital.core.Data;
using orbital.core.Metadata;

namespace orbital.data;

public class CosmosMetadataRepository : IMetadataRepository
{
    private readonly Container _metadataContainer;
    public CosmosMetadataRepository(Container metadataContainer)
    {
        _metadataContainer = metadataContainer;
    }

    public async Task<IEnumerable<T>> GetAllMetadataItemsAsync<T>(string metadataType) where T : IMetadataItem
    {
        var query = new QueryDefinition("SELECT * FROM c WHERE c.type = @type AND c.IsActive = true ORDER BY c.SortOrder")
            .WithParameter("@type", metadataType);

        var iterator = _metadataContainer.GetItemQueryIterator<T>(query);

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

        var iterator = _metadataContainer.GetItemQueryIterator<T>(query);

        if (iterator.HasMoreResults)
        {
            var response = await iterator.ReadNextAsync();
            return response.FirstOrDefault(Activator.CreateInstance<T>());
        }

        return Activator.CreateInstance<T>();
    }

    public async Task<bool> IsValidMetadataValueAsync(string metadataType, string value)
    {
        var query = new QueryDefinition("SELECT VALUE COUNT(1) FROM c WHERE c.type = @type AND c.value = @value AND c.IsActive = true")
            .WithParameter("@type", metadataType)
            .WithParameter("@value", value);

        var iterator = _metadataContainer.GetItemQueryIterator<int>(query);

        if (iterator.HasMoreResults)
        {
            var response = await iterator.ReadNextAsync();
            return response.FirstOrDefault() > 0;
        }

        return false;
    }

    public async Task<T> CreateMetadataItemAsync<T>(T item) where T : IMetadataItem
    {
        if (string.IsNullOrEmpty(item.id))
        {
            item.id = Guid.NewGuid().ToString();
        }

        var response = await _metadataContainer.CreateItemAsync(item, new PartitionKey(item.type));
        return response.Resource;
    }

    public async Task<bool> UpdateMetadataItemAsync<T>(T item) where T : IMetadataItem
    {
        try
        {
            await _metadataContainer.ReplaceItemAsync<T>(item, item.id, new PartitionKey(item.type));
            return true;
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return false;
        }
    }

    /// <summary>
    /// This is 'logical' delete of a metadata item by setting the IsActive flag to false.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="id"></param>
    /// <param name="metadataType"></param>
    /// <returns></returns>
    public async Task<bool> DeleteMetadataItemAsync<T>(string id, string metadataType) where T : IMetadataItem
    {
        try
        {
            await _metadataContainer.DeleteItemAsync<T>(id, new PartitionKey(metadataType));

            return true;
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return false;
        }
    }
}
