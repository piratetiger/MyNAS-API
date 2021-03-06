namespace MyNAS.Services.Abstraction
{
    public interface IServiceBase
    {
        string Name { get; }
        bool CacheService { get; }
    }
}