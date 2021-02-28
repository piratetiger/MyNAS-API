using System.Collections.Generic;
using System.Linq;
using MyNAS.Model.Files;
using MyNAS.Service.Helper;

namespace MyNAS.Service
{
    public class FilesService : ServiceBase
    {
        public List<FileModel> GetList(GetListRequest req)
        {
            return DBAccessor.SearchItems<FileModel>(Constants.TABLE_FILES, req);
        }

        public bool SaveItem(FileModel item)
        {
            return DBAccessor.SaveItem(Constants.TABLE_FILES, item);
        }

        public bool SaveItems(List<FileModel> items)
        {
            return DBAccessor.SaveItems(Constants.TABLE_FILES, items);
        }

        public bool DeleteItems(List<string> names)
        {
            if (names == null)
            {
                return false;
            }

            var deleteItems = names.Select(n => new FileModel { KeyName = n }).ToList();
            return DBAccessor.DeleteItems(Constants.TABLE_FILES, deleteItems);
        }

        public bool UpdateItems(List<FileModel> items)
        {
            return DBAccessor.UpdateItems(Constants.TABLE_FILES, items);
        }

        public FileModel GetItem(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }

            return DBAccessor.GetItem<FileModel>(Constants.TABLE_FILES, name);
        }

        public List<FileModel> GetItems(List<string> names)
        {
            if (names == null)
            {
                return new List<FileModel>();
            }

            return DBAccessor.GetItems<FileModel>(Constants.TABLE_FILES, names);
        }
    }
}