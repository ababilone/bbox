using System.Collections.Generic;
using Newtonsoft.Json;

namespace BBox.Client.Models
{
    public class Capability
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("Description")]
        public string description { get; set; }
        
        [JsonProperty("type")]
        public string Type { get; set; }
        
        [JsonProperty("flags")]
        public Flags Flags { get; set; }
        
        [JsonProperty("interfaces")]
        public List<Interface> Interfaces { get; set; }
    }
}