using Newtonsoft.Json;

namespace BBox.Client.Models
{
    public class OutputParameter
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("description")]
        public string Description { get; set; }
        
        [JsonProperty("type")]
        public string Type { get; set; }
        
        [JsonProperty("flags")]
        public Flags Flags { get; set; }
    }
}