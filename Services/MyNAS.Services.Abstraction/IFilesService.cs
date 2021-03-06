using System.Collections.Generic;
using System.Threading.Tasks;
using MyNAS.Model;
using MyNAS.Model.Files;
using MyNAS.Services.Abstraction.Helper;

namespace MyNAS.Services.Abstraction
{
    public interface IFilesService : ICollectionService<IFilesService>, IServiceBase
    {
        Task<DataResult<FileModel>> GetList(GetListRequest req)
        {
            var result = new DataResult<FileModel>(Name, null, Constants.End_Of_Chain);
            var next = Services.Next(this);
            return next == null ? Task.FromResult(result) : next.GetList(req);
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

        Task<DataResult<bool>> UpdateItems(List<FileModel> items)
        {
            var result = new DataResult<bool>(Name, null, Constants.End_Of_Chain);
            var next = Services.Next(this);
            return next == null ? Task.FromResult(result) : next.UpdateItems(items);
        }

        Task<DataResult<FileModel>> GetItem(string name)
        {
            var result = new DataResult<FileModel>(Name, null, Constants.End_Of_Chain);
            var next = Services.Next(this);
            return next == null ? Task.FromResult(result) : next.GetItem(name);
        }

        Task<DataResult<FileModel>> GetItems(List<string> names)
        {
            var result = new DataResult<FileModel>(Name, null, Constants.End_Of_Chain);
            var next = Services.Next(this);
            return next == null ? Task.FromResult(result) : next.GetItems(names);
        }
    }
}