namespace orbital.web.Services;

public interface IMetadataClient
{
    Task<IEnumerable<T>> GetMetadataItemsAsync<T>(string metadataType) where T : IMetadataItem;
    Task<IEnumerable<EventStatusDefinition>> GetEventStatusesAsync();
    Task<IEnumerable<AttendanceModeDefinition>> GetAttendanceModesAsync();
    // Add more specific methods as needed
    
    // Force refresh local cache
    void ClearCache(string metadataType = null);
}