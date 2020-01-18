using BBox.Client.Models;
using Newtonsoft.Json;

namespace BBox.Client
{
    public static class AuthUtils
    {
        public static string GenerateAuthCookie(AuthContext authContext)
        {
            var hal = ComputeHal(authContext);
            if (hal.Length == 32)
                hal = hal.Substring(0, 10) + authContext.Md5Password + hal.Substring(10);
            
            var authCookie = new AuthCookie(authContext.User, hal, authContext.Nonce);

            return JsonConvert.SerializeObject(authCookie);
        }

        public static string ComputeAuthKey(AuthContext authContext, int requestIndex, int requestNonce)
        {
            var hal = ComputeHal(authContext);
            var authKey = $"{hal}:{requestIndex}:{requestNonce}:JSON:/cgi/json-req";
            return Md5Helper.Hash(authKey);
        }

        private static string ComputeHal(AuthContext authContext)
        {
            var hal = $"{authContext.User}:{authContext.Nonce}:{authContext.Md5Password}";
            return Md5Helper.Hash(hal);
        }

        private class AuthCookie
        {
            public AuthCookie(string user, string hal, long? nonce)
            {
                User = user;
                Hal = hal;
                Nonce = nonce;
            }
            
            [JsonProperty("req_id")] 
            public string RequestId { get; } = "0";

            [JsonProperty("sess_id")] 
            public string SessionId { get; } = "1";

            [JsonProperty("basic")] 
            public bool Basic { get; }

            [JsonProperty("user")]
            public string User { get; }
            
            [JsonProperty("dataModel")]
            public Nss DataModel { get; } = new Nss();

            [JsonProperty("nonce")]
            public long? Nonce { get; }
            
            [JsonProperty("hal")] 
            public string Hal { get; }
        }
    }
}