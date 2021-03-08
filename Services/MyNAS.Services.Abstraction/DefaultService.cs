namespace MyNAS.Services.Abstraction
{
    public abstract class DefaultService<T> : IServiceBase, ICollectionService<T> where T : IServiceBase
    {
        public abstract string Name { get; }
        public abstract bool CacheService { get; }
        
        public ServiceCollection<T> Services { get; set; }
    }
}