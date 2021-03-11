using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MyNAS.Model;
using MyNAS.Model.Videos;
using MyNAS.Services.Abstraction;

namespace MyNAS.Services.FileSystemServices
{
    public class VideosService : FileSystemBaseService<IVideosService>, IVideosService
    {
        protected static readonly string Video_Path;
        protected static readonly string Video_Thumb_Path;

        static VideosService()
        {
            var basePath = Environment.CurrentDirectory;
            Video_Path = Path.Combine(basePath, "storage/videos");
            Video_Thumb_Path = Path.Combine(basePath, "tmp");
            if (!Directory.Exists(Video_Path))
            {
                Directory.CreateDirectory(Video_Path);
            }
            if (!Directory.Exists(Video_Thumb_Path))
            {
                Directory.CreateDirectory(Video_Thumb_Path);
            }
        }

        public async Task<DataResult<bool>> SaveItem(VideoModel item)
        {
            var path = Path.Combine(Video_Path, item.FileName);
            var thumbPath = Path.ChangeExtension(Path.Combine(Video_Thumb_Path, item.FileName), ".jpg");
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

        public async Task<DataResult<bool>> SaveItems(IList<VideoModel> items)
        {
            var success = true;

            foreach (var item in items)
            {
                var path = Path.Combine(Video_Path, item.FileName);
                var thumbPath = Path.ChangeExtension(Path.Combine(Video_Thumb_Path, item.FileName), ".jpg");

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
            var path = Path.Combine(Video_Path, name);
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
            var path = Path.ChangeExtension(Path.Combine(Video_Thumb_Path, name), ".jpg");
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
            var thumbPath = Path.ChangeExtension(Path.Combine(Video_Thumb_Path, name), ".jpg");
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

        public async Task<DataResult<bool>> DeleteItems(IList<string> names)
        {
            var success = true;

            foreach (var name in names)
            {
                var path = Path.Combine(Video_Path, name);
                var thumbPath = Path.ChangeExtension(Path.Combine(Video_Thumb_Path, name), ".jpg");

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