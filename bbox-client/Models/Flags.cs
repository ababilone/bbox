using Newtonsoft.Json;

namespace BBox.Client.Models
{
    public class Flags
    {
        [JsonProperty("node")]
        public bool Node { get; set; }
        
        [JsonProperty("config")]
        public bool Config { get; set; }
        
        [JsonProperty("value")]
        public bool Value { get; set; }
        
        [JsonProperty("capability")]
        public bool Capability { get; set; }
        
        [JsonProperty("xml-child")]
        public bool XmlChild { get; set; }
    }
}