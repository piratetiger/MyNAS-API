namespace MyNAS.Model.Videos
{
    public class VideoModel : VideoInfoModel, IContentModel
    {
        public byte[] Contents { get; set; }
        public byte[] ThumbContents { get; set; }
    }
}