using System.Text.Json.Serialization;
using orbital.core.Metadata;

namespace orbital.core.Metadata;

public class MetadataDefinition : IMetadataItem
{
    public string Id { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public int SortOrder { get; set; } = 0;
} 
