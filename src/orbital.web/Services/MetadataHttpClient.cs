using System.Net.Http.Json;
using orbital.core.Metadata;
using orbital.core.Models;

namespace orbital.web.Services;

public class MetadataHttpClient : IMetadataService
{
    private readonly HttpClient _httpClient;
    private readonly Dictionary<string, object> _cache = new();
    public MetadataHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<IEnumerable<T>> GetMetadataItemsAsync<T>(string metadataType) where T : IMetadataItem
    {
        string cacheKey = $"{metadataType}_{typeof(T).Name}";
        
        if (_cache.TryGetValue(cacheKey, out var cachedItems))
        {
            return (IEnumerable<T>)cachedItems;
        }
        
        // string endpoint = metadataType.EndsWith("s") ? metadataType : $"{metadataType}s";  // TODO: don't like this but it works so far.
        var response = await _httpClient.GetAsync($"/api/metadata/{metadataType}");
        
        if (response.IsSuccessStatusCode)
        {
            var items = await response.Content.ReadFromJsonAsync<IEnumerable<T>>();
            var itemsList = items?.ToList() ?? new List<T>();
            _cache[cacheKey] = itemsList;
            return itemsList;
        }
        
        throw new Exception($"Failed to load {metadataType} metadata");
    }
    
    public void ClearCache(string? metadataType)
    {
        if (metadataType == null)
        {
            _cache.Clear();
        }
        else
        {
            var keysToRemove = _cache.Keys
                .Where(k => k.StartsWith($"{metadataType}_"))
                .ToList();
                
            foreach (var key in keysToRemove)
            {
                _cache.Remove(key);
            }
        }
    }

    public T? GetMetadataItemByValue<T>(string metadataType, string value) where T : IMetadataItem
    {
        throw new NotImplementedException();
    }

    public bool IsValidMetadataValue(string metadataType, string value)
    {
        throw new NotImplementedException();
    }
}