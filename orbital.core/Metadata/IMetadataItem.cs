namespace orbital.core.Metadata;

public interface IMetadataItem
{
    string Id {get; set;}
    string Type { get; set;}
    string Value {get; set; }
    string DisplayName { get; set; }
    string Description { get; set; }
    bool IsActive { get; set; }
    int SortOrder { get; set }
}
