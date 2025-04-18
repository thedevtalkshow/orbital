using Microsoft.Azure.Cosmos;

namespace orbital.api
{
    public class OrbitalData
    {
        public Database OrbitalDatabase { get; set; }
        public Container MeetingsContainer { get; set; }

        public OrbitalData(CosmosClient client)
        {
            OrbitalDatabase = client.GetDatabase("orbital");
            MeetingsContainer = OrbitalDatabase.GetContainer("meetings");
        }
    }
}
