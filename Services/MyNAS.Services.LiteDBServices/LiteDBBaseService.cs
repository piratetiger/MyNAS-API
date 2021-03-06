using MyNAS.Services.Abstraction;
using MyNAS.Services.LiteDbServices.Helper;

namespace MyNAS.Services.LiteDbServices
{
    public abstract class LiteDBBaseService<T> : DefaultService<T> where T : IServiceBase
    {
        public override string Name { get; } = Constants.Service_Name;
        public override bool CacheService { get; } = false;
        public string ConnectionString { get; set; } = Constants.DB_FILE_NAME;

        protected LiteDBAccessor DBAccessor
        {
            get
            {
                return new LiteDBAccessor(ConnectionString);
            }
        }
    }
}