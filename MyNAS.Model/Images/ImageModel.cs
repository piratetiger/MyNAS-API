namespace MyNAS.Model.Images
{
    public class ImageModel : ImageInfoModel, IContentModel
    {
        public byte[] Contents { get; set; }
        public byte[] ThumbContents { get; set; }
    }
}