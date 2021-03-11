using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Xabe.FFmpeg;

namespace MyNAS.Util
{
    public static class VideoUtil
    {
        // public static void CreateThumbnail(string path, string output)
        // {
        //     var file = new FileInfo(path);
        //     if (file.Exists)
        //     {
        //         var process = new Process();

        //         process.StartInfo = new ProcessStartInfo("ffmpeg")
        //         {
        //             UseShellExecute = false,
        //             CreateNoWindow = true,
        //             Arguments = $"-i \"{path}\" -ss 1 -vframes 1 -r 1 -ac 1 -ab 2 -s 400*500 -f image2 \"{output}\"",
        //         };

        //         process.Start();
        //     }
        // }

        public static Task<byte[]> CreateThumbnail(byte[] contents)
        {
            using (var stream = new MemoryStream(contents))
            {
                return CreateThumbnail(stream);
            }
        }

        public static async Task<byte[]> CreateThumbnail(Stream stream)
        {
            byte[] result = null;
            var tempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".mp4");
            var tempOutput = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".jpg");
            using (var fileStream = File.Create(tempPath))
            {
                stream.Seek(0, SeekOrigin.Begin);
                await stream.CopyToAsync(fileStream);
            }

            IConversion conversion = await FFmpeg.Conversions.FromSnippet.Snapshot(tempPath, tempOutput, TimeSpan.FromSeconds(0));
            IConversionResult conversionResult = await conversion.Start();

            using (var outputStream = File.OpenRead(tempOutput))
            {
                result = new byte[outputStream.Length];
                await outputStream.ReadAsync(result, 0, result.Length);
            }

            File.Delete(tempPath);
            File.Delete(tempOutput);

            return result;
        }
    }
}