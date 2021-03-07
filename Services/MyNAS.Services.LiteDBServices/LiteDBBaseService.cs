using MyNAS.Services.Abstraction;
using MyNAS.Services.LiteDbServices.Helper;

namespace MyNAS.Services.LiteDbServices
{
    public abstract class LiteDbBaseService<T> : DefaultService<T> where T : IServiceBase
    {
        public override string Name { get; } = Constants.Service_Name;
        public override bool CacheService { get; } = false;
        public string ConnectionString { get; set; } = Constants.DB_FILE_NAME;

        protected LiteDbAccessor DbAccessor
        {
            get
            {
                return new LiteDbAccessor(ConnectionString);
            }
        }
    }
}