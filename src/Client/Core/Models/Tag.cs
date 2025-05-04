using System.Text.Json.Nodes;

namespace ACRViewer.BlazorServer.Core.Models
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
    }
}
