using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MyNAS.Model;
using MyNAS.Model.Files;
using MyNAS.Services.Abstraction;

namespace MyNAS.Services.FileSystemServices
{
    public class FilesService : FileSystemBaseService<IFilesService>, IFilesService
    {
        protected static readonly string File_Path;
        protected static readonly string File_Thumb_Path;

        static FilesService()
        {
            var basePath = Environment.CurrentDirectory;
            File_Path = Path.Combine(basePath, "storage/files");
            File_Thumb_Path = Path.Combine(basePath, "tmp");
            if (!Directory.Exists(File_Path))
            {
                Directory.CreateDirectory(File_Path);
            }
            if (!Directory.Exists(File_Thumb_Path))
            {
                Directory.CreateDirectory(File_Thumb_Path);
            }
        }

        public async Task<DataResult<bool>> SaveItem(FileModel item)
        {
            var success = false;

            if (item.IsFolder)
            {
                var path = Path.Combine(File_Path, item.PathName);

                try
                {
                    Directory.CreateDirectory(path);
                    success = true;
                }
                catch
                {
                    success = false;
                }
            }
            else
            {
                var path = Path.Combine(File_Path, item.PathName ?? string.Empty, item.KeyName);

                try
                {
                    if (item.Contents != null)
                    {
                        await File.WriteAllBytesAsync(path, item.Contents);
                    }
                    success = true;
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
                var nextResult = await next.SaveItem(item);
                result.Data = result.Data.Concat(nextResult.Data).ToList();
                result.Source = $"{result.Source};{nextResult.Source}";
            }

            return result;
        }

        public async Task<DataResult<bool>> SaveItems(IEnumerable<FileModel> items)
        {
            var success = true;

            foreach (var item in items.Where(i => !i.IsFolder))
            {
                var path = Path.Combine(File_Path, item.PathName ?? string.Empty, item.KeyName);
                // var thumbPath = Path.Combine(Image_Thumb_Path, item.FileName);

                try
                {
                    if (item.Contents != null)
                    {
                        await File.WriteAllBytesAsync(path, item.Contents);
                    }
                    // if (item.ThumbContents != null)
                    // {
                    //     await File.WriteAllBytesAsync(thumbPath, item.ThumbContents);
                    // }
                }
                catch
                {
                    success = false;
                }
            }
            foreach (var item in items.Where(i => i.IsFolder))
            {
                var path = Path.Combine(File_Path, item.PathName);

                try
                {
                    Directory.CreateDirectory(path);
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

        public async Task<DataResult<byte[]>> GetItemContents(FileInfoModel item)
        {
            var path = Path.Combine(File_Path, item.PathName ?? string.Empty, item.KeyName);
            var bytes = await File.ReadAllBytesAsync(path);
            var result = new DataResult<byte[]>(Name, new List<byte[]>() { bytes });

            return result;
        }
    }
}