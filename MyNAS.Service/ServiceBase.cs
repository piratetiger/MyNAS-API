namespace MyNAS.Service
{
    public abstract class ServiceBase
    {
        protected LiteDBAccessor DBAccessor
        {
            get
            {
                return new LiteDBAccessor();
            }
        }
    }
}