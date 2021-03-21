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
            var result = DbAccessor.SearchItems<VideoInfoModel>(Constants.TABLE_VIDEOS, req)?.Select(m => NASInfoModel.FromModel<VideoInfoModel>(m));
            return Task.FromResult(new DataResult<VideoInfoModel>(Name, result));
        }

        public async Task<DataResult<bool>> SaveItem(VideoModel item)
        {
            var saveResult = DbAccessor.SaveItem(Constants.TABLE_VIDEOS, NASInfoModel.FromModel<VideoInfoModel>(item));
            var result = new DataResult<bool>(Name, new List<bool> { saveResult });

            var next = Services.Next(this);
            if (next != null)
            {
                var nextResult = await next.SaveItem(item);
                result.Data = result.Data.Concat(nextResult.Data);
                result.Source = $"{result.Source};{nextResult.Source}";
            }

            return result;
        }

        public async Task<DataResult<bool>> SaveItems(IEnumerable<VideoModel> items)
        {
            var saveResult = DbAccessor.SaveItems(Constants.TABLE_VIDEOS, items.Select(i => NASInfoModel.FromModel<VideoInfoModel>(i)));
            var result = new DataResult<bool>(Name, new List<bool> { saveResult });

            var next = Services.Next(this);
            if (next != null)
            {
                var nextResult = await next.SaveItems(items);
                result.Data = result.Data.Concat(nextResult.Data);
                result.Source = $"{result.Source};{nextResult.Source}";
            }

            return result;
        }

        public async Task<DataResult<bool>> DeleteItems(IEnumerable<string> names)
        {
            DataResult<bool> result = null;
            if (names == null)
            {
                result = new DataResult<bool>(Name, new List<bool>() { false });
            }
            else
            {
                var deleteItems = names.Select(n => new VideoInfoModel { FileName = n });
                var deleteResult = DbAccessor.DeleteItems(Constants.TABLE_VIDEOS, deleteItems);
                result = new DataResult<bool>(Name, new List<bool> { deleteResult });
            }

            var next = Services.Next(this);
            if (next != null)
            {
                var nextResult = await next.DeleteItems(names);
                result.Data = result.Data.Concat(nextResult.Data);
                result.Source = $"{result.Source};{nextResult.Source}";
            }

            return result;
        }

        public Task<DataResult<bool>> UpdateInfoList(IEnumerable<VideoInfoModel> items)
        {
            var result = DbAccessor.UpdateItems(Constants.TABLE_VIDEOS, items);
            return Task.FromResult(new DataResult<bool>(Name, new List<bool>() { result }));
        }

        public Task<DataResult<VideoInfoModel>> GetInfoList(IEnumerable<string> names)
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