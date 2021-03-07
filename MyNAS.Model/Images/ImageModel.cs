namespace MyNAS.Model.Images
{
    public class ImageModel : ImageInfoModel, IEntityModel
    {
        public byte[] FileBytes { get; set; }
        public byte[] FileThumbBytes { get; set; }
    }
}