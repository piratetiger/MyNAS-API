namespace MyNAS.Service
{
    public abstract class ServiceBase
    {
        protected LiteDbAccessor DbAccessor
        {
            get
            {
                return new LiteDbAccessor();
            }
        }
    }
}