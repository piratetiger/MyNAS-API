using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyNAS.Model;
using MyNAS.Model.Images;
using MyNAS.Model.User;
using MyNAS.Service;
using MyNAS.Site;
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

        protected ImagesService ImagesService
        {
            get
            {
                return new ImagesService();
            }
        }

        public ImagesController(IWebHostEnvironment host)
        {
            _host = host;
        }

        [HttpPost("list")]
        public object GetImageList(GetListRequest req)
        {
            var user = HttpContext.GetUser();
            var list = ImagesService.GetList(req);
            if ((int)user.Role <= (int)UserRole.User)
            {
                list = list.Where(l => l.IsPublic || l.Owner == user.UserName).ToList();
            }
            return new DataResult<List<ImageModel>>(list);
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
        public object UploadImage(IEnumerable<IFormFile> files, [FromForm] string date, [FromForm] bool isPublic)
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

            return new MessageDataResult("Upload Image", ImagesService.SaveItems(imageList));
        }

        [HttpPost("updateDate")]
        [Authorize(Policy = "DataAdminBase")]
        public object UpdateImageDate(UpdateRequest req)
        {
            var imageList = ImagesService.GetItems(req.Names);

            if (req.NewModel != null)
            {
                foreach (var item in imageList)
                {
                    item.Date = req.NewModel.Date.Date;
                }
            }

            return new MessageDataResult("Update Image", ImagesService.UpdateItems(imageList));
        }

        [HttpPost("delete")]
        [Authorize(Policy = "DataAdminBase")]
        public object DeleteImage(DeleteRequest req)
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

            return new MessageDataResult("Delete Video", ImagesService.DeleteItems(req.Names));
        }
    }
}