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
using MyNAS.Model.Images;
using MyNAS.Model.User;
using MyNAS.Services.Abstraction;
using MyNAS.Site.Helper;
using MyNAS.Util;

namespace MyNAS.Site.Areas.Api.Controllers
{
    [Area("Api")]
    [ApiController]
    [Route("[area]/[controller]")]
    [TypeFilter(typeof(CreateFolderAttribute), Arguments = new[] { "storage/images" })]
    [TypeFilter(typeof(CreateFolderAttribute), Arguments = new[] { "tmp" })]
    public class ImagesController : ControllerBase
    {
        private readonly IWebHostEnvironment _host;
        private readonly ServiceCollection<IImagesService> _imagesServices;
        private IImagesService _imagesService;
        protected IImagesService ImagesService
        {
            get
            {
                if (_imagesService == null)
                {
                    _imagesServices.FilterOrder = this.GetServiceFilterOrder();
                    _imagesService = _imagesServices.First();
                }

                return _imagesService;
            }
        }

        public ImagesController(IWebHostEnvironment host, IEnumerable<IImagesService> imagesServices)
        {
            _host = host;
            _imagesServices = new ServiceCollection<IImagesService>(imagesServices);
        }

        [HttpPost("list")]
        public async Task<object> GetImageList(GetListRequest req)
        {
            var user = HttpContext.GetUser();
            var list = await ImagesService.GetInfoList(req);
            if ((int)user.Role <= (int)UserRole.User)
            {
                list.Data = list.Data.Where(l => l.IsPublic || l.Owner == user.UserName).ToList();
            }
            return list;
        }

        [HttpGet("")]
        [AllowAnonymous]
        public ActionResult GetImage(string name, bool thumb = true)
        {
            var path = Path.Combine(_host.WebRootPath, "storage/images", name);

            if (thumb)
            {
                var thumbPath = Path.Combine(_host.WebRootPath, "tmp", name);
                var thumbFile = new FileInfo(thumbPath);
                if (!thumbFile.Exists || thumbFile.Length == 0)
                {
                    using (var fileStream = System.IO.File.Create(thumbPath))
                    {
                        using (var thumbStream = ImageUtil.CreateThumbnail(path))
                        {
                            thumbStream.Seek(0, SeekOrigin.Begin);
                            thumbStream.CopyTo(fileStream);
                        }
                    }
                }
                return PhysicalFile(thumbPath, "image/jpeg");
            }
            else
            {
                return PhysicalFile(path, "image/jpeg");
            }
        }

        [HttpPost("add")]
        [Authorize(Policy = "UserBase")]
        [RequestFormLimits(MultipartBodyLengthLimit = 104857600)]
        [RequestSizeLimit(104857600)]
        public async Task<object> UploadImage(IEnumerable<IFormFile> files, [FromForm] string date, [FromForm] bool isPublic)
        {
            var imageList = new List<ImageModel>();
            foreach (var file in files)
            {
                try
                {
                    var imageDate = string.IsNullOrEmpty(date) ? DateTime.Now : DateTime.ParseExact(date, "yyyyMMdd", null);
                    var fileName = $"{imageDate.ToString("yyyyMMdd")}_{Guid.NewGuid().ToString()}.jpg";
                    var path = Path.Combine(_host.WebRootPath, "storage/images", fileName);
                    using (var fileStream = System.IO.File.Create(path))
                    {
                        using (var requestFileStream = file.OpenReadStream())
                        {
                            requestFileStream.Seek(0, SeekOrigin.Begin);
                            requestFileStream.CopyTo(fileStream);
                        }
                    }

                    var image = new ImageModel()
                    {
                        FileName = fileName,
                        Date = imageDate,
                        IsPublic = isPublic,
                        Owner = User.Identity.Name
                    };
                    imageList.Add(image);
                }
                catch { }
            }

            return new MessageDataResult(await ImagesService.SaveItems(imageList), "Upload Image");
        }

        [HttpPost("updateDate")]
        [Authorize(Policy = "DataAdminBase")]
        public async Task<object> UpdateImageDate(UpdateRequest req)
        {
            var imageList = await ImagesService.GetInfoList(req.Names);

            if (req.NewModel != null && imageList.First != null)
            {
                foreach (var item in imageList.Data)
                {
                    item.Date = req.NewModel.Date.Date;
                }
            }

            return new MessageDataResult(await ImagesService.UpdateInfoList(imageList.Data), "Update Image");
        }

        [HttpPost("delete")]
        [Authorize(Policy = "DataAdminBase")]
        public async Task<object> DeleteImage(DeleteRequest req)
        {
            foreach (var name in req.Names)
            {
                var path = Path.Combine(_host.WebRootPath, "storage/images", name);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                var thumbPath = Path.Combine(_host.WebRootPath, "tmp", name);
                if (System.IO.File.Exists(thumbPath))
                {
                    System.IO.File.Delete(thumbPath);
                }
            }

            return new MessageDataResult(await ImagesService.DeleteItems(req.Names), "Delete Video");
        }
    }
}