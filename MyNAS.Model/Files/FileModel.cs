namespace MyNAS.Model.Files
{
    public class FileModel : FileInfoModel, IEntityModel
    {
        public byte[] Contents { get; set; }
        public byte[] ThumbContents { get; set; }
    }
}