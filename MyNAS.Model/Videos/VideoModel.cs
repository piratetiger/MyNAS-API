namespace MyNAS.Model.Videos
{
    public class VideoModel : VideoInfoModel, IEntityModel
    {
        public byte[] FileBytes { get; set; }
        public byte[] FileThumbBytes { get; set; }
    }
}