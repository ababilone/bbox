using BBox.Client.Models;

namespace BBox.Client
{
    public static class Actions
    {
        public static GetValueActionBuilder GetValue() => new GetValueActionBuilder();
        public static SetValueActionBuilder SetValue() => new SetValueActionBuilder();

        public static Action Login(string user)
        {
            return new Action
            {
                Method = "logIn",
                Parameters = new Parameters(user)
                {
                    SessionOptions = new SessionOptions()
                }
            };
        }
    }
}