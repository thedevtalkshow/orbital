namespace orbital.web.Services;

public class MetadataService : IMetadataService
{
    private readonly IMetadataRepository _repository;
    private readonly Dictionary<string, object> _metadataCache = new();
    private readonly SemaphoreSlim _cacheLock = new SemaphoreSlim(1, 1);
    
    public MetadataService(IMetadataRepository repository)
    {
        _repository = repository;
        // Load common metadata on startup
        InitializeCommonMetadataAsync().GetAwaiter().GetResult();
    }
    
    private async Task InitializeCommonMetadataAsync()
    {
        // Load the most commonly used metadata types at startup
        await GetMetadataItemsAsync<EventStatusDefinition>("eventStatus");
        await GetMetadataItemsAsync<AttendanceModeDefinition>("attendanceMode");
        // Add more as needed
    }
    
    public async Task<IEnumerable<T>> GetMetadataItemsAsync<T>(string metadataType) where T : IMetadataItem
    {
        // Check cache first
        string cacheKey = $"{metadataType}_{typeof(T).Name}";
        
        if (_metadataCache.TryGetValue(cacheKey, out var cachedItems))
        {
            return (IEnumerable<T>)cachedItems;
        }
        
        // If not in cache, get from repository and cache it
        await _cacheLock.WaitAsync();
        try
        {
            // Double-check pattern
            if (_metadataCache.TryGetValue(cacheKey, out cachedItems))
            {
                return (IEnumerable<T>)cachedItems;
            }
            
            var items = await _repository.GetMetadataItemsAsync<T>(metadataType);
            var itemsList = items.ToList();
            _metadataCache[cacheKey] = itemsList;
            return itemsList;
        }
        finally
        {
            _cacheLock.Release();
        }
    }
    
    public T? GetMetadataItemByValue<T>(string metadataType, string value) where T : IMetadataItem
    {
        // Get all items (will use cache if available)
        var items = GetMetadataItemsAsync<T>(metadataType).GetAwaiter().GetResult();
        return items.FirstOrDefault(i => i.Value == value);
    }
    
    public bool IsValidMetadataValue(string metadataType, string value)
    {
        // For performance, we can directly check if any type exists with this value
        var items = GetMetadataItemsAsync<IMetadataItem>(metadataType).GetAwaiter().GetResult();
        return items.Any(i => i.Value == value && i.IsActive);
    }
    
    // Specific property accessors for convenience
    public IReadOnlyList<EventStatusDefinition> EventStatuses => 
        GetMetadataItemsAsync<EventStatusDefinition>("eventStatus").GetAwaiter().GetResult().ToList().AsReadOnly();
        
    public IReadOnlyList<AttendanceModeDefinition> AttendanceModes => 
        GetMetadataItemsAsync<AttendanceModeDefinition>("attendanceMode").GetAwaiter().GetResult().ToList().AsReadOnly();
    
    // Add more as needed
    
    public async Task RefreshCacheAsync(string metadataType = null)
    {
        await _cacheLock.WaitAsync();
        try
        {
            if (metadataType == null)
            {
                // Clear entire cache
                _metadataCache.Clear();
            }
            else
            {
                // Clear only entries for the specified type
                var keysToRemove = _metadataCache.Keys
                    .Where(k => k.StartsWith($"{metadataType}_"))
                    .ToList();
                    
                foreach (var key in keysToRemove)
                {
                    _metadataCache.Remove(key);
                }
            }
        }
        finally
        {
            _cacheLock.Release();
        }
    }
}
