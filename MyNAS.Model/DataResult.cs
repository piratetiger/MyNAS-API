namespace MyNAS.Model
{
    public class DataResult<T>
    {
        public DataResult(T data)
        {
            Data = data;
        }

        public T Data { get; set; }
        public virtual string Message { get; set; }
    }
}