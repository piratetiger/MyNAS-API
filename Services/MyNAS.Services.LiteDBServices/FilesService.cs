using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyNAS.Model;
using MyNAS.Model.Files;
using MyNAS.Services.Abstraction;
using MyNAS.Services.LiteDbServices.Helper;

namespace MyNAS.Services.LiteDbServices
{
    public class FilesService : LiteDBBaseService<IFilesService>, IFilesService
    {
        public Task<DataResult<FileModel>> GetList(GetListRequest req)
        {
            var result = DBAccessor.SearchItems<FileModel>(Constants.TABLE_FILES, req);
            return Task.FromResult(new DataResult<FileModel>(Name, result));
        }

        public Task<DataResult<bool>> SaveItem(FileModel item)
        {
            var result = DBAccessor.SaveItem(Constants.TABLE_FILES, item);
            return Task.FromResult(new DataResult<bool>(Name, new List<bool>() { result }));
        }

        public Task<DataResult<bool>> SaveItems(List<FileModel> items)
        {
            var result = DBAccessor.SaveItems(Constants.TABLE_FILES, items);
            return Task.FromResult(new DataResult<bool>(Name, new List<bool>() { result }));
        }

        public Task<DataResult<bool>> DeleteItems(List<string> names)
        {
            if (names == null)
            {
                return Task.FromResult(new DataResult<bool>(Name, new List<bool>() { false }));
            }

            var deleteItems = names.Select(n => new FileModel { KeyName = n }).ToList();
            var result = DBAccessor.DeleteItems(Constants.TABLE_FILES, deleteItems);
            return Task.FromResult(new DataResult<bool>(Name, new List<bool>() { result }));
        }

        public Task<DataResult<bool>> UpdateItems(List<FileModel> items)
        {
            var result = DBAccessor.UpdateItems(Constants.TABLE_FILES, items);
            return Task.FromResult(new DataResult<bool>(Name, new List<bool>() { result }));
        }

        public Task<DataResult<FileModel>> GetItem(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return Task.FromResult(new DataResult<FileModel>(Name, null));
            }

            var result = DBAccessor.GetItem<FileModel>(Constants.TABLE_FILES, name);
            return Task.FromResult(new DataResult<FileModel>(Name, new List<FileModel>() { result }));
        }

        public Task<DataResult<FileModel>> GetItems(List<string> names)
        {
            if (names == null)
            {
                return Task.FromResult(new DataResult<FileModel>(Name, null));
            }

            var result = DBAccessor.GetItems<FileModel>(Constants.TABLE_FILES, names);
            return Task.FromResult(new DataResult<FileModel>(Name, result));
        }
    }
}