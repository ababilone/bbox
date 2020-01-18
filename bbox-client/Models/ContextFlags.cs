using Newtonsoft.Json;

namespace BBox.Client.Models
{
    public class ContextFlags
    {
        [JsonProperty("get-content-name")] 
        public bool GetContentName { get; set; } = true;
        
        [JsonProperty("local-time")] 
        public bool LocalTime { get; set; } = true;
        
        [JsonProperty("no-default")]
        public bool NoDefault { get; set; }
    }
}