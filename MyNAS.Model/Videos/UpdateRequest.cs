using System.Collections.Generic;

namespace MyNAS.Model.Videos
{
    public class UpdateRequest
    {
        public List<string> Names { get; set; } = new List<string>();
        public VideoModel NewModel { get; set; }
    }
}