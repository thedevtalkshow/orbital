using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;
using orbital.core.Data;
using orbital.core.Metadata;
using orbital.core.Models;

namespace orbital.data;

public class CosmosMetadataRepository : IMetadataRepository
{
    private readonly Container _metadataContainer;
    public CosmosMetadataRepository([FromKeyedServices("metadataContainer")] Container metadataContainer)
    {
        _metadataContainer = metadataContainer;

        //check if data exists. If not seed the container.
        var eventStatusResults = GetAllMetadataItemsAsync<EventStatusDefinition>("eventStatus");
        // if (eventStatusResults.Result.Count() == 0)
        // {
        //     foreach (var status in MetadataDataLoader.EventStatuses)
        //     {
        //         CreateMetadataItemAsync<EventStatusDefinition>(status);
        //     }
        // }

        var attendanceNodeResults = GetAllMetadataItemsAsync<AttendanceModeDefinition>("attendanceMode");
        // if (attendanceNodeResults.Result.Count() == 0)
        // {
        //     foreach (var mode in MetadataDataLoader.EventAttendanceMode)
        //     {
        //         CreateMetadataItemAsync<AttendanceModeDefinition>(mode);
        //     }
        // }
    }

    public async Task<IEnumerable<T>> GetAllMetadataItemsAsync<T>(string metadataType) where T : IMetadataItem
    {
        var query = new QueryDefinition("SELECT * FROM c WHERE c.type = @Type AND c.isActive = true ORDER BY c.sortOrder")
            .WithParameter("@Type", metadataType);

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
        var query = new QueryDefinition("SELECT * FROM c WHERE c.type = @Type AND c.value = @value")
            .WithParameter("@Type", metadataType)
            .WithParameter("@Value", value);

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
        var query = new QueryDefinition("SELECT VALUE COUNT(1) FROM c WHERE c.type = @Type AND c.value = @value AND c.IsActive = true")
            .WithParameter("@Type", metadataType)
            .WithParameter("@Value", value);

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
        if (string.IsNullOrEmpty(item.Id))
        {
            item.Id = Guid.NewGuid().ToString();
        }

        var response = await _metadataContainer.CreateItemAsync(item, new PartitionKey(item.Type));
        return response.Resource;
    }

    public async Task<bool> UpdateMetadataItemAsync<T>(T item) where T : IMetadataItem
    {
        try
        {
            await _metadataContainer.ReplaceItemAsync<T>(item, item.Id, new PartitionKey(item.Type));
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
