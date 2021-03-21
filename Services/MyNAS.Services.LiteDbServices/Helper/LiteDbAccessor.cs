using System;
using System.Collections.Generic;
using System.Linq;
using LiteDB;
using MyNAS.Model;

namespace MyNAS.Services.LiteDbServices.Helper
{
    public class LiteDbAccessor
    {
        private string _connectionString;

        public LiteDbAccessor(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<T> GetAll<T>(string name) where T : IKeyNameModel
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                var collection = db.GetCollection<T>(name);
                return collection.FindAll().ToList();
            }
        }

        public List<T> SearchItems<T>(string name, IDateFilterRequest req) where T : IDateModel
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                var collection = db.GetCollection<T>(name);
                return collection.Find(i => ((i.Date >= req.StartDate) && (i.Date <= req.EndDate.AddDays(1))) &&
                                      (i.Cate == req.Cate))
                            .OrderByDescending(i => i.Date)
                            .ToList();
            }
        }

        public List<T> SearchItems<T>(string name, IOwnerFilterRequest req) where T : IOwnerModel
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                var collection = db.GetCollection<T>(name);
                if (req.Owner == null || !req.Owner.Any())
                {
                    return collection.Find(i => i.Cate == req.Cate)
                                    .OrderBy(i => i.Owner)
                                    .ToList();
                }
                else
                {
                    return collection.Find(i => (req.Owner.Contains(i.Owner) &&
                                          (i.Cate == req.Cate)))
                                .OrderBy(i => i.Owner)
                                .ToList();
                }
            }
        }

        public List<T> SearchItems<T>(string name, INASFilterRequest req) where T : NASInfoModel
        {
            if (req.Owner == null || !req.Owner.Any())
            {
                return SearchItems<T>(name, (IDateFilterRequest)req);
            }

            if (req.StartDate == DateTime.MinValue || req.EndDate == DateTime.MinValue)
            {
                return SearchItems<T>(name, (IOwnerFilterRequest)req);
            }

            using (var db = new LiteDatabase(_connectionString))
            {
                var collection = db.GetCollection<T>(name);
                return collection.Find(i => ((i.Date >= req.StartDate) && (i.Date <= req.EndDate.AddDays(1))) &&
                                            (req.Owner.Contains(i.Owner) &&
                                            (i.Cate == req.Cate)))
                            .OrderByDescending(i => i.Date)
                            .ToList();
            }
        }

        public bool SaveItem<T>(string name, T item) where T : IKeyNameModel
        {
            if (item == null)
            {
                return false;
            }

            try
            {
                using (var db = new LiteDatabase(_connectionString))
                {
                    var collection = db.GetCollection<T>(name);
                    var checkItem = collection.FindOne(i => i.KeyName == item.KeyName);
                    if (checkItem != null)
                    {
                        return false;
                    }
                    collection.Insert(item);
                    collection.EnsureIndex(i => i.KeyName);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool SaveItems<T>(string name, IEnumerable<T> items) where T : IKeyNameModel
        {
            if (items == null)
            {
                return false;
            }

            try
            {
                using (var db = new LiteDatabase(_connectionString))
                {
                    var collection = db.GetCollection<T>(name);
                    var checkItem = collection.FindOne(i => items.Select(ii => ii.KeyName).Contains(i.KeyName));
                    if (checkItem != null)
                    {
                        return false;
                    }
                    collection.InsertBulk(items);
                    collection.EnsureIndex(i => i.KeyName);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteItem<T>(string name, T item) where T : IKeyNameModel
        {
            if (item == null)
            {
                return false;
            }

            try
            {
                using (var db = new LiteDatabase(_connectionString))
                {
                    var collection = db.GetCollection<T>(name);
                    var record = collection.Delete(i => i.KeyName == item.KeyName);
                    return record > 0;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteItems<T>(string name, IEnumerable<T> items) where T : IKeyNameModel
        {
            if (items == null)
            {
                return false;
            }

            try
            {
                using (var db = new LiteDatabase(_connectionString))
                {
                    var collection = db.GetCollection<T>(name);
                    var deleteKeys = items.Select(i => i.KeyName);
                    var record = collection.Delete(i => deleteKeys.Contains(i.KeyName));
                    return record == items.Count();
                }
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateItem<T>(string name, T item) where T : IKeyNameModel
        {
            if (item == null)
            {
                return false;
            }

            try
            {
                using (var db = new LiteDatabase(_connectionString))
                {
                    var collection = db.GetCollection<T>(name);
                    if (item.Id == 0)
                    {
                        var dbItem = collection.FindOne(i => i.KeyName == item.KeyName);
                        if (dbItem != null)
                        {
                            item.Id = dbItem.Id;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    collection.Update(item);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateItems<T>(string name, IEnumerable<T> items) where T : IKeyNameModel
        {
            if (items == null)
            {
                return false;
            }

            try
            {
                using (var db = new LiteDatabase(_connectionString))
                {
                    var collection = db.GetCollection<T>(name);
                    var checkCount = collection.LongCount(i => items.Select(ii => ii.KeyName).Contains(i.KeyName));
                    if (checkCount != items.Count())
                    {
                        return false;
                    }
                    collection.Update(items);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public T GetItem<T>(string name, string keyName) where T : IKeyNameModel
        {
            if (string.IsNullOrEmpty(keyName))
            {
                return default(T);
            }
            try
            {
                using (var db = new LiteDatabase(_connectionString))
                {
                    var collection = db.GetCollection<T>(name);
                    return collection.FindOne(i => i.KeyName == keyName);
                }
            }
            catch
            {
                return default(T);
            }
        }

        public List<T> GetItems<T>(string name, IEnumerable<string> keyNames) where T : IKeyNameModel
        {
            if (keyNames == null)
            {
                return new List<T>();
            }
            try
            {
                using (var db = new LiteDatabase(_connectionString))
                {
                    var collection = db.GetCollection<T>(name);
                    return collection.Find(i => keyNames.Contains(i.KeyName)).ToList();
                }
            }
            catch
            {
                return new List<T>();
            }
        }
    }
}
