using Newtonsoft.Json;

namespace BBox.Client.Models
{
    public class Parameters
    {
        public Parameters()
        {
        }
        
        public Parameters(string user)
        {
            User = user;
        }
        
        [JsonProperty("id")] 
        public int Id { get; set; }

        [JsonProperty("nonce")]
        public long Nonce { get; set; }
        
        [JsonProperty("user")]
        public string User { get; }

        [JsonProperty("persistent")] 
        public string Persistent { get; set; } = "true";
        
        [JsonProperty("session-options")]
        public SessionOptions SessionOptions { get; set; }

        [JsonProperty("value")]
        public object Value { get; set; }
    }
}