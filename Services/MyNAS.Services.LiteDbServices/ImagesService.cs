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

        public async Task<DataResult<bool>> SaveItem(ImageModel item)
        {
            var saveResult = DbAccessor.SaveItem(Constants.TABLE_IMAGES, item as ImageInfoModel);
            var result = new DataResult<bool>(Name, new List<bool> { saveResult });

            var next = Services.Next(this);
            if (next != null)
            {
                var nextResult = await next.SaveItem(item);
                result.Data = result.Data.Concat(nextResult.Data).ToList();
                result.Source = $"{result.Source};{nextResult.Source}";
            }

            return result;
        }

        public async Task<DataResult<bool>> SaveItems(IList<ImageModel> items)
        {
            var saveResult = DbAccessor.SaveItems(Constants.TABLE_IMAGES, items.Cast<ImageInfoModel>().ToList());
            var result = new DataResult<bool>(Name, new List<bool> { saveResult });

            var next = Services.Next(this);
            if (next != null)
            {
                var nextResult = await next.SaveItems(items);
                result.Data = result.Data.Concat(nextResult.Data).ToList();
                result.Source = $"{result.Source};{nextResult.Source}";
            }

            return result;
        }

        public async Task<DataResult<bool>> DeleteItems(IList<string> names)
        {
            DataResult<bool> result = null;
            if (names == null)
            {
                result = new DataResult<bool>(Name, new List<bool>() { false });
            }
            else
            {
                var deleteItems = names.Select(n => new ImageInfoModel { FileName = n }).ToList();
                var deleteResult = DbAccessor.DeleteItems(Constants.TABLE_IMAGES, deleteItems);
                result = new DataResult<bool>(Name, new List<bool> { deleteResult });
            }

            var next = Services.Next(this);
            if (next != null)
            {
                var nextResult = await next.DeleteItems(names);
                result.Data = result.Data.Concat(nextResult.Data).ToList();
                result.Source = $"{result.Source};{nextResult.Source}";
            }

            return result;
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