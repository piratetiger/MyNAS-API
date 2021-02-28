using System.Collections.Generic;

namespace MyNAS.Model.Images
{
    public class UpdateRequest
    {
        public List<string> Names { get; set; } = new List<string>();
        public ImageModel NewModel { get; set; }
    }
}