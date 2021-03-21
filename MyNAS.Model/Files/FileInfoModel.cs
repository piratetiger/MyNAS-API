using System;

namespace MyNAS.Model.Files
{
    public class FileInfoModel : NASInfoModel
    {
        [JsonIgnoreSerialization]
        public override long Id { get; set; }
        [JsonIgnoreSerialization]
        public string PathName { get; set; }
        public bool IsFolder { get; set; }
    }
}