using System;

namespace MyNAS.Model.Videos
{
    public class VideoInfoModel : NASInfoModel
    {
        [JsonIgnoreSerialization]
        public override long Id { get; set; }
        public override string Type { get => "video"; }
        [JsonIgnoreSerialization]
        public override string KeyName
        {
            get
            {
                return FileName;
            }
            set
            {
                FileName = value;
            }
        }
        public DateTime OriginalDateTime { get; set; }
    }
}