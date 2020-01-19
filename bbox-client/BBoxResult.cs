using System.Text;
using BBox.Client.Helpers;
using BBox.Client.Models;

namespace BBox.Client
{
    public class BBoxResult<T> : BBoxResult
    {
        public T Result { get; set; }

        public BBoxResult(BBoxResult bboxResult) : base(bboxResult.Request, bboxResult.Reply)
        {
            
        }
    }
    
    public class BBoxResult
    {
        public Request Request { get; }
        public Reply Reply { get; }

        public BBoxResult(Request request, Reply reply)
        {
            Request = request;
            Reply = reply;
            Succeed = reply.Error.IsSucceed();
        }

        public bool Succeed { get; }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();

            foreach (var actionReply in Reply.Actions)
            {
                if (actionReply.IsError())
                {
                    var action = Request.GetAction(actionReply.Id);
                    stringBuilder.AppendLine($"{action} > {actionReply.Error.Description}");
                }
            }
            
            return stringBuilder.ToString().Trim();
        }
    }
}