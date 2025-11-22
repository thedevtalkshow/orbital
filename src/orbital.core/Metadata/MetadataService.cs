// generate a class for the metadtaservice that inherits from IMetadataService
using System;
using orbital.core.Data;

namespace orbital.core.Metadata;
public class MetadataService : IMetadataService
{
    private readonly IMetadataRepository _metadataRepository;

    public MetadataService(IMetadataRepository metadataRepository)
    {
        _metadataRepository = metadataRepository;
    }

    public async Task<IEnumerable<T>> GetMetadataItemsAsync<T>(string metadataType) where T : IMetadataItem
    {
        return await _metadataRepository.GetAllMetadataItemsAsync<T>(metadataType);
    }

    public T? GetMetadataItemByValue<T>(string metadataType, string value) where T : IMetadataItem
    {
        var task = _metadataRepository.GetMetadataItemByValueAsync<T>(metadataType, value);
        task.Wait();
        return task.Result;
    }

    public bool IsValidMetadataValue(string metadataType, string value)
    {
        var task = _metadataRepository.IsValidMetadataValueAsync(metadataType, value);
        task.Wait();
        return task.Result;
    }

    public async Task<T> UpdateMetadataAsync<T>(T metadataItem) where T : IMetadataItem
    {
        ArgumentNullException.ThrowIfNull(metadataItem);
        // check if the metadataitem.value contains only valid characters
        if (!System.Text.RegularExpressions.Regex.IsMatch(metadataItem.Value, "^[a-zA-Z0-9]*$"))
        {
            throw new ArgumentException("Metadata value contains invalid characters. Only alphanumeric characters are allowed.");
        }

        await _metadataRepository.UpdateMetadataItemAsync(metadataItem).ConfigureAwait(false);
        return metadataItem;
    }
}