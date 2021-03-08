using System;

namespace MyNAS.Model.Files
{
    public class FileInfoModel : INASModel
    {
        [JsonIgnoreSerialization]
        public long Id { get; set; }
        public string KeyName { get; set; }
        public string FileName { get; set; }
        public DateTime Date { get; set; }
        public bool IsPublic { get; set; }
        public string Owner { get; set; }
        public string Cate { get; set; }
        [JsonIgnoreSerialization]
        public string PathName { get; set; }
        public bool IsFolder { get; set; }
    }
}