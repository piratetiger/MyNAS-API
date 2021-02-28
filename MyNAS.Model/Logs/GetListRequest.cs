using System;

namespace MyNAS.Model.Logs
{
    public class GetListRequest : IDateFilterRequest
    {
        public string Start { get; set; }
        public string End { get; set; }
        public DateTime StartDate
        {
            get
            {
                if (string.IsNullOrEmpty(Start))
                {
                    return DateTime.MinValue;
                }
                return DateTime.ParseExact(Start, "yyyyMMdd", null);
            }
        }
        public DateTime EndDate
        {
            get
            {
                if (string.IsNullOrEmpty(End))
                {
                    return DateTime.MaxValue;
                }
                return DateTime.ParseExact(End, "yyyyMMdd", null);
            }
        }
        public string Cate { get; set; }
    }
}