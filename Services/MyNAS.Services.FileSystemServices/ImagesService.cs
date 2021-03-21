using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MyNAS.Model;
using MyNAS.Model.Images;
using MyNAS.Services.Abstraction;

namespace MyNAS.Services.FileSystemServices
{
    public class ImagesService : FileSystemBaseService<IImagesService>, IImagesService
    {
        protected static readonly string Image_Path;
        protected static readonly string Image_Thumb_Path;

        static ImagesService()
        {
            var basePath = Environment.CurrentDirectory;
            Image_Path = Path.Combine(basePath, "storage/images");
            Image_Thumb_Path = Path.Combine(basePath, "tmp");
            if (!Directory.Exists(Image_Path))
            {
                Directory.CreateDirectory(Image_Path);
            }
            if (!Directory.Exists(Image_Thumb_Path))
            {
                Directory.CreateDirectory(Image_Thumb_Path);
            }
        }

        public async Task<DataResult<bool>> SaveItem(ImageModel item)
        {
            var path = Path.Combine(Image_Path, item.FileName);
            var thumbPath = Path.Combine(Image_Thumb_Path, item.FileName);
            var success = false;

            try
            {
                if (item.Contents != null)
                {
                    await File.WriteAllBytesAsync(path, item.Contents);
                }
                if (item.ThumbContents != null)
                {
                    await File.WriteAllBytesAsync(thumbPath, item.ThumbContents);
                }

                success = true;
            }
            catch
            {
                success = false;
            }
            var result = new DataResult<bool>(Name, new List<bool>() { success });

            var next = Services.Next(this);
            if (next != null)
            {
                var nextResult = await next.SaveItem(item);
                result.Data = result.Data.Concat(nextResult.Data).ToList();
                result.Source = $"{result.Source};{nextResult.Source}";
            }

            return result;
        }

        public async Task<DataResult<bool>> SaveItems(IEnumerable<ImageModel> items)
        {
            var success = true;

            foreach (var item in items)
            {
                var path = Path.Combine(Image_Path, item.FileName);
                var thumbPath = Path.Combine(Image_Thumb_Path, item.FileName);

                try
                {
                    if (item.Contents != null)
                    {
                        await File.WriteAllBytesAsync(path, item.Contents);
                    }
                    if (item.ThumbContents != null)
                    {
                        await File.WriteAllBytesAsync(thumbPath, item.ThumbContents);
                    }
                }
                catch
                {
                    success = false;
                }
            }
            var result = new DataResult<bool>(Name, new List<bool>() { success });

            var next = Services.Next(this);
            if (next != null)
            {
                var nextResult = await next.SaveItems(items);
                result.Data = result.Data.Concat(nextResult.Data).ToList();
                result.Source = $"{result.Source};{nextResult.Source}";
            }

            return result;
        }

        public async Task<DataResult<byte[]>> GetItemContents(string name)
        {
            var path = Path.Combine(Image_Path, name);
            DataResult<byte[]> result = null;

            try
            {
                var bytes = await File.ReadAllBytesAsync(path);
                result = new DataResult<byte[]>(Name, new List<byte[]>() { bytes });
            }
            catch
            {
                result = new DataResult<byte[]>(Name, null);
            }

            if (result.First == null)
            {
                var next = Services.Next(this);
                if (next != null)
                {
                    result = await next.GetItemContents(name);
                }
            }

            return result;
        }

        public async Task<DataResult<byte[]>> GetItemThumbContents(string name)
        {
            var path = Path.Combine(Image_Thumb_Path, name);
            DataResult<byte[]> result = null;

            try
            {
                var bytes = await File.ReadAllBytesAsync(path);
                result = new DataResult<byte[]>(Name, new List<byte[]>() { bytes });
            }
            catch
            {
                result = new DataResult<byte[]>(Name, null);
            }

            if (result.First == null)
            {
                var next = Services.Next(this);
                if (next != null)
                {
                    result = await next.GetItemThumbContents(name);
                }
            }

            return result;
        }

        public async Task<DataResult<bool>> UpdateItemThumbContents(string name, byte[] contents)
        {
            var thumbPath = Path.Combine(Image_Thumb_Path, name);
            var success = false;

            try
            {
                if (contents != null)
                {
                    await File.WriteAllBytesAsync(thumbPath, contents);
                }

                success = true;
            }
            catch
            {
                success = false;
            }
            var result = new DataResult<bool>(Name, new List<bool>() { success });

            var next = Services.Next(this);
            if (next != null)
            {
                var nextResult = await next.UpdateItemThumbContents(name, contents);
                result.Data = result.Data.Concat(nextResult.Data).ToList();
                result.Source = $"{result.Source};{nextResult.Source}";
            }

            return result;
        }

        public async Task<DataResult<bool>> DeleteItems(IEnumerable<string> names)
        {
            var success = true;

            foreach (var name in names)
            {
                var path = Path.Combine(Image_Path, name);
                var thumbPath = Path.Combine(Image_Thumb_Path, name);

                try
                {
                    File.Delete(path);
                    File.Delete(thumbPath);
                }
                catch
                {
                    success = false;
                }
            }
            var result = new DataResult<bool>(Name, new List<bool>() { success });

            var next = Services.Next(this);
            if (next != null)
            {
                var nextResult = await next.DeleteItems(names);
                result.Data = result.Data.Concat(nextResult.Data).ToList();
                result.Source = $"{result.Source};{nextResult.Source}";
            }

            return result;
        }
    }
}