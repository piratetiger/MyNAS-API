using System.Collections.Generic;
using System.Threading.Tasks;
using MyNAS.Model;
using MyNAS.Model.Files;
using MyNAS.Services.Abstraction.Helper;

namespace MyNAS.Services.Abstraction
{
    public interface IFilesService : ICollectionService<IFilesService>, IServiceBase
    {
        Task<DataResult<FileInfoModel>> GetInfoList(GetListRequest req)
        {
            var result = new DataResult<FileInfoModel>(Name, null, Constants.End_Of_Chain);
            var next = Services.Next(this);
            return next == null ? Task.FromResult(result) : next.GetInfoList(req);
        }

        Task<DataResult<bool>> SaveItem(FileModel item)
        {
            var result = new DataResult<bool>(Name, null, Constants.End_Of_Chain);
            var next = Services.Next(this);
            return next == null ? Task.FromResult(result) : next.SaveItem(item);
        }

        Task<DataResult<bool>> SaveItems(List<FileModel> items)
        {
            var result = new DataResult<bool>(Name, null, Constants.End_Of_Chain);
            var next = Services.Next(this);
            return next == null ? Task.FromResult(result) : next.SaveItems(items);
        }

        Task<DataResult<bool>> DeleteItems(List<string> names)
        {
            var result = new DataResult<bool>(Name, null, Constants.End_Of_Chain);
            var next = Services.Next(this);
            return next == null ? Task.FromResult(result) : next.DeleteItems(names);
        }

        Task<DataResult<bool>> UpdateInfoList(List<FileInfoModel> items)
        {
            var result = new DataResult<bool>(Name, null, Constants.End_Of_Chain);
            var next = Services.Next(this);
            return next == null ? Task.FromResult(result) : next.UpdateInfoList(items);
        }

        Task<DataResult<FileInfoModel>> GetInfo(string name)
        {
            var result = new DataResult<FileInfoModel>(Name, null, Constants.End_Of_Chain);
            var next = Services.Next(this);
            return next == null ? Task.FromResult(result) : next.GetInfo(name);
        }

        Task<DataResult<byte[]>> GetItemContents(FileInfoModel item)
        {
            var result = new DataResult<byte[]>(Name, null, Constants.End_Of_Chain);
            var next = Services.Next(this);
            return next == null ? Task.FromResult(result) : next.GetItemContents(item);
        }
    }
}