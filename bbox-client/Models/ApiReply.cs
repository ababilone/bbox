using Newtonsoft.Json;

namespace BBox.Client.Models
{
    public class ApiReply
    {
        [JsonProperty("reply")]
        public Reply Reply { get; set; }
    }
}