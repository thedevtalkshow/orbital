using orbital.core.Models;

namespace orbital.core.Metadata;

public interface IMetadataService
{
    // Generic methods
    Task<IEnumerable<T>> GetMetadataItemsAsync<T>(string metadataType) where T : IMetadataItem;
    T? GetMetadataItemByValue<T>(string metadataType, string value) where T : IMetadataItem;
    bool IsValidMetadataValue(string metadataType, string value);
    Task<IEnumerable<EventStatusDefinition>> GetEventStatusesAsync();

    // Convenience methods for specific types
    IReadOnlyList<EventStatusDefinition> EventStatuses { get; }
    // IReadOnlyList<AttendanceModeDefinition> AttendanceModes { get; }
    // Add more as needed

    // Force refresh cache for testing or admin purposes
    // Task RefreshCacheAsync(string metadataType = null);
}
