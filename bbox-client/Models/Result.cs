using Newtonsoft.Json;

namespace BBox.Client.Models
{
    public class Result
    {
        [JsonProperty("code")]
        public int Code { get; set; }
	
        [JsonProperty("description")]
        public string Description { get; set; }
    }
}