using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BBox.Client.Models;
using Action = BBox.Client.Models.Action;

namespace BBox.Client
{
    public interface IBBoxClient : IDisposable
    {
        Task<BBoxResult> LoginAsync();
        Task<BBoxResult> LoginAsync(string user, string password);
        Task<ApiReply> SendAsync(params Action[] actions);
        Task<List<AccessPointInfo>> GetAccessPointsAsync();
        Task<BBoxResult<List<SSIDInfo>>> GetSSIDAsync();
        Task<BBoxResult> EnableWirelessAsync(WirelessType wirelessType);
        Task<BBoxResult> DisableWirelessAsync(WirelessType wirelessType);
        string Uri { get; }
        string User { get; }
    }
}