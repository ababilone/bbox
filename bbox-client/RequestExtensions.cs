using BBox.Client.Models;

namespace BBox.Client
{
    public static class RequestExtensions
    {
        public static Request AddAction(this Request request, Action action)
        {
            action.Id = request.Actions.Count;
            request.Actions.Add(action);
            return request;
        }
    }
}