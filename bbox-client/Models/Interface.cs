using System.Collections.Generic;
using Newtonsoft.Json;

namespace BBox.Client.Models
{
    public class Interface
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("description")]
        public string Description { get; set; }
        
        [JsonProperty("major-version")]
        public string MajorVersion { get; set; }
        
        [JsonProperty("minor-version")]
        public string MinorVersion { get; set; }
        
        [JsonProperty("commands")]
        public List<Command> Commands { get; set; }
    }
}