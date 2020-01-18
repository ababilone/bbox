using System.Collections.Generic;
using Newtonsoft.Json;

namespace BBox.Client.Models
{
    public class AccessPoint
    {
        [JsonProperty("uid")]
        public int Uid { get; set; }
        public bool Enable { get; set; }
        public string Status { get; set; }
        public string Alias { get; set; }
        public string SSIDReference { get; set; }
        public bool SSIDAdvertisementEnabled { get; set; }
        public int RetryLimit { get; set; }
        public bool WMMCapability { get; set; }
        public bool UAPSDCapability { get; set; }
        public bool WMMEnable { get; set; }
        public bool UAPSDEnable { get; set; }
        public bool DirectMulticast { get; set; }
        public MACFiltering MACFiltering { get; set; }
        public Security Security { get; set; }
        public WPS WPS { get; set; }
        public List<object> AssociatedDevices { get; set; }
        public List<object> ACs { get; set; }
        public string AuthenticationServiceMode { get; set; }
        public string BasicAuthenticationMode { get; set; }
        public string BasicDataTransmitRates { get; set; }
        public string OperationalDataTransmitRates { get; set; }
        public string PossibleDataTransmitRates { get; set; }
        public string Bridge { get; set; }
        public string Ip { get; set; }
        public bool WDSMode { get; set; }
        public int MaxAssociatedDevices { get; set; }
        public bool AssociationForbidden { get; set; }
        public bool IsolationEnable { get; set; }
        public Accounting Accounting { get; set; }
        public string RadiusOwnIpInterface { get; set; }
        public bool RadiusOwnIpv6Enable { get; set; }
        public RadioMeasurements RadioMeasurements { get; set; }
        public string ProxyMode { get; set; }
    }
}