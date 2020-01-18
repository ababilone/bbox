using Newtonsoft.Json;

namespace BBox.Client.Models
{
    public class CapabilityFlags
    {
        [JsonProperty("name")] 
        public bool Name { get; set; } = true;

        [JsonProperty("default-value")] 
        public bool DefaultValue { get; set; } = true;

        [JsonProperty("restriction")] 
        public bool Restriction { get; set; } = true;
        
        [JsonProperty("description")] 
        public bool Description { get; set; } = false;
        
        [JsonProperty("flags")] 
        public bool Flags { get; set; } = true;
        
        [JsonProperty("type")] 
        public bool Type { get; set; } = true;
    }
}