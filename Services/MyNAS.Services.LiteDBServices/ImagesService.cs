using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyNAS.Model;
using MyNAS.Model.Images;
using MyNAS.Services.Abstraction;
using MyNAS.Services.LiteDbServices.Helper;

namespace MyNAS.Services.LiteDbServices
{
    public class ImagesService : LiteDbBaseService<IImagesService>, IImagesService
    {
        public Task<DataResult<ImageInfoModel>> GetInfoList(GetListRequest req)
        {
            var result = DbAccessor.SearchItems<ImageInfoModel>(Constants.TABLE_IMAGES, req);
            return Task.FromResult(new DataResult<ImageInfoModel>(Name, result));
        }

        public Task<DataResult<bool>> SaveItem(ImageModel item)
        {
            var result = DbAccessor.SaveItem(Constants.TABLE_IMAGES, item as ImageInfoModel);
            return Task.FromResult(new DataResult<bool>(Name, new List<bool> { result }));
        }

        public Task<DataResult<bool>> SaveItems(IList<ImageModel> items)
        {
            var result = DbAccessor.SaveItems(Constants.TABLE_IMAGES, items.Cast<ImageInfoModel>().ToList());
            return Task.FromResult(new DataResult<bool>(Name, new List<bool> { result }));
        }

        public Task<DataResult<bool>> DeleteItems(IList<string> names)
        {
            if (names == null)
            {
                return Task.FromResult(new DataResult<bool>(Name, new List<bool>() { false }));
            }

            var deleteItems = names.Select(n => new ImageModel { FileName = n }).ToList();
            var result = DbAccessor.DeleteItems(Constants.TABLE_IMAGES, deleteItems);
            return Task.FromResult(new DataResult<bool>(Name, new List<bool> { result }));
        }

        public Task<DataResult<bool>> UpdateInfoList(IList<ImageInfoModel> items)
        {
            var result = DbAccessor.UpdateItems(Constants.TABLE_IMAGES, items);
            return Task.FromResult(new DataResult<bool>(Name, new List<bool> { result }));
        }

        public Task<DataResult<ImageInfoModel>> GetInfoList(IList<string> names)
        {
            if (names == null)
            {
                return Task.FromResult(new DataResult<ImageInfoModel>(Name, null));
            }

            var result = DbAccessor.GetItems<ImageInfoModel>(Constants.TABLE_IMAGES, names);
            return Task.FromResult(new DataResult<ImageInfoModel>(Name, result));
        }
    }
}