using orbital.core.Metadata;

namespace orbital.core.Models;

public class EventStatusDefinition : MetadataDefinition
{
    // Additional properties specific to event status
    public bool RequiresPreviousStartDate { get; set; } = false;
    
    public EventStatusDefinition()
    {
        type = "eventStatus";
    }
}
