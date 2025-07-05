namespace orbital.core.Metadata;

public interface IMetadataItem
{
    string id { get; set; }
    string type { get; set; }
    string Value { get; set; }
    string DisplayName { get; set; }
    string Description { get; set; }
    bool IsActive { get; set; }
    int SortOrder { get; set; }
}
