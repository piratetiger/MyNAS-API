namespace MyNAS.Model.Files
{
    public class FileModel : FileInfoModel, IEntityModel
    {
        public byte[] FileBytes { get; set; }
        public byte[] FileThumbBytes { get; set; }
    }
}