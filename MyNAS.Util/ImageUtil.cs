using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace MyNAS.Util
{
    public static class ImageUtil
    {
        public static byte[] CreateThumbnail(string path)
        {
            if (File.Exists(path))
            {
                using (var fileStream = File.OpenRead(path))
                {
                    return CreateThumbnail(fileStream);
                }
            }

            return null;
        }

        public static byte[] CreateThumbnail(Stream stream)
        {
            MemoryStream result = new MemoryStream();
            Image image = Image.FromStream(stream, false);
            Image thumbImage = image.GetThumbnailImage(400, 500, null, System.IntPtr.Zero);
            thumbImage.Save(result, ImageFormat.Jpeg);
            result.Position = 0;

            return result.ToArray();
        }

        public static byte[] CreateThumbnail(byte[] contents)
        {
            using (var stream = new MemoryStream(contents))
            {
                return CreateThumbnail(stream);
            }
        }
    }
}