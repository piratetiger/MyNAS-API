namespace MyNAS.Model
{
    public interface IEntityModel
    {
        byte[] FileBytes { get; set; }
        byte[] FileThumbBytes { get; set; }
    }
}