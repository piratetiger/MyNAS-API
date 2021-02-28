using System.Collections.Generic;

namespace MyNAS.Model.Images
{
    public class DeleteRequest
    {
        public List<string> Names { get; set; } = new List<string>();
    }
}