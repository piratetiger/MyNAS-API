using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyNAS.Model;
using MyNAS.Model.User;
using MyNAS.Model.Videos;
using MyNAS.Services.Abstraction;
using MyNAS.Site.Helper;
using MyNAS.Util;

namespace MyNAS.Site.Areas.Api.Controllers
{
    [Area("Api")]
    [ApiController]
    [Route("[area]/[controller]")]
    public class VideosController : ControllerBase
    {
        private readonly IWebHostEnvironment _host;
        private readonly ServiceCollection<IVideosService> _videosServices;
        private IVideosService _videosService;
        protected IVideosService VideosService
        {
            get
            {
                if (_videosService == null)
                {
                    _videosServices.FilterOrder = this.GetServiceFilterOrder();
                    _videosService = _videosServices.First();
                }

                return _videosService;
            }
        }

        public VideosController(IWebHostEnvironment host, IEnumerable<IVideosService> videosServices)
        {
            _host = host;
            _videosServices = new ServiceCollection<IVideosService>(videosServices);
        }

        [HttpPost("list")]
        public async Task<object> GetVideoList(GetListRequest req)
        {
            var user = HttpContext.GetUser();
            var list = await VideosService.GetInfoList(req);
            if ((int)user.Role <= (int)UserRole.User)
            {
                list.Data = list.Data.Where(l => l.IsPublic || l.Owner == user.UserName).ToList();
            }
            return list;
        }

        [HttpGet("")]
        [AllowAnonymous]
        public async Task<IActionResult> GetVideo(string name, bool thumb = true)
        {
            if (thumb)
            {
                var bytes = (await VideosService.GetItemThumbContents(name)).First;
                if (bytes == null)
                {
                    bytes = (await VideosService.GetItemContents(name)).First;
                    if (bytes != null)
                    {
                        bytes = await VideoUtil.CreateThumbnail(bytes);
                        await VideosService.UpdateItemThumbContents(name, bytes);
                    }
                }
                return File(bytes, "image/jpeg");
            }
            else
            {
                var bytes = (await VideosService.GetItemContents(name)).First;
                return File(bytes, "video/mp4");
            }
        }

        [HttpPost("add")]
        [Authorize(Policy = "UserBase")]
        [RequestFormLimits(MultipartBodyLengthLimit = 419430400)]
        [RequestSizeLimit(419430400)]
        public async Task<object> UploadVideo(IEnumerable<IFormFile> files, [FromForm] string date, [FromForm] bool isPublic)
        {
            var videoList = new List<VideoModel>();
            foreach (var file in files)
            {
                try
                {
                    var videoDate = string.IsNullOrEmpty(date) ? DateTime.Now : DateTime.ParseExact(date, "yyyyMMdd", null);
                    var fileName = $"{videoDate.ToString("yyyyMMdd")}_{Guid.NewGuid().ToString()}.mp4";

                    var video = new VideoModel()
                    {
                        FileName = fileName,
                        Date = videoDate,
                        IsPublic = isPublic,
                        Owner = User.Identity.Name
                    };

                    using (var stream = file.OpenReadStream())
                    {
                        video.Contents = new byte[stream.Length];
                        await stream.ReadAsync(video.Contents, 0, video.Contents.Length);

                        stream.Seek(0, SeekOrigin.Begin);
                        video.ThumbContents = await VideoUtil.CreateThumbnail(stream);
                    }
                    videoList.Add(video);
                }
                catch { }
            }

            return new MessageDataResult(await VideosService.SaveItems(videoList), "Upload Video");
        }

        [HttpPost("updateDate")]
        [Authorize(Policy = "DataAdminBase")]
        public async Task<object> UpdateVideoDate(UpdateRequest req)
        {
            var videoList = await VideosService.GetInfoList(req.Names);

            if (req.NewModel != null && videoList.First != null)
            {
                foreach (var item in videoList.Data)
                {
                    item.Date = req.NewModel.Date.Date;
                }
            }

            return new MessageDataResult(await VideosService.UpdateInfoList(videoList.Data), "Update Video");
        }

        [HttpPost("delete")]
        [Authorize(Policy = "DataAdminBase")]
        public async Task<object> DeleteVideo(DeleteRequest req)
        {
            return new MessageDataResult(await VideosService.DeleteItems(req.Names), "Delete Video");
        }
    }
}