using System.Collections.Generic;
using Newtonsoft.Json;

namespace MyNAS.Model
{
    public class MessageDataResult : DataResult<bool>
    {
        private string _action;

        public bool MessageResult
        {
            get
            {
                return true;
            }
        }

        public string MessageType
        {
            get
            {
                return ActionResult ? "Success" : "Error";
            }
        }

        [JsonIgnore]
        public bool ActionResult
        {
            get
            {
                return Status == DataStatus.Success && First;
            }
        }

        public override string Message
        {
            get
            {
                return $"{_action} {(ActionResult ? "Success" : "Failed")}";
            }
        }

        public MessageDataResult(DataResult<bool> dataResult, string action) : base(dataResult.Source, dataResult.Data)
        {
            _action = action;
        }

        public MessageDataResult(string source, bool result, string action) : base(source, new List<bool>() { result })
        {
            _action = action;
        }
    }
}