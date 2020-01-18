using System.Collections.Generic;
using Newtonsoft.Json;

namespace BBox.Client.Models
{
    public class SessionOptions
    {
        [JsonProperty("nss")]
        public List<Nss> Nss { get; set; } = new List<Nss> { new Nss() };
        
        [JsonProperty("language")]
        public string Language { get; set; } = "ident";
        
        [JsonProperty("context-flags")]
        public ContextFlags ContextFlags { get; set; } = new ContextFlags();

        [JsonProperty("capability-depth")] 
        public int CapabilityDepth { get; set; } = 2;
        
        [JsonProperty("capability-flags")]
        public CapabilityFlags CapabilityFlags { get; set; } = new CapabilityFlags();

        [JsonProperty("time-format")] 
        public string TimeFormat { get; set; } = "ISO_8601";

        [JsonProperty("depth")]
        public int Depth { get; set; } = 6;

        [JsonProperty("max-add-events")] 
        public int MaxAddEvents { get; set; } = 5;

        [JsonProperty("write-only-string")] 
        public string WriteOnlyString { get; set; } = "_XMO_WRITE_ONLY_";

        [JsonProperty("undefined-write-only-string")]
        public string UndefinedWriteOnlyString { get; set; } = "_XMO_UNDEFINED_WRITE_ONLY_";
    }
}