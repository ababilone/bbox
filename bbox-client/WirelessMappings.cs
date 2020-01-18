using System.Collections.Generic;

namespace BBox.Client
{
    public static class WirelessMappings
    {
        public static Dictionary<WirelessType, string[]> AccessPoints = new Dictionary<WirelessType, string[]>
        {
            {WirelessType.Private, new[] {"PRIV0", "VID0"}},
            {WirelessType.Public, new[] {"PUB", "PUB1", "PUB2"}}
        };
    }
}