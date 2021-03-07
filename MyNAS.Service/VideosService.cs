using System.Collections.Generic;
using System.Linq;
using MyNAS.Model;
using MyNAS.Model.Videos;
using MyNAS.Service.Helper;

namespace MyNAS.Service
{
    public class VideosService : ServiceBase
    {
        public List<VideoModel> GetList(GetListRequest req)
        {
            return DbAccessor.SearchItems<VideoModel>(Constants.TABLE_VIDEOS, req);
        }

        public bool SaveItem(VideoModel item)
        {
            return DbAccessor.SaveItem(Constants.TABLE_VIDEOS, item);
        }

        public bool SaveItems(List<VideoModel> items)
        {
            return DbAccessor.SaveItems(Constants.TABLE_VIDEOS, items);
        }

        public bool DeleteItems(List<string> names)
        {
            if (names == null)
            {
                return false;
            }

            var deleteItems = names.Select(n => new VideoModel { FileName = n }).ToList();
            return DbAccessor.DeleteItems(Constants.TABLE_VIDEOS, deleteItems);
        }

        public bool UpdateItems(List<VideoModel> items)
        {
            return DbAccessor.UpdateItems(Constants.TABLE_VIDEOS, items);
        }

        public List<VideoModel> GetItems(List<string> names)
        {
            if (names == null)
            {
                return new List<VideoModel>();
            }

            return DbAccessor.GetItems<VideoModel>(Constants.TABLE_VIDEOS, names);
        }
    }
}