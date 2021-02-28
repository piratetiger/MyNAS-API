using System;

namespace MyNAS.Model.Logs
{
    public class ErrorLogModel : IDateModel
    {
        public DateTime Date { get; set; }
        public string Level { get; set; }
        public string Logger { get; set; }
        public string Message { get; set; }
        public object Properties { get; set; }
        public string Cate { get; set; }
    }
}