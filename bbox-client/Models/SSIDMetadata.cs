using Newtonsoft.Json;

namespace BBox.Client.Models
{
    public class SSIDMetadata
    {
        [JsonProperty("uid")]
        public int Uid { get; set; }
        public bool Enable { get; set; }
        public string StoppedBy { get; set; }
        public string Status { get; set; }
        public string Alias { get; set; }
        public string Name { get; set; }
        public int LastChange { get; set; }
        public string LowerLayers { get; set; }
        public int ConnectionTime { get; set; }
        public string IfcName { get; set; }
        public string BSSID { get; set; }
        public string MACAddress { get; set; }
        public string SSID { get; set; }
        public Stats Stats { get; set; }
    }
}