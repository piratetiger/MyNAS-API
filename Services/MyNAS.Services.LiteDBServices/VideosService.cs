using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyNAS.Model;
using MyNAS.Model.Videos;
using MyNAS.Services.Abstraction;
using MyNAS.Services.LiteDbServices.Helper;

namespace MyNAS.Services.LiteDbServices
{
    public class VideoService : LiteDBBaseService<IVideosService>, IVideosService
    {
        public Task<DataResult<VideoModel>> GetList(GetListRequest req)
        {
            var result = DBAccessor.SearchItems<VideoModel>(Constants.TABLE_VIDEOS, req);
            return Task.FromResult(new DataResult<VideoModel>(Name, result));
        }

        public Task<DataResult<bool>> SaveItem(VideoModel item)
        {
            var result = DBAccessor.SaveItem(Constants.TABLE_VIDEOS, item);
            return Task.FromResult(new DataResult<bool>(Name, new List<bool>() { result }));
        }

        public Task<DataResult<bool>> SaveItems(List<VideoModel> items)
        {
            var result = DBAccessor.SaveItems(Constants.TABLE_VIDEOS, items);
            return Task.FromResult(new DataResult<bool>(Name, new List<bool>() { result }));
        }

        public Task<DataResult<bool>> DeleteItems(List<string> names)
        {
            if (names == null)
            {
                return Task.FromResult(new DataResult<bool>(Name, new List<bool>() { false }));
            }

            var deleteItems = names.Select(n => new VideoModel { FileName = n }).ToList();
            var result = DBAccessor.DeleteItems(Constants.TABLE_VIDEOS, deleteItems);
            return Task.FromResult(new DataResult<bool>(Name, new List<bool>() { result }));
        }

        public Task<DataResult<bool>> UpdateItems(List<VideoModel> items)
        {
            var result = DBAccessor.UpdateItems(Constants.TABLE_VIDEOS, items);
            return Task.FromResult(new DataResult<bool>(Name, new List<bool>() { result }));
        }

        public Task<DataResult<VideoModel>> GetItems(List<string> names)
        {
            if (names == null)
            {
                return Task.FromResult(new DataResult<VideoModel>(Name, null));
            }

            var result = DBAccessor.GetItems<VideoModel>(Constants.TABLE_VIDEOS, names);
            return Task.FromResult(new DataResult<VideoModel>(Name, result));
        }
    }
}