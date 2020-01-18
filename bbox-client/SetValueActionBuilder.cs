using BBox.Client.Models;

namespace BBox.Client
{
    public class SetValueActionBuilder
    {
        public class SetValueEnableActionBuilder
        {
            private readonly Action _action;

            public SetValueEnableActionBuilder(Action action, bool setPathSuffix = true)
            {
                _action = action;
                _action.Parameters = new Parameters();

                if (setPathSuffix)
                    _action.XPath += "/Enable";
            }

            public Action Enabled()
            {
                _action.Parameters.Value = true;
                return _action;
            }

            public Action Disabled()
            {
                _action.Parameters.Value = false;
                return _action;
            }
        }
        
        private readonly Action _action = new Action
        {
            Method = "setValue"
        };

        public SetValueEnableActionBuilder AccessPoint(string xpath)
        {
            _action.XPath = xpath;
            return new SetValueEnableActionBuilder(_action);
        }
        
        public SetValueEnableActionBuilder AccessPoint(int uid)
        {
            _action.XPath = $"Device/WiFi/AccessPoints/AccessPoint[@uid='{uid}']";
            return new SetValueEnableActionBuilder(_action);
        }
        
        public SetValueEnableActionBuilder SSID(string xpath)
        {
            _action.XPath = xpath;
            return new SetValueEnableActionBuilder(_action);
        }
        
        public SetValueEnableActionBuilder SSID(int uid)
        {
            _action.XPath = $"Device/WiFi/AccessPoints/AccessPoint[@uid='{uid}']";
            return new SetValueEnableActionBuilder(_action);
        }
        
        public SetValueEnableActionBuilder BGCButtons(string buttonName)
        {
            _action.XPath = $"Device/Services/BGCButton/{buttonName}";
            return new SetValueEnableActionBuilder(_action, false);
        }
    }
}