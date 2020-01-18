using Newtonsoft.Json;

namespace BBox.Client.Models
{
    public class ParametersReply
    {
        [JsonProperty("id")] 
        public int Id { get; set; }

        [JsonProperty("nonce")]
        public long Nonce { get; set; }
        
        [JsonProperty("value")]
        public Value Value { get; set; }
        
        [JsonProperty("capability")]
        public Capability Capability { get; set; }
    }
}