using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BBox.Client.Configuration;
using BBox.Client.Models;
using Flurl.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Action = BBox.Client.Models.Action;

namespace BBox.Client
{
    public class BBoxClient : IBBoxClient
    {
        private readonly BBoxClientConfiguration _bboxClientConfiguration;
        private readonly ILogger<BBoxClient> _logger;
        private readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore};

        private readonly FlurlClient _flurlClient;
        private readonly Random _random = new Random();
        private const string CookieName = "bbox3_session";
        private int _requestIndex;
        private int _sessionId;
        private AuthContext _authContext;
        
        public string Uri { get; }
        public string User { get; set; }

        public BBoxClient(BBoxClientConfiguration bboxClientConfiguration, ILogger<BBoxClient> logger)
        {
            _bboxClientConfiguration = bboxClientConfiguration;
            _logger = logger;
            _flurlClient = new FlurlClient(_bboxClientConfiguration.Uri);

            Uri = _bboxClientConfiguration.Uri;
            User = _bboxClientConfiguration.User;
        }

        private int GenerateNonce() => _random.Next();
        private int NextRequestIndex() => _requestIndex++;

        public Task<BBoxResult> LoginAsync()
        {
            return LoginAsync(_bboxClientConfiguration.User, _bboxClientConfiguration.Password);
        }
        
        public async Task<BBoxResult> LoginAsync(string user, string password)
        {
            _authContext = new AuthContext(user, password, null);

            if (_flurlClient.Cookies.ContainsKey(CookieName))
                _flurlClient.Cookies.Remove(CookieName);

            var authCookie = AuthUtils.GenerateAuthCookie(_authContext);
            _flurlClient.Cookies.Add(CookieName, new Cookie(CookieName, authCookie));

            var apiReply = await SendAsync(Actions.Login(user));
            var bBoxResult = new BBoxResult(apiReply.Reply);
            
            if (!bBoxResult.Succeed)
                return bBoxResult;
            
            var parameters = apiReply.Reply.Actions.FirstOrDefault()?.Callbacks.FirstOrDefault()?.Parameters;
            _sessionId = parameters.Id;
            _authContext.Nonce = parameters.Nonce;
            
            return bBoxResult;
        }

        public async Task<ApiReply> SendAsync(params Action[] actions)
        {
            if (_authContext == null)
            {
                _logger.LogInformation($"Connecting to {Uri}");
                var bBoxResult = await LoginAsync();
                if (bBoxResult.Succeed)
                    _logger.LogInformation($"Logged in as {User}");
                else
                    _logger.LogError($"Error during login as {User}: {bBoxResult.Description}");
            }
            
            var requestIndex = NextRequestIndex();
            var requestNonce = GenerateNonce();
            var requestAuthKey = AuthUtils.ComputeAuthKey(_authContext, requestIndex, requestNonce);

            var request = new Request(requestIndex, _sessionId, requestNonce, requestAuthKey);
            foreach (var action in actions)
                request.AddAction(action);

            var apiCall = new ApiCall {request = request};
            var httpResponseMessage = await _flurlClient.Request("cgi", "json-req").PostUrlEncodedAsync(new
            {
                req = JsonConvert.SerializeObject(apiCall, _jsonSerializerSettings)
            });

            var content = await httpResponseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ApiReply>(content);
        }

        public async Task<List<AccessPointInfo>> GetAccessPointsAsync()
        {
            var apiReply = await SendAsync(Actions.GetValue().AccessPoints());
            var accessPointInfos = new List<AccessPointInfo>();
            foreach (var callback in apiReply.Reply.Actions?.FirstOrDefault()?.Callbacks ?? new List<ActionReplyCallback>())
            {
                accessPointInfos.Add(new AccessPointInfo
                {
                    Capability = callback.Parameters.Capability,
                    Metadata = callback.Parameters.Value.AccessPoint
                });
            }

            return accessPointInfos;
        }

        public async Task<BBoxResult<List<SSIDInfo>>> GetSSIDAsync()
        {
            var apiReply = await SendAsync(Actions.GetValue().SSIDs());

            var ssidInfos = new List<SSIDInfo>();
            foreach (var callback in apiReply.Reply.Actions?.FirstOrDefault()?.Callbacks  ?? new List<ActionReplyCallback>())
            {
                ssidInfos.Add(new SSIDInfo
                {
                    Capability = callback.Parameters.Capability,
                    Metadata = callback.Parameters.Value.SSID
                });
            }

            return new BBoxResult<List<SSIDInfo>>(apiReply.Reply, ssidInfos);
        }

        private async Task<BBoxResult> EnableDisableWirelessAsync(WirelessType wirelessType, Func<SetValueActionBuilder.SetValueEnableActionBuilder, Action> setValueEnableActionBuilderAction)
        {
            if (wirelessType.HasFlag(WirelessType.Public))
                throw new NotSupportedException("Cannot handle public wireless");

            var privateAccessPointAliases = WirelessMappings.AccessPoints.Where(pair => wirelessType.HasFlag(pair.Key))
                .SelectMany(pair => pair.Value).ToArray();

            var actions = new List<Action>();

            var accessPointsInfos = await GetAccessPointsAsync();
            foreach (var accessPoint in accessPointsInfos.Where(info =>
                privateAccessPointAliases.Contains(info.Metadata.Alias)))
            {
                actions.Add(
                    setValueEnableActionBuilderAction(Actions.SetValue().AccessPoint(accessPoint.Metadata.Uid)));
                actions.Add(
                    setValueEnableActionBuilderAction(Actions.SetValue().SSID(accessPoint.Metadata.SSIDReference)));
            }

            actions.Add(setValueEnableActionBuilderAction(Actions.SetValue().BGCButtons("GuiEnable_5G")));
            actions.Add(setValueEnableActionBuilderAction(Actions.SetValue().BGCButtons("GuiEnable_2G4")));
            
            var apiReply = await SendAsync(actions.ToArray());
            
            return new BBoxResult(apiReply.Reply);
        }

        public Task<BBoxResult> EnableWirelessAsync(WirelessType wirelessType) => EnableDisableWirelessAsync(wirelessType, builder => builder.Enabled());

        public Task<BBoxResult> DisableWirelessAsync(WirelessType wirelessType) => EnableDisableWirelessAsync(wirelessType, builder => builder.Disabled());

        public void Dispose() => _flurlClient?.Dispose();
    }
}