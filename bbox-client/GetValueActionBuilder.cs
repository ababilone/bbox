using BBox.Client.Models;

namespace BBox.Client
{
    public class GetValueActionBuilder
    {
        private Action _action = new Action
        {
            Method = "getValue"
        };

        public Action SSIDs()
        {
            _action.XPath = "Device/WiFi/SSIDs/SSID";
            return _action;
        }

        public Action AccessPoints()
        {
            _action.XPath = "Device/WiFi/AccessPoints/AccessPoint";
            return _action;
        }

        public Action BGCButtons()
        {
            _action.XPath = "Device/Services/BGCButton";
            return _action;
        }
        
        public Action Devices()
        {
            _action.XPath = "Device/Hosts/Hosts/Host";
            return _action;
        }
    }
}