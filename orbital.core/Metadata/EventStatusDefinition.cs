namespace orbital.core.Metadata;

public class EventStatusDefinition : MetadataDefinition
{
    // Additional properties specific to event status
    public bool RequiresPreviousStartDate { get; set; } = false;
    
    public EventStatusDefinition()
    {
        Type = "eventStatus";
    }
}
