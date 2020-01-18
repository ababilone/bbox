using Newtonsoft.Json;

namespace BBox.Client.Models
{
    public class Value
    {
        [JsonProperty("SSID")]
        public SSIDMetadata SSID { get; set; }
        
        [JsonProperty("AccessPoint")] 
        public AccessPoint AccessPoint { get; set; }
    }
}