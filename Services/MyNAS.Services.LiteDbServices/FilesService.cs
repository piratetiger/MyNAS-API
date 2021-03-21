using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyNAS.Model;
using MyNAS.Model.Files;
using MyNAS.Services.Abstraction;
using MyNAS.Services.LiteDbServices.Helper;

namespace MyNAS.Services.LiteDbServices
{
    public class FilesService : LiteDbBaseService<IFilesService>, IFilesService
    {
        public Task<DataResult<FileInfoModel>> GetInfoList(GetListRequest req)
        {
            var result = DbAccessor.SearchItems<FileInfoModel>(Constants.TABLE_FILES, req);
            return Task.FromResult(new DataResult<FileInfoModel>(Name, result));
        }

        public async Task<DataResult<bool>> SaveItem(FileModel item)
        {
            var saveResult = DbAccessor.SaveItem(Constants.TABLE_FILES, NASInfoModel.FromModel<FileInfoModel>(item));
            var result = new DataResult<bool>(Name, new List<bool>() { saveResult });

            var next = Services.Next(this);
            if (next != null)
            {
                var nextResult = await next.SaveItem(item);
                result.Data = result.Data.Concat(nextResult.Data);
                result.Source = $"{result.Source};{nextResult.Source}";
            }

            return result;
        }

        public async Task<DataResult<bool>> SaveItems(IEnumerable<FileModel> items)
        {
            var saveResult = DbAccessor.SaveItems(Constants.TABLE_FILES, items.Select(i => NASInfoModel.FromModel<FileInfoModel>(i)));
            var result = new DataResult<bool>(Name, new List<bool>() { saveResult });

            var next = Services.Next(this);
            if (next != null)
            {
                var nextResult = await next.SaveItems(items);
                result.Data = result.Data.Concat(nextResult.Data);
                result.Source = $"{result.Source};{nextResult.Source}";
            }

            return result;
        }

        public Task<DataResult<bool>> DeleteItems(IEnumerable<string> names)
        {
            if (names == null)
            {
                return Task.FromResult(new DataResult<bool>(Name, new List<bool>() { false }));
            }

            var deleteItems = names.Select(n => new FileInfoModel { KeyName = n });
            var result = DbAccessor.DeleteItems(Constants.TABLE_FILES, deleteItems);
            return Task.FromResult(new DataResult<bool>(Name, new List<bool>() { result }));
        }

        public Task<DataResult<bool>> UpdateInfoList(IEnumerable<FileInfoModel> items)
        {
            var result = DbAccessor.UpdateItems(Constants.TABLE_FILES, items);
            return Task.FromResult(new DataResult<bool>(Name, new List<bool>() { result }));
        }

        public Task<DataResult<FileInfoModel>> GetInfo(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return Task.FromResult(new DataResult<FileInfoModel>(Name, null));
            }

            var result = DbAccessor.GetItem<FileInfoModel>(Constants.TABLE_FILES, name);
            return Task.FromResult(new DataResult<FileInfoModel>(Name, new List<FileInfoModel>() { result }));
        }
    }
}