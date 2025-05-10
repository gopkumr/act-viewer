using System.Text.Json.Nodes;

namespace Arinco.BicepHub.App.Core.Models
{
    public class Tag
    {
        public string Name { get; set; }
        public string Digest { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public string Platform { get; set; }
        public long? SizeInBytes { get; set; }
        public JsonNode? Manifest { get; set; }
        public string DocketCommand { get; set; }
        public string Source { get; set; }
        public string Documentation { get; set; }
    }
}
