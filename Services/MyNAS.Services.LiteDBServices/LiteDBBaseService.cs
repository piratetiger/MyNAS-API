using MyNAS.Services.Abstraction;
using MyNAS.Services.LiteDBServices.Helper;

namespace MyNAS.Services.LiteDBServices
{
    public abstract class LiteDBBaseService<T> : DefaultService<T> where T : IServiceBase
    {
        public override string Name { get; } = Constants.Service_Name;
        public override bool CacheService { get; } = false;

        protected LiteDBAccessor DBAccessor
        {
            get
            {
                return new LiteDBAccessor();
            }
        }
    }
}