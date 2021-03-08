namespace MyNAS.Model.Videos
{
    public class VideoModel : VideoInfoModel, IEntityModel
    {
        public byte[] Contents { get; set; }
        public byte[] ThumbContents { get; set; }
    }
}