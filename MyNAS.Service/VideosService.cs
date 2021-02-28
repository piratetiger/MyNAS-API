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
            return DBAccessor.SearchItems<VideoModel>(Constants.TABLE_VIDEOS, req);
        }

        public bool SaveItem(VideoModel item)
        {
            return DBAccessor.SaveItem(Constants.TABLE_VIDEOS, item);
        }

        public bool SaveItems(List<VideoModel> items)
        {
            return DBAccessor.SaveItems(Constants.TABLE_VIDEOS, items);
        }

        public bool DeleteItems(List<string> names)
        {
            if (names == null)
            {
                return false;
            }

            var deleteItems = names.Select(n => new VideoModel { FileName = n }).ToList();
            return DBAccessor.DeleteItems(Constants.TABLE_VIDEOS, deleteItems);
        }

        public bool UpdateItems(List<VideoModel> items)
        {
            return DBAccessor.UpdateItems(Constants.TABLE_VIDEOS, items);
        }

        public List<VideoModel> GetItems(List<string> names)
        {
            if (names == null)
            {
                return new List<VideoModel>();
            }

            return DBAccessor.GetItems<VideoModel>(Constants.TABLE_VIDEOS, names);
        }
    }
}