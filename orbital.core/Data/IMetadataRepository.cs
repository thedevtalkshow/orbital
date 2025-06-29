using orbital.core.Metadata;

namespace orbital.core.Data;

public interface IMetadataRepository
{
    Task<IEnumerable<T>> GetAllMetadataItemsAsync<T>(string metadataType) where T : IMetadataItem;
    Task<T> GetMetadataItemByValueAsync<T>(string metadataType, string value) where T : IMetadataItem;
    Task<bool> IsValidMetadataValueAsync(string metadataType, string value);

    // Add, Update, Delete methods if needed
    Task<T> CreateMetadataItemAsync<T>(T item) where T : IMetadataItem;
    Task<bool> UpdateMetadataItemAsync<T>(T item) where T : IMetadataItem;
    Task<bool> DeleteMEtadataItemAsync<T>(string id, string metadataType) where T : IMetadataItem;
}
