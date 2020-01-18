using System.Collections.Generic;
using Newtonsoft.Json;

namespace BBox.Client.Models
{
    public class Request
    {
        public Request(int id, int sessionId, int cnonce, string authKey)
        {
            Id = id;
            SessionId = id == 0 ? 0 : sessionId;
            Priority = id == 0;
            CNonce = cnonce;
            AuthKey = authKey;
            
            Actions = new List<Action>();
        }
        
        [JsonProperty("id")]
        public int Id { get; }

        [JsonProperty("session-id")] 
        public int SessionId { get; }

        [JsonProperty("priority")]
        public bool Priority { get; }
        
        [JsonProperty("actions")]
        public List<Action> Actions { get; }

        [JsonProperty("cnonce")] 
        public int CNonce { get; }

        [JsonProperty("auth-key")]
        public string AuthKey { get; }
    }
}