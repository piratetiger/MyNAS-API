using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace MyNAS.Model
{
    public class DataResult<T>
    {
        private string _source;
        private IEnumerable<T> _data;
        private DataStatus _dataStatus;
        private string _message;

        public string Source
        {
            get
            {
                return _source;
            }
            set
            {
                _source = value;
            }
        }

        public IEnumerable<T> Data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
                _dataStatus = value == null ? DataStatus.Failed : DataStatus.Success;
            }
        }

        public DataStatus Status
        {
            get
            {
                return _dataStatus;
            }
        }

        public virtual string Message
        {
            get
            {
                return _message;
            }
        }

        [JsonIgnore]
        public T First
        {
            get
            {
                if (Status == DataStatus.Success && Data != null)
                {
                    return Data.FirstOrDefault();
                }

                return default(T);
            }
        }

        public DataResult(string source)
        {
            _source = source;
        }

        public DataResult(string source, IEnumerable<T> data) : this(source)
        {
            Data = data;
        }

        public DataResult(string source, IEnumerable<T> data, DataStatus status) : this(source, data)
        {
            _dataStatus = status;
        }

        public DataResult(string source, IEnumerable<T> data, string message) : this(source, data)
        {
            _message = message;
        }

        public DataResult(string source, IEnumerable<T> data, DataStatus status, string message) : this(source, data)
        {
            _dataStatus = status;
            _message = message;
        }
    }
}