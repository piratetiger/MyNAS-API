using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MyNAS.Services.Abstraction
{
    public class ServiceCollection<T> : IEnumerable<T> where T : IServiceBase
    {
        private IEnumerable<T> _services;
        private IList<T> _activedServices;
        private bool _refreshFlag = true;
        private IList<string> _filterOrder;

        public IList<string> FilterOrder
        {
            get
            {
                return _filterOrder;
            }
            set
            {
                _filterOrder = value;
                _refreshFlag = true;
            }
        }

        public IList<T> ActivedServices
        {
            get
            {
                if (_activedServices == null || _refreshFlag)
                {
                    if (FilterOrder != null)
                    {
                        _activedServices = _services.Where(s => FilterOrder.Contains(s.Name)).OrderBy(s => FilterOrder.IndexOf(s.Name)).ToList();
                    }
                    else
                    {
                        _activedServices = _services.ToList();
                    }
                }

                return _activedServices;
            }
        }

        public ServiceCollection(IEnumerable<T> services)
        {
            _services = services;

            foreach (var service in _services)
            {
                var collectionService = service as ICollectionService<T>;
                if (collectionService != null)
                {
                    collectionService.Services = this;
                }
            }
        }

        public T Next(T item)
        {
            if (ActivedServices != null)
            {
                var index = ActivedServices.IndexOf(item);
                if (index > -1 && index + 1 < ActivedServices.Count)
                {
                    return ActivedServices[index + 1];
                }
            }

            return default(T);
        }

        public T First()
        {
            if (ActivedServices != null)
            {
                return ActivedServices.FirstOrDefault();
            }

            return default(T);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return ActivedServices.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ActivedServices.GetEnumerator();
        }
    }
}