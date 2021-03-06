using System.Collections.Generic;
using System.Threading.Tasks;
using MyNAS.Model;
using MyNAS.Model.Videos;
using MyNAS.Services.Abstraction.Helper;

namespace MyNAS.Services.Abstraction
{
    public interface IVideosService : ICollectionService<IVideosService>, IServiceBase
    {
        Task<DataResult<VideoModel>> GetList(GetListRequest req)
        {
            var result = new DataResult<VideoModel>(Name, null, Constants.End_Of_Chain);
            var next = Services.Next(this);
            return next == null ? Task.FromResult(result) : next.GetList(req);
        }

        Task<DataResult<bool>> SaveItem(VideoModel item)
        {
            var result = new DataResult<bool>(Name, null, Constants.End_Of_Chain);
            var next = Services.Next(this);
            return next == null ? Task.FromResult(result) : next.SaveItem(item);
        }

        Task<DataResult<bool>> SaveItems(IList<VideoModel> items)
        {
            var result = new DataResult<bool>(Name, null, Constants.End_Of_Chain);
            var next = Services.Next(this);
            return next == null ? Task.FromResult(result) : next.SaveItems(items);
        }

        Task<DataResult<bool>> DeleteItems(IList<string> names)
        {
            var result = new DataResult<bool>(Name, null, Constants.End_Of_Chain);
            var next = Services.Next(this);
            return next == null ? Task.FromResult(result) : next.DeleteItems(names);
        }

        Task<DataResult<bool>> UpdateItems(IList<VideoModel> items)
        {
            var result = new DataResult<bool>(Name, null, Constants.End_Of_Chain);
            var next = Services.Next(this);
            return next == null ? Task.FromResult(result) : next.UpdateItems(items);
        }

        Task<DataResult<VideoModel>> GetItems(IList<string> names)
        {
            var result = new DataResult<VideoModel>(Name, null, Constants.End_Of_Chain);
            var next = Services.Next(this);
            return next == null ? Task.FromResult(result) : next.GetItems(names);
        }
    }
}