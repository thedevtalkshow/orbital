using orbital.core.Data;
using orbital.core.Metadata;

namespace orbital.integration.tests.Infrastructure;

/// <summary>
/// In-memory implementation of IMetadataRepository for integration testing.
/// Provides fast, isolated test execution without external database dependencies.
/// </summary>
public class InMemoryMetadataRepository : IMetadataRepository
{
    private readonly List<IMetadataItem> _metadataItems = new();

    public Task<IEnumerable<T>> GetAllMetadataItemsAsync<T>(string metadataType) where T : IMetadataItem
    {
        var items = _metadataItems
            .OfType<T>()
            .Where(item => item.Type == metadataType);
        return Task.FromResult(items);
    }

    public Task<T> GetMetadataItemByValueAsync<T>(string metadataType, string value) where T : IMetadataItem
    {
        var item = _metadataItems
            .OfType<T>()
            .FirstOrDefault(i => i.Type == metadataType && i.Value == value);
        return Task.FromResult(item)!;
    }

    public Task<bool> IsValidMetadataValueAsync(string metadataType, string value)
    {
        var isValid = _metadataItems.Any(i => i.Type == metadataType && i.Value == value);
        return Task.FromResult(isValid);
    }

    public Task<T> CreateMetadataItemAsync<T>(T item) where T : IMetadataItem
    {
        _metadataItems.Add(item);
        return Task.FromResult(item);
    }

    public Task<bool> UpdateMetadataItemAsync<T>(T item) where T : IMetadataItem
    {
        var index = _metadataItems.FindIndex(i => i.Id == item.Id && i.Type == item.Type);
        if (index != -1)
        {
            _metadataItems[index] = item;
            return Task.FromResult(true);
        }
        return Task.FromResult(false);
    }

    public Task<bool> DeleteMetadataItemAsync<T>(string id, string metadataType) where T : IMetadataItem
    {
        var index = _metadataItems.FindIndex(i => i.Id == id && i.Type == metadataType);
        if (index != -1)
        {
            _metadataItems.RemoveAt(index);
            return Task.FromResult(true);
        }
        return Task.FromResult(false);
    }
}
