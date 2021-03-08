namespace MyNAS.Model.Images
{
    public class ImageModel : ImageInfoModel, IEntityModel
    {
        public byte[] Contents { get; set; }
        public byte[] ThumbContents { get; set; }
    }
}