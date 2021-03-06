using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyNAS.Model;
using MyNAS.Model.Images;
using MyNAS.Services.Abstraction;
using MyNAS.Services.LiteDBServices.Helper;

namespace MyNAS.Services.LiteDBServices
{
    public class ImagesService : LiteDBBaseService<IImagesService>, IImagesService
    {
        public Task<DataResult<ImageModel>> GetList(GetListRequest req)
        {
            var result = DBAccessor.SearchItems<ImageModel>(Constants.TABLE_IMAGES, req);
            return Task.FromResult(new DataResult<ImageModel>(Name, result));
        }

        public Task<DataResult<bool>> SaveItem(ImageModel item)
        {
            var result = DBAccessor.SaveItem(Constants.TABLE_IMAGES, item);
            return Task.FromResult(new DataResult<bool>(Name, new List<bool> { result }));
        }

        public Task<DataResult<bool>> SaveItems(List<ImageModel> items)
        {
            var result = DBAccessor.SaveItems(Constants.TABLE_IMAGES, items);
            return Task.FromResult(new DataResult<bool>(Name, new List<bool> { result }));
        }

        public Task<DataResult<bool>> DeleteItems(List<string> names)
        {
            if (names == null)
            {
                return Task.FromResult(new DataResult<bool>(Name, new List<bool>() { false }));
            }

            var deleteItems = names.Select(n => new ImageModel { FileName = n }).ToList();
            var result = DBAccessor.DeleteItems(Constants.TABLE_IMAGES, deleteItems);
            return Task.FromResult(new DataResult<bool>(Name, new List<bool> { result }));
        }

        public Task<DataResult<bool>> UpdateItems(List<ImageModel> items)
        {
            var result = DBAccessor.UpdateItems(Constants.TABLE_IMAGES, items);
            return Task.FromResult(new DataResult<bool>(Name, new List<bool> { result }));
        }

        public Task<DataResult<ImageModel>> GetItems(List<string> names)
        {
            if (names == null)
            {
                return Task.FromResult(new DataResult<ImageModel>(Name, null));
            }

            var result = DBAccessor.GetItems<ImageModel>(Constants.TABLE_IMAGES, names);
            return Task.FromResult(new DataResult<ImageModel>(Name, result));
        }
    }
}