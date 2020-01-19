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

            var bBoxResult = await SendAsync(Actions.Login(user));
            
            if (!bBoxResult.Succeed)
                return bBoxResult;
            
            var parameters = bBoxResult.Reply.Actions.FirstOrDefault()?.Callbacks.FirstOrDefault()?.Parameters;
            _sessionId = parameters.Id;
            _authContext.Nonce = parameters.Nonce;
            
            return bBoxResult;
        }

        public async Task<BBoxResult> SendAsync(params Action[] actions)
        {
            if (_authContext == null)
            {
                _logger.LogInformation($"Connecting to {Uri}");
                var bBoxResult = await LoginAsync();
                if (bBoxResult.Succeed)
                    _logger.LogInformation($"Logged in as {User}");
                else
                {
                    return bBoxResult;
                }
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
            var apiReply = JsonConvert.DeserializeObject<ApiReply>(content);
            return new BBoxResult(request, apiReply.Reply);
        }

        public async Task<BBoxResult<List<AccessPointInfo>>> GetAccessPointsAsync()
        {
            var bboxResult = await SendAsync(Actions.GetValue().AccessPoints());
            var typedBBoxResult = new BBoxResult<List<AccessPointInfo>>(bboxResult);

            if (typedBBoxResult.Succeed)
            {
                var accessPointInfos = new List<AccessPointInfo>();
                foreach (var callback in typedBBoxResult.Reply.Actions?.FirstOrDefault()?.Callbacks ??
                                         new List<ActionReplyCallback>())
                {
                    accessPointInfos.Add(new AccessPointInfo
                    {
                        Capability = callback.Parameters.Capability,
                        Metadata = callback.Parameters.Value.AccessPoint
                    });
                }

                typedBBoxResult.Result = accessPointInfos;
            }

            return typedBBoxResult;
        }

        public async Task<BBoxResult<List<SSIDInfo>>> GetSSIDAsync()
        {
            var bboxResult = await SendAsync(Actions.GetValue().SSIDs());
            var typedBBoxResult = new BBoxResult<List<SSIDInfo>>(bboxResult);

            if (typedBBoxResult.Succeed)
            {
                var ssidInfos = new List<SSIDInfo>();
                foreach (var callback in typedBBoxResult.Reply.Actions?.FirstOrDefault()?.Callbacks ??
                                         new List<ActionReplyCallback>())
                {
                    ssidInfos.Add(new SSIDInfo
                    {
                        Capability = callback.Parameters.Capability,
                        Metadata = callback.Parameters.Value.SSID
                    });
                }

                typedBBoxResult.Result = ssidInfos;
            }

            return typedBBoxResult;
        }

        private async Task<BBoxResult> EnableDisableWirelessAsync(WirelessType wirelessType, Func<SetValueActionBuilder.SetValueEnableActionBuilder, Action> setValueEnableActionBuilderAction)
        {
            if (wirelessType.HasFlag(WirelessType.Public))
                throw new NotSupportedException("Cannot handle public wireless");

            var privateAccessPointAliases = WirelessMappings.AccessPoints.Where(pair => wirelessType.HasFlag(pair.Key))
                .SelectMany(pair => pair.Value).ToArray();

            var actions = new List<Action>();

            var accessPointsInfos = (await GetAccessPointsAsync()).Result;
            var ssidInfos = (await GetSSIDAsync()).Result;
            foreach (var accessPoint in accessPointsInfos.Where(info =>
                privateAccessPointAliases.Contains(info.Metadata.Alias)))
            {
                actions.Add(setValueEnableActionBuilderAction(Actions.SetValue().AccessPoint(accessPoint.Metadata.Uid)));

                var ssidInfo = ssidInfos.FirstOrDefault(info => info.Metadata.Name == accessPoint.Metadata.SSIDReference);
                
                actions.Add(setValueEnableActionBuilderAction(Actions.SetValue().SSID(ssidInfo.Metadata.Uid)));
            }

            actions.Add(setValueEnableActionBuilderAction(Actions.SetValue().BGCButtons("GuiEnable_5G")));
            actions.Add(setValueEnableActionBuilderAction(Actions.SetValue().BGCButtons("GuiEnable_2G4")));
            
            return await SendAsync(actions.ToArray());
        }

        public Task<BBoxResult> EnableWirelessAsync(WirelessType wirelessType) => EnableDisableWirelessAsync(wirelessType, builder => builder.Enabled());

        public Task<BBoxResult> DisableWirelessAsync(WirelessType wirelessType) => EnableDisableWirelessAsync(wirelessType, builder => builder.Disabled());

        public void Dispose() => _flurlClient?.Dispose();
    }
}