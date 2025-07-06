using orbital.core.Models;

namespace orbital.core.Metadata;

public interface IMetadataService
{
    // Generic methods
    Task<IEnumerable<T>> GetMetadataItemsAsync<T>(string metadataType) where T : IMetadataItem;
    T? GetMetadataItemByValue<T>(string metadataType, string value) where T : IMetadataItem;
    bool IsValidMetadataValue(string metadataType, string value);
}
