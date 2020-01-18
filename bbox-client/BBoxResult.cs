using System.Collections.Generic;
using System.Linq;
using System.Text;
using BBox.Client.Models;

namespace BBox.Client
{
    public class BBoxResult<T> : BBoxResult
    {
        public T Result { get; set; }

        public BBoxResult(Reply reply) : base(reply)
        {
        }
        
        public BBoxResult(Reply reply, T result) : base(reply)
        {
            Result = result;
        }
    }
    
    public class BBoxResult
    {
        private BBoxResult(ActionReply actionReply) : this(actionReply.Error)
        {
        }
        
        public BBoxResult(Reply reply) : this(reply.Error, reply.Actions.Select(actionReply => new BBoxResult(actionReply)).ToArray())
        {
            
        }

        private BBoxResult(Result result, params BBoxResult[] innerResults)
        {
            Code = result.Code;
            Description = result.Description;
            Succeed = result.Code == 16777216 && result.Description == "Ok";
            InnerResults = innerResults.ToList();
        }
        
        public int Code { get; }
        public string Description { get; }
        public bool Succeed { get; }
        public List<BBoxResult> InnerResults { get; }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder(Description);
            
            foreach (var innerResult in InnerResults)
            {
                stringBuilder.AppendLine();
                stringBuilder.Append(innerResult);
            }
            
            return stringBuilder.ToString();
        }
    }
}