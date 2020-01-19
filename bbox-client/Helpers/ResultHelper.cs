using BBox.Client.Models;

namespace BBox.Client.Helpers
{
    public static class ResultHelper
    {
        public static bool IsError(this ActionReply actionReply)
        {
            return actionReply.Error.IsError();
        }
        
        public static bool IsError(this Result result)
        {
            return !result.IsSucceed();
        }
        
        public static bool IsSucceed(this Result result)
        {
            return  result.Code == 16777238 && result.Description == "Applied" ||
                    result.Code == 16777216 && result.Description == "Ok";
        }
    }
}