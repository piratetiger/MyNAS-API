using System.Collections.Generic;
using System.Threading.Tasks;
using MyNAS.Model;
using MyNAS.Model.Images;
using MyNAS.Services.Abstraction.Helper;

namespace MyNAS.Services.Abstraction
{
    public interface IImagesService : ICollectionService<IImagesService>, IServiceBase
    {
        Task<DataResult<ImageInfoModel>> GetInfoList(GetListRequest req)
        {
            var result = new DataResult<ImageInfoModel>(Name, null, Constants.End_Of_Chain);
            var next = Services.Next(this);
            return next == null ? Task.FromResult(result) : next.GetInfoList(req);
        }

        Task<DataResult<bool>> SaveItem(ImageModel item)
        {
            var result = new DataResult<bool>(Name, null, Constants.End_Of_Chain);
            var next = Services.Next(this);
            return next == null ? Task.FromResult(result) : next.SaveItem(item);
        }

        Task<DataResult<bool>> SaveItems(IEnumerable<ImageModel> items)
        {
            var result = new DataResult<bool>(Name, null, Constants.End_Of_Chain);
            var next = Services.Next(this);
            return next == null ? Task.FromResult(result) : next.SaveItems(items);
        }

        Task<DataResult<bool>> DeleteItems(IEnumerable<string> names)
        {
            var result = new DataResult<bool>(Name, null, Constants.End_Of_Chain);
            var next = Services.Next(this);
            return next == null ? Task.FromResult(result) : next.DeleteItems(names);
        }

        Task<DataResult<bool>> UpdateInfoList(IEnumerable<ImageInfoModel> items)
        {
            var result = new DataResult<bool>(Name, null, Constants.End_Of_Chain);
            var next = Services.Next(this);
            return next == null ? Task.FromResult(result) : next.UpdateInfoList(items);
        }

        Task<DataResult<ImageInfoModel>> GetInfoList(IEnumerable<string> names)
        {
            var result = new DataResult<ImageInfoModel>(Name, null, Constants.End_Of_Chain);
            var next = Services.Next(this);
            return next == null ? Task.FromResult(result) : next.GetInfoList(names);
        }

        Task<DataResult<byte[]>> GetItemContents(string name)
        {
            var result = new DataResult<byte[]>(Name, null, Constants.End_Of_Chain);
            var next = Services.Next(this);
            return next == null ? Task.FromResult(result) : next.GetItemContents(name);
        }

        Task<DataResult<byte[]>> GetItemThumbContents(string name)
        {
            var result = new DataResult<byte[]>(Name, null, Constants.End_Of_Chain);
            var next = Services.Next(this);
            return next == null ? Task.FromResult(result) : next.GetItemThumbContents(name);
        }

        Task<DataResult<bool>> UpdateItemThumbContents(string name, byte[] contents)
        {
            var result = new DataResult<bool>(Name, null, Constants.End_Of_Chain);
            var next = Services.Next(this);
            return next == null ? Task.FromResult(result) : next.UpdateItemThumbContents(name, contents);
        }
    }
}