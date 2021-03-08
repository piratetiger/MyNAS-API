using System;

namespace MyNAS.Model.Videos
{
    public class VideoInfoModel : INASModel
    {
        [JsonIgnoreSerialization]
        public long Id { get; set; }
        [JsonIgnoreSerialization]
        public string KeyName
        {
            get
            {
                return FileName;
            }
        }
        public string FileName { get; set; }
        public DateTime Date { get; set; }
        public bool IsPublic { get; set; }
        public string Owner { get; set; }
        public string Cate { get; set; }
    }
}