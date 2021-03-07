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
using MyNAS.Model.Files;
using MyNAS.Model.User;
using MyNAS.Services.Abstraction;
using MyNAS.Site.BackendServices;
using MyNAS.Site.Helper;

namespace MyNAS.Site.Areas.Api.Controllers
{
    [Area("Api")]
    [ApiController]
    [Route("[area]/[controller]")]
    public class FilesController : ControllerBase
    {
        private readonly IWebHostEnvironment _host;
        private readonly ITorrentDownloadService _btService;

        private readonly ServiceCollection<IFilesService> _filesServices;
        private IFilesService _filesService;
        protected IFilesService FilesService
        {
            get
            {
                if (_filesService == null)
                {
                    _filesServices.FilterOrder = this.GetServiceFilterOrder();
                    _filesService = _filesServices.First();
                }

                return _filesService;
            }
        }

        public FilesController(IWebHostEnvironment host, ITorrentDownloadService btService, IEnumerable<IFilesService> filesServices)
        {
            _host = host;
            _filesServices = new ServiceCollection<IFilesService>(filesServices);
            _btService = btService;
        }

        [HttpPost("list")]
        public async Task<object> GetFilesList(GetListRequest req)
        {
            var user = HttpContext.GetUser();
            var dataResult = await FilesService.GetList(req);
            dataResult.Data = dataResult.Data.OrderByDescending(f => f.IsFolder).ToList();
            if ((int)user.Role <= (int)UserRole.User)
            {
                dataResult.Data = dataResult.Data.Where(l => l.IsPublic || l.Owner == user.UserName).ToList();
            }
            return dataResult;
        }

        [HttpGet("")]
        [AllowAnonymous]
        public async Task<ActionResult> GetFile(string name)
        {
            var item = (await FilesService.GetItem(name)).First;
            var path = string.Empty;
            if (item != null)
            {
                path = Path.Combine(_host.WebRootPath, "storage/files", item.PathName ?? string.Empty, item.KeyName);
            }

            return PhysicalFile(path, "text/plain", item.FileName);
        }

        [HttpPost("add")]
        [Authorize(Policy = "UserBase")]
        [RequestFormLimits(MultipartBodyLengthLimit = 104857600)]
        [RequestSizeLimit(104857600)]
        public async Task<object> UploadFile(IEnumerable<IFormFile> files, [FromForm] string cate, [FromForm] bool isPublic)
        {
            var fileList = new List<FileModel>();
            var date = DateTime.Now;
            foreach (var file in files)
            {
                try
                {
                    var pathName = string.Empty;
                    if (!string.IsNullOrEmpty(cate))
                    {
                        var cateModel = (await FilesService.GetItem(cate)).First;
                        if (cateModel == null)
                        {
                            continue;
                        }
                        else
                        {
                            pathName = cateModel.PathName;
                        }
                    }
                    var keyName = $"{date.ToString("yyyyMMdd")}_{Guid.NewGuid().ToString()}";
                    var path = Path.Combine(_host.WebRootPath, "storage/files", pathName, keyName);
                    using (var fileStream = System.IO.File.Create(path))
                    {
                        using (var requestFileStream = file.OpenReadStream())
                        {
                            requestFileStream.Seek(0, SeekOrigin.Begin);
                            requestFileStream.CopyTo(fileStream);
                        }
                    }

                    var fileModel = new FileModel()
                    {
                        KeyName = keyName,
                        FileName = file.FileName,
                        Date = DateTime.Now,
                        IsPublic = isPublic,
                        Owner = User.Identity.Name,
                        Cate = string.IsNullOrEmpty(cate) ? null : cate,
                        PathName = pathName,
                        IsFolder = false
                    };
                    fileList.Add(fileModel);
                }
                catch { }
            }

            return new MessageDataResult(await FilesService.SaveItems(fileList), "Upload File");
        }

        [HttpPost("folder/add")]
        [Authorize(Policy = "UserBase")]
        public async Task<object> CreateFolder(CreateFolderRequest req)
        {
            var date = DateTime.Now;
            var success = true;
            try
            {
                var pathName = string.Empty;
                if (!string.IsNullOrEmpty(req.Cate))
                {
                    var cateModel = (await FilesService.GetItem(req.Cate)).First;
                    if (cateModel == null)
                    {
                        throw new ArgumentException();
                    }
                    else
                    {
                        pathName = cateModel.PathName;
                    }
                }
                var keyName = $"{date.ToString("yyyyMMdd")}_{Guid.NewGuid().ToString()}";
                var path = Path.Combine(_host.WebRootPath, "storage/files", pathName, req.Name);
                System.IO.Directory.CreateDirectory(path);

                var fileModel = new FileModel()
                {
                    KeyName = keyName,
                    FileName = req.Name,
                    Date = DateTime.Now,
                    IsPublic = req.IsPublic,
                    Owner = User.Identity.Name,
                    Cate = string.IsNullOrEmpty(req.Cate) ? null : req.Cate,
                    PathName = string.IsNullOrEmpty(pathName) ? req.Name : $"{pathName}/{req.Name}",
                    IsFolder = true
                };

                success = (await FilesService.SaveItem(fileModel)).First;
            }
            catch
            {
                success = false;
            }

            return new MessageDataResult(nameof(FilesController), success, "Create Folder");
        }

        [HttpPost("addbttask")]
        public object AddBtTask(AddBtTaskRequest req)
        {
            var success = true;
            try
            {
                if (string.IsNullOrEmpty(req.SavePath))
                {
                    req.SavePath = Path.Combine(_host.WebRootPath, "storage/files/downloads");
                }
                else
                {
                    req.SavePath = Path.Combine(_host.WebRootPath, "storage/files", req.SavePath);
                }
                _btService.Enqueue(req.SavePath, req.TorrentPath);
            }
            catch
            {
                success = false;
            }

            return new MessageDataResult(nameof(FilesController), success, "Add Bt Task");
        }

        // [HttpPost("status")]
        // public object GetStatus()
        // {
        //     return new DataResult<double[]>(_btService.Status());
        // }
    }
}