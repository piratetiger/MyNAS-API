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
            return DbAccessor.SearchItems<ImageModel>(Constants.TABLE_IMAGES, req);
        }

        public bool SaveItem(ImageModel item)
        {
            return DbAccessor.SaveItem(Constants.TABLE_IMAGES, item);
        }

        public bool SaveItems(List<ImageModel> items)
        {
            return DbAccessor.SaveItems(Constants.TABLE_IMAGES, items);
        }

        public bool DeleteItems(List<string> names)
        {
            if (names == null)
            {
                return false;
            }

            var deleteItems = names.Select(n => new ImageModel { FileName = n }).ToList();
            return DbAccessor.DeleteItems(Constants.TABLE_IMAGES, deleteItems);
        }

        public bool UpdateItems(List<ImageModel> items)
        {
            return DbAccessor.UpdateItems(Constants.TABLE_IMAGES, items);
        }

        public List<ImageModel> GetItems(List<string> names)
        {
            if (names == null)
            {
                return new List<ImageModel>();
            }

            return DbAccessor.GetItems<ImageModel>(Constants.TABLE_IMAGES, names);
        }
    }
}