using System;

namespace MyNAS.Model.Images
{
    public class ImageInfoModel : NASInfoModel
    {
        [JsonIgnoreSerialization]
        public override long Id { get; set; }
        public override string Type { get => "image"; }
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