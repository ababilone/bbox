using System.Collections.Generic;
using Newtonsoft.Json;

namespace BBox.Client.Models
{
    public class Command
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("output-parameters")]
        public List<OutputParameter> OutputParameters { get; set; }
    }
}