namespace MyNAS.Model
{
    public interface IContentModel
    {
        byte[] Contents { get; set; }
        byte[] ThumbContents { get; set; }
    }
}