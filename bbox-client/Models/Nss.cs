using Newtonsoft.Json;

namespace BBox.Client.Models
{
    public class Nss
    {
        [JsonProperty("name")]
        public string Name { get; set; } = "gtw";
        
        [JsonProperty("uri")]
        public string Uri { get; set; } = "http://sagemcom.com/gateway-data";
    }
}