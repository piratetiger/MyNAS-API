namespace MyNAS.Model
{
    public interface IEntityModel
    {
        byte[] Contents { get; set; }
        byte[] ThumbContents { get; set; }
    }
}