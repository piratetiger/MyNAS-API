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
            return DbAccessor.SearchItems<FileModel>(Constants.TABLE_FILES, req);
        }

        public bool SaveItem(FileModel item)
        {
            return DbAccessor.SaveItem(Constants.TABLE_FILES, item);
        }

        public bool SaveItems(List<FileModel> items)
        {
            return DbAccessor.SaveItems(Constants.TABLE_FILES, items);
        }

        public bool DeleteItems(List<string> names)
        {
            if (names == null)
            {
                return false;
            }

            var deleteItems = names.Select(n => new FileModel { KeyName = n }).ToList();
            return DbAccessor.DeleteItems(Constants.TABLE_FILES, deleteItems);
        }

        public bool UpdateItems(List<FileModel> items)
        {
            return DbAccessor.UpdateItems(Constants.TABLE_FILES, items);
        }

        public FileModel GetItem(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }

            return DbAccessor.GetItem<FileModel>(Constants.TABLE_FILES, name);
        }

        public List<FileModel> GetItems(List<string> names)
        {
            if (names == null)
            {
                return new List<FileModel>();
            }

            return DbAccessor.GetItems<FileModel>(Constants.TABLE_FILES, names);
        }
    }
}