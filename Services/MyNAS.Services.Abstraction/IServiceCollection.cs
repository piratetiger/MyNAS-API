namespace MyNAS.Services.Abstraction
{
    public interface ICollectionService<T> where T : IServiceBase
    {
        ServiceCollection<T> Services { get; set; }
    }
}