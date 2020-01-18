using System.Collections.Generic;
using Newtonsoft.Json;

namespace BBox.Client.Models
{
    public class ActionReply 
    {
        [JsonProperty("uid")]
        public int Uid { get; set; }
	
        [JsonProperty("id")]
        public int Id { get; set; }
	
        [JsonProperty("error")]
        public Result Error { get; set; }
	
        [JsonProperty("callbacks")]
        public List<ActionReplyCallback> Callbacks { get; set; }
    }
}