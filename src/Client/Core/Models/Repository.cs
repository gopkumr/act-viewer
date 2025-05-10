using System.Text.Json.Nodes;

namespace Arinco.BicepHub.App.Core.Models
{
    public class Repository
    {
        public string Name { get; set; }
        public string RegistryLoginServer { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public int ManifestCount { get; set; }
        public int TagCount { get; set; }

        public JsonNode? Manifest { get; set; }
    }
}
