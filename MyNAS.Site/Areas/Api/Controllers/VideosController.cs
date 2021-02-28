using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyNAS.Model;
using MyNAS.Model.User;
using MyNAS.Model.Videos;
using MyNAS.Service;
using MyNAS.Site;
using MyNAS.Site.Helper;
using MyNAS.Util;

namespace MyNAS.Site.Areas.Api.Controllers
{
    [Area("Api")]
    [ApiController]
    [Route("[area]/[controller]")]
    [TypeFilter(typeof(CreateFolderAttribute), Arguments = new[] { "storage/videos" })]
    [TypeFilter(typeof(CreateFolderAttribute), Arguments = new[] { "tmp" })]
    public class VideosController : ControllerBase
    {
        private readonly IWebHostEnvironment _host;

        protected VideosService VideosService
        {
            get
            {
                return new VideosService();
            }
        }

        public VideosController(IWebHostEnvironment host)
        {
            _host = host;
        }

        [HttpPost("list")]
        public object GetVideoList(GetListRequest req)
        {
            var user = HttpContext.GetUser();
            var list = VideosService.GetList(req);
            if ((int)user.Role <= (int)UserRole.User)
            {
                list = list.Where(l => l.IsPublic || l.Owner == user.UserName).ToList();
            }
            return new DataResult<List<VideoModel>>(list);
        }

        [HttpGet("")]
        [AllowAnonymous]
        public ActionResult GetVideo(string name, bool thumb = true)
        {
            var path = Path.Combine(_host.WebRootPath, "storage/videos", name);
            if (thumb)
            {
                var thumbPath = Path.Combine(_host.WebRootPath, "tmp", name) + ".jpg";
                if (System.IO.File.Exists(thumbPath))
                {
                    return PhysicalFile(thumbPath, "image/jpeg");
                }
                else
                {
                    var defaultThumb = Path.Combine(_host.WebRootPath, "MP4thumb.jpg");
                    VideoUtil.CreateThumbnail(path, thumbPath);
                    return PhysicalFile(defaultThumb, "image/jpeg");
                }
            }
            else
            {
                return PhysicalFile(path, "video/mp4");
            }
        }

        [HttpPost("add")]
        [Authorize(Policy = "UserBase")]
        [RequestFormLimits(MultipartBodyLengthLimit = 419430400)]
        [RequestSizeLimit(419430400)]
        public object UploadVideo(IEnumerable<IFormFile> files, [FromForm] string date, [FromForm] bool isPublic)
        {
            var videoList = new List<VideoModel>();
            foreach (var file in files)
            {
                try
                {
                    var videoDate = string.IsNullOrEmpty(date) ? DateTime.Now : DateTime.ParseExact(date, "yyyyMMdd", null);
                    var fileName = $"{videoDate.ToString("yyyyMMdd")}_{Guid.NewGuid().ToString()}.mp4";
                    var path = Path.Combine(_host.WebRootPath, "storage/videos", fileName);
                    using (var fileStream = System.IO.File.Create(path))
                    {
                        using (var requestFileStream = file.OpenReadStream())
                        {
                            requestFileStream.Seek(0, SeekOrigin.Begin);
                            requestFileStream.CopyTo(fileStream);
                        }
                    }

                    var video = new VideoModel()
                    {
                        FileName = fileName,
                        Date = videoDate,
                        IsPublic = isPublic,
                        Owner = User.Identity.Name
                    };
                    videoList.Add(video);
                }
                catch { }
            }

            return new MessageDataResult("Upload Video", VideosService.SaveItems(videoList));
        }

        [HttpPost("updateDate")]
        [Authorize(Policy = "DataAdminBase")]
        public object UpdateVideoDate(UpdateRequest req)
        {
            var videoList = VideosService.GetItems(req.Names);

            if (req.NewModel != null)
            {
                foreach (var item in videoList)
                {
                    item.Date = req.NewModel.Date.Date;
                }
            }

            return new MessageDataResult("Update Video", VideosService.UpdateItems(videoList));
        }

        [HttpPost("delete")]
        [Authorize(Policy = "DataAdminBase")]
        public object DeleteVideo(DeleteRequest req)
        {
            foreach (var name in req.Names)
            {
                var path = Path.Combine(_host.WebRootPath, "storage/videos", name);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                var thumbPath = Path.Combine(_host.WebRootPath, "tmp", name) + ".jpg";
                if (System.IO.File.Exists(thumbPath))
                {
                    System.IO.File.Delete(thumbPath);
                }
            }

            return new MessageDataResult("Delete Video", VideosService.DeleteItems(req.Names));
        }
    }
}