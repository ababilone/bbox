using Newtonsoft.Json;

namespace BBox.Client.Models
{
    public class Action
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        
        [JsonProperty("method")]
        public string Method { get; set; }
        
        [JsonProperty("parameters")]
        public Parameters Parameters { get; set; }
        
        [JsonProperty("xpath")]
        public string XPath { get; set; }

        public override string ToString()
        {
            return string.IsNullOrWhiteSpace(XPath) ? Method : $"{Method} @ {XPath}";
        }
    }
}