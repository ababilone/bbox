using System.Collections.Generic;
using Newtonsoft.Json;

namespace BBox.Client.Models
{
    public class Reply
    {
        [JsonProperty("uid")]
        public int Uid { get; set; }
	
        [JsonProperty("id")]
        public int Id { get; set; }
	
        [JsonProperty("error")]
        public Result Error { get; set; }
	
        [JsonProperty("actions")]
        public List<ActionReply> Actions { get; set; }
    }
}