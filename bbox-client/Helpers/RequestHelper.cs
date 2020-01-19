using System.Linq;
using BBox.Client.Models;

namespace BBox.Client.Helpers
{
    public static class RequestHelper
    {
        public static Action GetAction(this Request request, int id)
        {
            return request.Actions.FirstOrDefault(action => action.Id == id);
        }
    }
}