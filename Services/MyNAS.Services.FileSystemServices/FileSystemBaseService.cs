using MyNAS.Services.Abstraction;
using MyNAS.Services.FileSystemServices.Helper;

namespace MyNAS.Services.FileSystemServices
{
    public class FileSystemBaseService<T> : DefaultService<T> where T : IServiceBase
    {
        public override string Name { get; } = Constants.Service_Name;
        public override bool CacheService { get; } = false;
    }
}