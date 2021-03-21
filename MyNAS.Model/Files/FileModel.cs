namespace MyNAS.Model.Files
{
    public class FileModel : FileInfoModel, IContentModel
    {
        public byte[] Contents { get; set; }
        public byte[] ThumbContents { get; set; }
    }
}