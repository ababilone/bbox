using Newtonsoft.Json;

namespace BBox.Client.Models
{
    public class ActionReplyCallback 
    {
        [JsonProperty("uid")]
        public int Uid { get; set; }
	
        [JsonProperty("result")]
        public Result Result { get; set; }
	
        [JsonProperty("xpath")]
        public string xpath { get; set; }
	
        [JsonProperty("parameters")]
        public ParametersReply Parameters { get; set; }
    }
}