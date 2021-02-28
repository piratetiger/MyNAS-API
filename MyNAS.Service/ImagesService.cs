using System.Collections.Generic;
using System.Linq;
using MyNAS.Model;
using MyNAS.Model.Images;
using MyNAS.Service.Helper;

namespace MyNAS.Service
{
    public class ImagesService : ServiceBase
    {
        public List<ImageModel> GetList(GetListRequest req)
        {
            return DBAccessor.SearchItems<ImageModel>(Constants.TABLE_IMAGES, req);
        }

        public bool SaveItem(ImageModel item)
        {
            return DBAccessor.SaveItem(Constants.TABLE_IMAGES, item);
        }

        public bool SaveItems(List<ImageModel> items)
        {
            return DBAccessor.SaveItems(Constants.TABLE_IMAGES, items);
        }

        public bool DeleteItems(List<string> names)
        {
            if (names == null)
            {
                return false;
            }

            var deleteItems = names.Select(n => new ImageModel { FileName = n }).ToList();
            return DBAccessor.DeleteItems(Constants.TABLE_IMAGES, deleteItems);
        }

        public bool UpdateItems(List<ImageModel> items)
        {
            return DBAccessor.UpdateItems(Constants.TABLE_IMAGES, items);
        }

        public List<ImageModel> GetItems(List<string> names)
        {
            if (names == null)
            {
                return new List<ImageModel>();
            }

            return DBAccessor.GetItems<ImageModel>(Constants.TABLE_IMAGES, names);
        }
    }
}