using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyNAS.Model;
using MyNAS.Model.Videos;
using MyNAS.Services.Abstraction;
using MyNAS.Services.LiteDbServices.Helper;

namespace MyNAS.Services.LiteDbServices
{
    public class VideoService : LiteDbBaseService<IVideosService>, IVideosService
    {
        public Task<DataResult<VideoInfoModel>> GetInfoList(GetListRequest req)
        {
            var result = DbAccessor.SearchItems<VideoInfoModel>(Constants.TABLE_VIDEOS, req);
            return Task.FromResult(new DataResult<VideoInfoModel>(Name, result));
        }

        public Task<DataResult<bool>> SaveItem(VideoModel item)
        {
            var result = DbAccessor.SaveItem(Constants.TABLE_VIDEOS, item as VideoInfoModel);
            return Task.FromResult(new DataResult<bool>(Name, new List<bool>() { result }));
        }

        public Task<DataResult<bool>> SaveItems(IList<VideoModel> items)
        {
            var result = DbAccessor.SaveItems(Constants.TABLE_VIDEOS, items.Cast<VideoInfoModel>().ToList());
            return Task.FromResult(new DataResult<bool>(Name, new List<bool>() { result }));
        }

        public Task<DataResult<bool>> DeleteItems(IList<string> names)
        {
            if (names == null)
            {
                return Task.FromResult(new DataResult<bool>(Name, new List<bool>() { false }));
            }

            var deleteItems = names.Select(n => new VideoModel { FileName = n }).ToList();
            var result = DbAccessor.DeleteItems(Constants.TABLE_VIDEOS, deleteItems);
            return Task.FromResult(new DataResult<bool>(Name, new List<bool>() { result }));
        }

        public Task<DataResult<bool>> UpdateInfoList(IList<VideoInfoModel> items)
        {
            var result = DbAccessor.UpdateItems(Constants.TABLE_VIDEOS, items);
            return Task.FromResult(new DataResult<bool>(Name, new List<bool>() { result }));
        }

        public Task<DataResult<VideoInfoModel>> GetInfoList(IList<string> names)
        {
            if (names == null)
            {
                return Task.FromResult(new DataResult<VideoInfoModel>(Name, null));
            }

            var result = DbAccessor.GetItems<VideoInfoModel>(Constants.TABLE_VIDEOS, names);
            return Task.FromResult(new DataResult<VideoInfoModel>(Name, result));
        }
    }
}