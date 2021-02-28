using System.Collections.Generic;

namespace MyNAS.Model.Videos
{
    public class DeleteRequest
    {
        public List<string> Names { get; set; } = new List<string>();
    }
}